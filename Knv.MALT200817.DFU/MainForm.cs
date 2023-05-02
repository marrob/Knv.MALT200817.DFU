
namespace Knv.MALT200817.DFU
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Diagnostics;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Properties;

    public interface IMainForm
    {

        event EventHandler Shown;
        event FormClosedEventHandler FormClosed;
        event FormClosingEventHandler FormClosing;

        event EventHandler Disposed;

        event EventHandler WriteEventHandler;
        event EventHandler DeviceRestart;
        event EventHandler FileBrowseEventHandler;

        string Text { get; set; }
        string FileName { get; set; }

        int PoregressValue { get; set; }
        bool LogEnable { get; set; }

        byte CardAddress { get; set; }

        string LabelStatus { get; set; }

        bool WriteEnabled { get; set; }

        int LastWriteTimeMs { get; set; }
        uint Baudrate { get; set; }

        //ToolStripItem[] MenuBar { set; }
        //ToolStripItem[] StatusBar { set; }
        //bool AlwaysOnTop { get; set; }


        //event KeyEventHandler KeyUp;
        //event HelpEventHandler HelpRequested; /*????*/

        //void CursorWait();
        //void CursorDefault();
    }

    public partial class MainForm : Form, IMainForm
    {
        public event EventHandler DeviceRestart
        {
            add { buttonRestart.Click += value; }
            remove { buttonRestart.Click -= value; }
        }

        public event EventHandler WriteEventHandler
        {
            add { buttonWrite.Click += value; }
            remove { buttonWrite.Click -= value; }
        }
        public event EventHandler FileBrowseEventHandler
        {
            add { buttonBrowse.Click += value; }
            remove { buttonBrowse.Click -= value; }
        }

        public bool LogEnable{
            get { return checkBoxLogEnable.Checked; }
            set { checkBoxLogEnable.Checked = value; }
        }

        public int PoregressValue
        {
            get { return ProgressBar.Value; }
            set { ProgressBar.Value = value; }                   
        }

        public byte CardAddress {
            get { return (byte)NumericUpDownAddress.Value; }
            set { NumericUpDownAddress.Value = value; }
        }

        public string FileName
        {
            get { return textFileName.Text; }
            set { textFileName.Text = value; }
        }
        
        public string LabelStatus {
            get { return labelStatus.Text; }
            set { labelStatus.Text = value; }
        }

        bool wrtieEnabled;
        public bool WriteEnabled
        {
            get { return wrtieEnabled; }
            set 
            {
                buttonWrite.Enabled = value;
                wrtieEnabled = value; 
            }
        }

        public int LastWriteTimeMs
        {
            get { return int.Parse(toolStripStatusLabelLastWrite.Text);  }
            set { toolStripStatusLabelLastWrite.Text = value.ToString(); }
        }

        public uint Baudrate
        {
            get { return uint.Parse(comboBox1.SelectedValue.ToString()); }
            set { comboBox1.SelectedItem = MUDS150628.Constants.Baudrate.FirstOrDefault(n => n.Value == value); }
        }

        public MainForm() 
        {
            InitializeComponent();   
            comboBox1.DataSource = new BindingSource(MUDS150628.Constants.Baudrate, null);
            comboBox1.DisplayMember = "Key";
            comboBox1.ValueMember = "Value";
        }

        private void buttonOpenLog_Click(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(Settings.Default.LogPath))
                Process.Start(Settings.Default.LogPath);
        }

        private void buttonRestart_Click(object sender, EventArgs e)
        {

        }
    }
}
