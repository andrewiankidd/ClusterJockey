using System;
using System.Drawing;
using System.Windows.Forms;

namespace ClusterJockey
{
    public enum LogLevel
    {
        Success,
        Warning,
        Error,
        Info
    }

    public class Logger
    {
        private readonly ListView _consoleOutputListView;

        public Logger(ListView consoleOutputListView)
        {
            _consoleOutputListView = consoleOutputListView;

            if (_consoleOutputListView.View != View.Details)
            {
                _consoleOutputListView.View = View.Details;
            }

            if (_consoleOutputListView.Columns.Count < 3)
            {
                _consoleOutputListView.Columns.Clear();
                int totalWidth = _consoleOutputListView.Width;
                _consoleOutputListView.Columns.Add("Timestamp", (int)(totalWidth * 0.1));
                _consoleOutputListView.Columns.Add("Source", (int)(totalWidth * 0.1));
                _consoleOutputListView.Columns.Add("Message", (int)(totalWidth * 0.7));
                _consoleOutputListView.Columns.Add("Level", (int)(totalWidth * 0.1));
            }
        }

        public void Log(string source, string message, LogLevel level = LogLevel.Info)
        {
            if (_consoleOutputListView.InvokeRequired)
                _consoleOutputListView.Invoke(new Action(() => Append(source, message, level)));
            else
                Append(source, message, level);
        }

        private void Append(string source, string message, LogLevel level)
        {
            var item = new ListViewItem(DateTime.Now.ToString());
            item.SubItems.Add(source.ToString());
            item.SubItems.Add(message.ToString());
            item.SubItems.Add(level.ToString().ToUpper());
            item.ForeColor = GetColor(level);
            _consoleOutputListView.Items.Insert(0, item);
            _consoleOutputListView.EnsureVisible(0);
        }

        private Color GetColor(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Success: return Color.Green;
                case LogLevel.Warning: return Color.Orange;
                case LogLevel.Error: return Color.Red;
                default: return Color.Black;
            }
        }
    }
}
