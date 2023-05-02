
namespace Knv.MALT200817.DFU
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Forms;
    using System.Threading;
    using Properties;
    using System.Diagnostics;
    using Knv.MUDS150628;
    using Knv.MUDS150628.Common;
    using Knv.MUDS150628.NiCanApi;
    using System.Drawing;
    using System.Runtime.InteropServices;

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run((MainForm)new App().MainForm);
        }
    }

    class App
    {
        public static SynchronizationContext SyncContext = null;

        public IMainForm MainForm { get; set; }

        ICanInterface CanLink;

        public App()
        {

            if (Settings.Default.ApplictionSettingsSaveCounter == 0)
            {
                Settings.Default.Upgrade();
                Settings.Default.ApplictionSettingsUpgradeCounter++;
            }
            Settings.Default.ApplictionSettingsSaveCounter++;
            Settings.Default.PropertyChanged += (s, e) =>
            {
                Debug.WriteLine(GetType().Namespace + "." + GetType().Name + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "(): " +
                    e.PropertyName + ", NewValue: " + Settings.Default[e.PropertyName]);
            };

            Settings.Default.SettingsLoaded += (s, e) =>
            {
                Debug.WriteLine("SettingsLoaded");
            };

            Settings.Default.SettingChanging += (s, e) =>
            {
                Debug.WriteLine(GetType().Namespace + "." + GetType().Name + "." +
                    System.Reflection.MethodBase.GetCurrentMethod().Name + "()");
            };

         

            MainForm = new MainForm();
            MainForm.Text = AppConstants.SoftwareTitle;

            MainForm.FileBrowseEventHandler += ButtonBrowse_Click;
            MainForm.FormClosed += MainForm_FormClosed;
            MainForm.WriteEventHandler += ButtonWrite_Click;
            MainForm.Shown += MainForm_Shown;

            MainForm.DeviceRestart += MainForm_DeviceRestart;
        }

        private void MainForm_DeviceRestart(object sender, EventArgs e)
        {
            using (var canLink = new NiCanApiInterface())
            {
                /*** Restart module ***/
                /* Globális címen külödk célzottan a kártynak egy Restart parancsot */
                canLink.Init(baudrate: 250000, "CAN0");
                canLink.WriteFrame(new CanMsg[] { new CanMsg(0x1558FFFF | 0x20000000, new byte[] { 0xAA, MainForm.CardAddress }) });
                canLink.Dispose();
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            SyncContext = SynchronizationContext.Current;

            MainForm.CardAddress = (byte)Settings.Default.Address;
            MainForm.LogEnable = Settings.Default.LogEnable;
            MainForm.Baudrate = (uint)Settings.Default.Baudrate;

            if (!string.IsNullOrWhiteSpace(Settings.Default.LastPath))
                if(System.IO.File.Exists(Settings.Default.LastPath))
                   MainForm.FileName = System.IO.Path.GetFileName(Settings.Default.LastPath);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.Address = MainForm.CardAddress;
            Settings.Default.LogEnable = MainForm.LogEnable;
            Settings.Default.Baudrate = MainForm.Baudrate;
            Settings.Default.Save();
        }

        private void ButtonWrite_Click(object sender, EventArgs e)
        {
            byte address = MainForm.CardAddress;

            CanLink = new NiCanApiInterface();
            CanLink.Init(baudrate: 250000, "CAN0");
            MainForm.WriteEnabled = false;

            var network = new Iso15765NetwrorkLayer(CanLink);
            network.ReceiveId = (UInt32)0x700 | address;
            network.TransmittId = (UInt32)0x600 | address; 
            network.ReadTimeoutMs = 1000;
            network.Log = Settings.Default.LogEnable;
            Settings.Default.LogPath  = AppConstants.LogPath + "_" + DateTime.Now.ToString(AppConstants.FileNameTimestampFormat) + ".txt";
            network.LogPath = Settings.Default.LogPath;

            var dfu = new AppDfu(network);
            
            dfu.ProgressChange += (o, ev) =>
            {
                MainForm.PoregressValue= ev.ProgressPercentage;
                MainForm.LabelStatus = ev.UserState.ToString();
            };

            try
            {
                byte[] firmware = Tools.OpenFile(Settings.Default.LastPath);
                dfu.Begin(firmware);

                Action syncCompleted = () =>
                {

                };

                dfu.Completed += (o, ev) =>
                {
                    CanLink.Dispose();
                    MainForm.WriteEnabled = true;

                    if (App.SyncContext != null)
                        App.SyncContext.Post((e1) => { syncCompleted(); }, null);

                    if (ev.Result is Exception)
                        MessageBox.Show((ev.Result as Exception).Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                };
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ButtonBrowse_Click(object sender, EventArgs e)
        {
            var ofd =  new OpenFileDialog();
            if (string.IsNullOrEmpty(Settings.Default.LastPath))
                ofd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                ofd.InitialDirectory = Settings.Default.LastPath;
            ofd.Filter = AppConstants.FileFilter;
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                Settings.Default.LastPath = ofd.FileName;
                MainForm.FileName = System.IO.Path.GetFileName(Settings.Default.LastPath);
            }
        }
    }
        
}
