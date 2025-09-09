namespace ClusterJockey
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            mainFormSplitContainer = new SplitContainer();
            mainFormSplitContainerWindowsLinux = new SplitContainer();
            linuxGroupBox = new GroupBox();
            mainFormLinuxSplitContainer = new SplitContainer();
            linuxTextBox = new RichTextBox();
            mainFormLinuxToolStrip = new ToolStrip();
            linuxGroupBoxCreateButton = new ToolStripButton();
            linuxGroupBoxStartButton = new ToolStripButton();
            linuxGroupBoxStopButton = new ToolStripButton();
            linuxGroupBoxDeleteButton = new ToolStripButton();
            linuxCommandInputTextBox = new TextBox();
            windowsGroupBox = new GroupBox();
            mainFormWindowsSplitContainer = new SplitContainer();
            windowsTextBox = new RichTextBox();
            mainFormWindowsToolStrip = new ToolStrip();
            windowsGroupBoxCreateButton = new ToolStripButton();
            windowsGroupBoxStartButton = new ToolStripButton();
            windowsGroupBoxStopButton = new ToolStripButton();
            windowsGroupBoxDeleteButton = new ToolStripButton();
            windowsCommandInputTextBox = new TextBox();
            mainFormToolStrip = new ToolStrip();
            mainFormToolStripFileButton = new ToolStripDropDownButton();
            mainFormToolStripFileExitButton = new ToolStripMenuItem();
            mainFormToolStripToolsButton = new ToolStripDropDownButton();
            mainFormToolStripViewMultipassButton = new ToolStripMenuItem();
            mainFormToolStripViewHyperVButton = new ToolStripMenuItem();
            mainFormToolStripViewButton = new ToolStripDropDownButton();
            mainFormToolStripViewConsoleButton = new ToolStripMenuItem();
            consoleOutputListView = new ListView();
            ((System.ComponentModel.ISupportInitialize)mainFormSplitContainer).BeginInit();
            mainFormSplitContainer.Panel1.SuspendLayout();
            mainFormSplitContainer.Panel2.SuspendLayout();
            mainFormSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainFormSplitContainerWindowsLinux).BeginInit();
            mainFormSplitContainerWindowsLinux.Panel1.SuspendLayout();
            mainFormSplitContainerWindowsLinux.Panel2.SuspendLayout();
            mainFormSplitContainerWindowsLinux.SuspendLayout();
            linuxGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainFormLinuxSplitContainer).BeginInit();
            mainFormLinuxSplitContainer.Panel1.SuspendLayout();
            mainFormLinuxSplitContainer.Panel2.SuspendLayout();
            mainFormLinuxSplitContainer.SuspendLayout();
            mainFormLinuxToolStrip.SuspendLayout();
            windowsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)mainFormWindowsSplitContainer).BeginInit();
            mainFormWindowsSplitContainer.Panel1.SuspendLayout();
            mainFormWindowsSplitContainer.Panel2.SuspendLayout();
            mainFormWindowsSplitContainer.SuspendLayout();
            mainFormWindowsToolStrip.SuspendLayout();
            mainFormToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainFormSplitContainer
            // 
            mainFormSplitContainer.Dock = DockStyle.Fill;
            mainFormSplitContainer.Location = new Point(0, 0);
            mainFormSplitContainer.Margin = new Padding(6);
            mainFormSplitContainer.Name = "mainFormSplitContainer";
            mainFormSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // mainFormSplitContainer.Panel1
            // 
            mainFormSplitContainer.Panel1.Controls.Add(mainFormSplitContainerWindowsLinux);
            mainFormSplitContainer.Panel1.Controls.Add(mainFormToolStrip);
            // 
            // mainFormSplitContainer.Panel2
            // 
            mainFormSplitContainer.Panel2.Controls.Add(consoleOutputListView);
            mainFormSplitContainer.Size = new Size(1486, 960);
            mainFormSplitContainer.SplitterDistance = 427;
            mainFormSplitContainer.SplitterWidth = 9;
            mainFormSplitContainer.TabIndex = 2;
            // 
            // mainFormSplitContainerWindowsLinux
            // 
            mainFormSplitContainerWindowsLinux.Dock = DockStyle.Fill;
            mainFormSplitContainerWindowsLinux.Location = new Point(0, 42);
            mainFormSplitContainerWindowsLinux.Name = "mainFormSplitContainerWindowsLinux";
            // 
            // mainFormSplitContainerWindowsLinux.Panel1
            // 
            mainFormSplitContainerWindowsLinux.Panel1.Controls.Add(linuxGroupBox);
            // 
            // mainFormSplitContainerWindowsLinux.Panel2
            // 
            mainFormSplitContainerWindowsLinux.Panel2.Controls.Add(windowsGroupBox);
            mainFormSplitContainerWindowsLinux.Size = new Size(1486, 385);
            mainFormSplitContainerWindowsLinux.SplitterDistance = 743;
            mainFormSplitContainerWindowsLinux.TabIndex = 1;
            // 
            // linuxGroupBox
            // 
            linuxGroupBox.Controls.Add(mainFormLinuxSplitContainer);
            linuxGroupBox.Dock = DockStyle.Fill;
            linuxGroupBox.Location = new Point(0, 0);
            linuxGroupBox.Name = "linuxGroupBox";
            linuxGroupBox.Size = new Size(743, 385);
            linuxGroupBox.TabIndex = 0;
            linuxGroupBox.TabStop = false;
            linuxGroupBox.Text = "Linux VM";
            // 
            // mainFormLinuxSplitContainer
            // 
            mainFormLinuxSplitContainer.Dock = DockStyle.Fill;
            mainFormLinuxSplitContainer.FixedPanel = FixedPanel.Panel2;
            mainFormLinuxSplitContainer.Location = new Point(3, 35);
            mainFormLinuxSplitContainer.Name = "mainFormLinuxSplitContainer";
            mainFormLinuxSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // mainFormLinuxSplitContainer.Panel1
            // 
            mainFormLinuxSplitContainer.Panel1.Controls.Add(linuxTextBox);
            mainFormLinuxSplitContainer.Panel1.Controls.Add(mainFormLinuxToolStrip);
            // 
            // mainFormLinuxSplitContainer.Panel2
            // 
            mainFormLinuxSplitContainer.Panel2.Controls.Add(linuxCommandInputTextBox);
            mainFormLinuxSplitContainer.Size = new Size(737, 347);
            mainFormLinuxSplitContainer.SplitterDistance = 292;
            mainFormLinuxSplitContainer.TabIndex = 2;
            // 
            // linuxTextBox
            // 
            linuxTextBox.Dock = DockStyle.Fill;
            linuxTextBox.Location = new Point(0, 42);
            linuxTextBox.Name = "linuxTextBox";
            linuxTextBox.ReadOnly = true;
            linuxTextBox.Size = new Size(737, 250);
            linuxTextBox.TabIndex = 2;
            linuxTextBox.Text = "";
            linuxTextBox.WordWrap = false;
            // 
            // mainFormLinuxToolStrip
            // 
            mainFormLinuxToolStrip.ImageScalingSize = new Size(32, 32);
            mainFormLinuxToolStrip.Items.AddRange(new ToolStripItem[] { linuxGroupBoxCreateButton, linuxGroupBoxStartButton, linuxGroupBoxStopButton, linuxGroupBoxDeleteButton });
            mainFormLinuxToolStrip.Location = new Point(0, 0);
            mainFormLinuxToolStrip.Name = "mainFormLinuxToolStrip";
            mainFormLinuxToolStrip.Size = new Size(737, 42);
            mainFormLinuxToolStrip.TabIndex = 1;
            // 
            // linuxGroupBoxCreateButton
            // 
            linuxGroupBoxCreateButton.Image = Properties.Resources.add_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            linuxGroupBoxCreateButton.ImageTransparentColor = Color.Magenta;
            linuxGroupBoxCreateButton.Name = "linuxGroupBoxCreateButton";
            linuxGroupBoxCreateButton.Size = new Size(119, 36);
            linuxGroupBoxCreateButton.Text = "Create";
            linuxGroupBoxCreateButton.Click += linuxGroupBoxCreateButton_Click;
            // 
            // linuxGroupBoxStartButton
            // 
            linuxGroupBoxStartButton.Image = Properties.Resources.play_circle_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            linuxGroupBoxStartButton.ImageTransparentColor = Color.Magenta;
            linuxGroupBoxStartButton.Name = "linuxGroupBoxStartButton";
            linuxGroupBoxStartButton.Size = new Size(98, 36);
            linuxGroupBoxStartButton.Text = "Start";
            linuxGroupBoxStartButton.Click += linuxGroupBoxStartButton_Click;
            // 
            // linuxGroupBoxStopButton
            // 
            linuxGroupBoxStopButton.Image = Properties.Resources.cancel_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            linuxGroupBoxStopButton.ImageTransparentColor = Color.Magenta;
            linuxGroupBoxStopButton.Name = "linuxGroupBoxStopButton";
            linuxGroupBoxStopButton.Size = new Size(98, 36);
            linuxGroupBoxStopButton.Text = "Stop";
            linuxGroupBoxStopButton.Click += linuxGroupBoxStopButton_Click;
            // 
            // linuxGroupBoxDeleteButton
            // 
            linuxGroupBoxDeleteButton.Image = Properties.Resources.delete_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            linuxGroupBoxDeleteButton.ImageTransparentColor = Color.Magenta;
            linuxGroupBoxDeleteButton.Name = "linuxGroupBoxDeleteButton";
            linuxGroupBoxDeleteButton.Size = new Size(120, 36);
            linuxGroupBoxDeleteButton.Text = "Delete";
            linuxGroupBoxDeleteButton.Click += linuxGroupBoxDeleteButton_Click;
            // 
            // linuxCommandInputTextBox
            // 
            linuxCommandInputTextBox.Dock = DockStyle.Fill;
            linuxCommandInputTextBox.Location = new Point(0, 0);
            linuxCommandInputTextBox.Multiline = true;
            linuxCommandInputTextBox.Name = "linuxCommandInputTextBox";
            linuxCommandInputTextBox.Size = new Size(737, 51);
            linuxCommandInputTextBox.TabIndex = 0;
            linuxCommandInputTextBox.KeyPress += linuxCommandInputTextBox_KeyPress;
            // 
            // windowsGroupBox
            // 
            windowsGroupBox.Controls.Add(mainFormWindowsSplitContainer);
            windowsGroupBox.Dock = DockStyle.Fill;
            windowsGroupBox.Location = new Point(0, 0);
            windowsGroupBox.Name = "windowsGroupBox";
            windowsGroupBox.Size = new Size(739, 385);
            windowsGroupBox.TabIndex = 0;
            windowsGroupBox.TabStop = false;
            windowsGroupBox.Text = "Windows VM";
            // 
            // mainFormWindowsSplitContainer
            // 
            mainFormWindowsSplitContainer.Dock = DockStyle.Fill;
            mainFormWindowsSplitContainer.FixedPanel = FixedPanel.Panel2;
            mainFormWindowsSplitContainer.Location = new Point(3, 35);
            mainFormWindowsSplitContainer.Name = "mainFormWindowsSplitContainer";
            mainFormWindowsSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // mainFormWindowsSplitContainer.Panel1
            // 
            mainFormWindowsSplitContainer.Panel1.Controls.Add(windowsTextBox);
            mainFormWindowsSplitContainer.Panel1.Controls.Add(mainFormWindowsToolStrip);
            // 
            // mainFormWindowsSplitContainer.Panel2
            // 
            mainFormWindowsSplitContainer.Panel2.Controls.Add(windowsCommandInputTextBox);
            mainFormWindowsSplitContainer.Size = new Size(733, 347);
            mainFormWindowsSplitContainer.SplitterDistance = 295;
            mainFormWindowsSplitContainer.TabIndex = 4;
            // 
            // windowsTextBox
            // 
            windowsTextBox.Dock = DockStyle.Fill;
            windowsTextBox.Location = new Point(0, 42);
            windowsTextBox.Name = "windowsTextBox";
            windowsTextBox.ReadOnly = true;
            windowsTextBox.Size = new Size(733, 253);
            windowsTextBox.TabIndex = 3;
            windowsTextBox.Text = "";
            windowsTextBox.WordWrap = false;
            // 
            // mainFormWindowsToolStrip
            // 
            mainFormWindowsToolStrip.ImageScalingSize = new Size(32, 32);
            mainFormWindowsToolStrip.Items.AddRange(new ToolStripItem[] { windowsGroupBoxCreateButton, windowsGroupBoxStartButton, windowsGroupBoxStopButton, windowsGroupBoxDeleteButton });
            mainFormWindowsToolStrip.Location = new Point(0, 0);
            mainFormWindowsToolStrip.Name = "mainFormWindowsToolStrip";
            mainFormWindowsToolStrip.Size = new Size(733, 42);
            mainFormWindowsToolStrip.TabIndex = 2;
            // 
            // windowsGroupBoxCreateButton
            // 
            windowsGroupBoxCreateButton.Image = Properties.Resources.add_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            windowsGroupBoxCreateButton.ImageTransparentColor = Color.Magenta;
            windowsGroupBoxCreateButton.Name = "windowsGroupBoxCreateButton";
            windowsGroupBoxCreateButton.Size = new Size(119, 36);
            windowsGroupBoxCreateButton.Text = "Create";
            windowsGroupBoxCreateButton.Click += windowsGroupBoxCreateButton_Click;
            // 
            // windowsGroupBoxStartButton
            // 
            windowsGroupBoxStartButton.Image = Properties.Resources.play_circle_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            windowsGroupBoxStartButton.ImageTransparentColor = Color.Magenta;
            windowsGroupBoxStartButton.Name = "windowsGroupBoxStartButton";
            windowsGroupBoxStartButton.Size = new Size(98, 36);
            windowsGroupBoxStartButton.Text = "Start";
            windowsGroupBoxStartButton.Click += windowsGroupBoxStartButton_Click;
            // 
            // windowsGroupBoxStopButton
            // 
            windowsGroupBoxStopButton.Image = Properties.Resources.cancel_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            windowsGroupBoxStopButton.ImageTransparentColor = Color.Magenta;
            windowsGroupBoxStopButton.Name = "windowsGroupBoxStopButton";
            windowsGroupBoxStopButton.Size = new Size(98, 36);
            windowsGroupBoxStopButton.Text = "Stop";
            windowsGroupBoxStopButton.Click += windowsGroupBoxStopButton_Click;
            // 
            // windowsGroupBoxDeleteButton
            // 
            windowsGroupBoxDeleteButton.Image = Properties.Resources.delete_24dp_E3E3E3_FILL0_wght400_GRAD0_opsz24;
            windowsGroupBoxDeleteButton.ImageTransparentColor = Color.Magenta;
            windowsGroupBoxDeleteButton.Name = "windowsGroupBoxDeleteButton";
            windowsGroupBoxDeleteButton.Size = new Size(120, 36);
            windowsGroupBoxDeleteButton.Text = "Delete";
            windowsGroupBoxDeleteButton.Click += windowsGroupBoxDeleteButton_Click;
            // 
            // windowsCommandInputTextBox
            // 
            windowsCommandInputTextBox.Dock = DockStyle.Fill;
            windowsCommandInputTextBox.Location = new Point(0, 0);
            windowsCommandInputTextBox.Multiline = true;
            windowsCommandInputTextBox.Name = "windowsCommandInputTextBox";
            windowsCommandInputTextBox.Size = new Size(733, 48);
            windowsCommandInputTextBox.TabIndex = 0;
            windowsCommandInputTextBox.KeyPress += windowsCommandInputTextBox_KeyPress;
            // 
            // mainFormToolStrip
            // 
            mainFormToolStrip.ImageScalingSize = new Size(32, 32);
            mainFormToolStrip.Items.AddRange(new ToolStripItem[] { mainFormToolStripFileButton, mainFormToolStripToolsButton, mainFormToolStripViewButton });
            mainFormToolStrip.Location = new Point(0, 0);
            mainFormToolStrip.Name = "mainFormToolStrip";
            mainFormToolStrip.Padding = new Padding(0, 0, 4, 0);
            mainFormToolStrip.Size = new Size(1486, 42);
            mainFormToolStrip.TabIndex = 0;
            mainFormToolStrip.Text = "toolStrip1";
            // 
            // mainFormToolStripFileButton
            // 
            mainFormToolStripFileButton.DropDownItems.AddRange(new ToolStripItem[] { mainFormToolStripFileExitButton });
            mainFormToolStripFileButton.ImageTransparentColor = Color.Magenta;
            mainFormToolStripFileButton.Name = "mainFormToolStripFileButton";
            mainFormToolStripFileButton.Size = new Size(73, 36);
            mainFormToolStripFileButton.Text = "File";
            mainFormToolStripFileButton.ToolTipText = "File";
            // 
            // mainFormToolStripFileExitButton
            // 
            mainFormToolStripFileExitButton.Name = "mainFormToolStripFileExitButton";
            mainFormToolStripFileExitButton.Size = new Size(184, 44);
            mainFormToolStripFileExitButton.Text = "Exit";
            mainFormToolStripFileExitButton.Click += mainFormToolStripFileExitButton_Click;
            // 
            // mainFormToolStripToolsButton
            // 
            mainFormToolStripToolsButton.DropDownItems.AddRange(new ToolStripItem[] { mainFormToolStripViewMultipassButton, mainFormToolStripViewHyperVButton });
            mainFormToolStripToolsButton.ImageTransparentColor = Color.Magenta;
            mainFormToolStripToolsButton.Name = "mainFormToolStripToolsButton";
            mainFormToolStripToolsButton.Size = new Size(91, 36);
            mainFormToolStripToolsButton.Text = "Tools";
            mainFormToolStripToolsButton.ToolTipText = "View";
            // 
            // mainFormToolStripViewMultipassButton
            // 
            mainFormToolStripViewMultipassButton.Name = "mainFormToolStripViewMultipassButton";
            mainFormToolStripViewMultipassButton.Size = new Size(394, 44);
            mainFormToolStripViewMultipassButton.Text = "Open Multipass";
            mainFormToolStripViewMultipassButton.Click += mainFormToolStripViewMultipassButton_Click;
            // 
            // mainFormToolStripViewHyperVButton
            // 
            mainFormToolStripViewHyperVButton.Name = "mainFormToolStripViewHyperVButton";
            mainFormToolStripViewHyperVButton.Size = new Size(394, 44);
            mainFormToolStripViewHyperVButton.Text = "Open HyperV Manager";
            mainFormToolStripViewHyperVButton.Click += mainFormToolStripViewHyperVButton_Click;
            // 
            // mainFormToolStripViewButton
            // 
            mainFormToolStripViewButton.DropDownItems.AddRange(new ToolStripItem[] { mainFormToolStripViewConsoleButton });
            mainFormToolStripViewButton.ImageTransparentColor = Color.Magenta;
            mainFormToolStripViewButton.Name = "mainFormToolStripViewButton";
            mainFormToolStripViewButton.Size = new Size(87, 36);
            mainFormToolStripViewButton.Text = "View";
            mainFormToolStripViewButton.ToolTipText = "View";
            // 
            // mainFormToolStripViewConsoleButton
            // 
            mainFormToolStripViewConsoleButton.Name = "mainFormToolStripViewConsoleButton";
            mainFormToolStripViewConsoleButton.Size = new Size(312, 44);
            mainFormToolStripViewConsoleButton.Text = "Toggle Console";
            mainFormToolStripViewConsoleButton.Click += mainFormToolStripViewConsoleButton_Click;
            // 
            // consoleOutputListView
            // 
            consoleOutputListView.Dock = DockStyle.Fill;
            consoleOutputListView.FullRowSelect = true;
            consoleOutputListView.GridLines = true;
            consoleOutputListView.Location = new Point(0, 0);
            consoleOutputListView.Margin = new Padding(6);
            consoleOutputListView.Name = "consoleOutputListView";
            consoleOutputListView.Size = new Size(1486, 524);
            consoleOutputListView.TabIndex = 2;
            consoleOutputListView.UseCompatibleStateImageBehavior = false;
            consoleOutputListView.View = View.Details;
            consoleOutputListView.DoubleClick += consoleOutputListView_DoubleClick;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1486, 960);
            Controls.Add(mainFormSplitContainer);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(6);
            Name = "MainForm";
            Text = "ClusterJockey";
            mainFormSplitContainer.Panel1.ResumeLayout(false);
            mainFormSplitContainer.Panel1.PerformLayout();
            mainFormSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainFormSplitContainer).EndInit();
            mainFormSplitContainer.ResumeLayout(false);
            mainFormSplitContainerWindowsLinux.Panel1.ResumeLayout(false);
            mainFormSplitContainerWindowsLinux.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainFormSplitContainerWindowsLinux).EndInit();
            mainFormSplitContainerWindowsLinux.ResumeLayout(false);
            linuxGroupBox.ResumeLayout(false);
            mainFormLinuxSplitContainer.Panel1.ResumeLayout(false);
            mainFormLinuxSplitContainer.Panel1.PerformLayout();
            mainFormLinuxSplitContainer.Panel2.ResumeLayout(false);
            mainFormLinuxSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mainFormLinuxSplitContainer).EndInit();
            mainFormLinuxSplitContainer.ResumeLayout(false);
            mainFormLinuxToolStrip.ResumeLayout(false);
            mainFormLinuxToolStrip.PerformLayout();
            windowsGroupBox.ResumeLayout(false);
            mainFormWindowsSplitContainer.Panel1.ResumeLayout(false);
            mainFormWindowsSplitContainer.Panel1.PerformLayout();
            mainFormWindowsSplitContainer.Panel2.ResumeLayout(false);
            mainFormWindowsSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)mainFormWindowsSplitContainer).EndInit();
            mainFormWindowsSplitContainer.ResumeLayout(false);
            mainFormWindowsToolStrip.ResumeLayout(false);
            mainFormWindowsToolStrip.PerformLayout();
            mainFormToolStrip.ResumeLayout(false);
            mainFormToolStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer mainFormSplitContainer;
        private ListView consoleOutputListView;
        private ToolStrip mainFormToolStrip;
        private ToolStripDropDownButton mainFormToolStripViewButton;
        private ToolStripDropDownButton mainFormToolStripFileButton;
        private ToolStripMenuItem mainFormToolStripFileExitButton;
        private ToolStripMenuItem mainFormToolStripViewConsoleButton;
        private SplitContainer mainFormSplitContainerWindowsLinux;
        private GroupBox linuxGroupBox;
        private GroupBox windowsGroupBox;
        private ToolStrip mainFormLinuxToolStrip;
        private ToolStripButton linuxGroupBoxCreateButton;
        private ToolStrip mainFormWindowsToolStrip;
        private ToolStripButton windowsGroupBoxCreateButton;
        private ToolStripButton linuxGroupBoxStartButton;
        private ToolStripButton linuxGroupBoxStopButton;
        private ToolStripButton linuxGroupBoxDeleteButton;
        private ToolStripButton windowsGroupBoxStartButton;
        private ToolStripButton windowsGroupBoxStopButton;
        private ToolStripButton windowsGroupBoxDeleteButton;
        private ToolStripDropDownButton mainFormToolStripToolsButton;
        private ToolStripMenuItem mainFormToolStripViewMultipassButton;
        private ToolStripMenuItem mainFormToolStripViewHyperVButton;
        private RichTextBox linuxTextBox;
        private RichTextBox windowsTextBox;
        private SplitContainer mainFormWindowsSplitContainer;
        private SplitContainer mainFormLinuxSplitContainer;
        private TextBox linuxCommandInputTextBox;
        private TextBox windowsCommandInputTextBox;
    }
}
