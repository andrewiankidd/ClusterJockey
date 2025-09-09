# Run as Administrator on Windows Server
param (
    [string]$MasterIP = "10.0.0.5",  # Replace with your actual control plane IP
    [string]$JoinToken = "abcdef.0123456789abcdef",
    [string]$CACertHash = "sha256:0123456789abcdef0123456789abcdef0123456789abcdef0123456789abcd",  # Replace with actual CA cert hash
    [string]$K8sVersion = "v1.29.0",
    [string]$PodNetworkCidr = "10.244.0.0/16"
)

function Log($msg) {
    Write-Host "[INFO] $msg"
}

Log "Enabling Windows container feature..."
Install-WindowsFeature containers
Install-WindowsFeature -Name Microsoft-Hyper-V -IncludeAllSubFeature -Restart:$false

Log "Installing container runtime..."
Invoke-WebRequest "https://github.com/containerd/containerd/releases/download/v1.7.11/containerd-1.7.11-windows-amd64.tar.gz" -OutFile "containerd.tar.gz"
tar -xf containerd.tar.gz -C "C:\Program Files\containerd"
Remove-Item containerd.tar.gz

sc.exe create containerd binPath= "C:\Program Files\containerd\containerd.exe" start= auto
Start-Service containerd

Log "Installing Kubernetes binaries..."
$baseUrl = "https://dl.k8s.io/$K8sVersion/bin/windows/amd64"
New-Item -Path "C:\k" -ItemType Directory -Force | Out-Null
Invoke-WebRequest "$baseUrl/kubelet.exe" -OutFile "C:\k\kubelet.exe"
Invoke-WebRequest "$baseUrl/kubectl.exe" -OutFile "C:\k\kubectl.exe"
Invoke-WebRequest "$baseUrl/kube-proxy.exe" -OutFile "C:\k\kube-proxy.exe"

Log "Creating kubelet service..."
New-Item -Path 'C:\k\kubelet.conf' -ItemType File -Force
New-Item -Path 'C:\var\lib\kubelet' -ItemType Directory -Force

sc.exe create kubelet binPath= "C:\k\kubelet.exe --config=C:\k\kubelet-config.yaml --kubeconfig=C:\k\kubelet.conf" start= auto
Set-Service kubelet -StartupType Automatic

Log "Writing kubelet config..."
@"
kind: KubeletConfiguration
apiVersion: kubelet.config.k8s.io/v1beta1
cgroupDriver: "none"
clusterDNS:
  - 10.96.0.10
clusterDomain: "cluster.local"
"@ | Out-File -Encoding ASCII C:\k\kubelet-config.yaml

Log "Writing kube-proxy config..."
@"
apiVersion: kubeproxy.config.k8s.io/v1alpha1
kind: KubeProxyConfiguration
mode: kernelspace
clusterCIDR: "$PodNetworkCidr"
"@ | Out-File -Encoding ASCII C:\k\kube-proxy.yaml

Log "Writing kube-proxy service..."
sc.exe create kube-proxy binPath= "C:\k\kube-proxy.exe --config=C:\k\kube-proxy.yaml --v=4" start= auto
Set-Service kube-proxy -StartupType Automatic

Log "Generating bootstrap kubeconfig..."
& "C:\k\kubectl.exe" config set-cluster cluster --server=https://$($MasterIP):6443 --insecure-skip-tls-verify
& "C:\k\kubectl.exe" config set-credentials bootstrap --token=$($JoinToken)
& "C:\k\kubectl.exe" config set-context bootstrap --cluster=cluster --user=bootstrap
& "C:\k\kubectl.exe" config use-context bootstrap

Log "Copying kubeconfig to kubelet.conf..."
Copy-Item "C:\Users\Public\.kube\config" -Destination "C:\k\kubelet.conf" -Force

Log "Starting kubelet and kube-proxy services..."
Start-Service kubelet
Start-Service kube-proxy

Log "Node setup complete. Verify on control plane: kubectl get nodes"
