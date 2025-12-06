using HITS.LIB.WeakEvents;
using System.Diagnostics.Eventing.Reader;

namespace WebcamImageCapture
{
    public partial class Form1 : Form
    {
        private List<string> fileList;
        private ImageViewModel model = new ImageViewModel();
        private CaptureService captureService = new();
        EventMgr eventMgr = new EventMgr();

        public Form1()
        {
            try
            {
                InitializeComponent();
                pictureBox1.DataBindings.Add("ImageLocation", model, "ImageLocation", true, DataSourceUpdateMode.OnPropertyChanged);
                tbImage.DataBindings.Add("Value", model, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
                GetImages();
                eventMgr.SubscribeToEvent(this, OnWeakEvent);
                tbFrequency.Value = 3;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbAuto.Checked = Properties.Settings.Default.AutoShowLast;
            tbPath.Text = Properties.Settings.Default.CapturePath;
            tbFrequency.Value = Properties.Settings.Default.SnapshotFrequency;
            lblFrequency.Text = $"Snapshot Frequency {tbFrequency.Value} seconds";
            if (Properties.Settings.Default.WebcamIndex == 0)
            {
                rbWebcam0.Checked = true;
            }
            else if (Properties.Settings.Default.WebcamIndex == 1)
            {
                rbWebcam1.Checked = true;
            }
            else if (Properties.Settings.Default.WebcamIndex == 2)
            {
                rbWebcam2.Checked = true;
            }
            cbResolution.SelectedIndex = Properties.Settings.Default.ResolutionIndex;
            if (Properties.Settings.Default.DiskLimit == 0)
            {
                Properties.Settings.Default.DiskLimit = 100 * CaptureService.MEGABYTE;
            }
            nudDiskLimit.Value = Properties.Settings.Default.DiskLimit / CaptureService.MEGABYTE;
            lblDirSize.Text = (CaptureService.GetDirectorySize(Properties.Settings.Default.CapturePath) / CaptureService.MEGABYTE).ToString("G") + " MB";
            ServiceStopped();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            eventMgr.UnsubscribeAll(this);
            captureService?.StopAsync(new CancellationToken());
            captureService?.Dispose();
            int webcamIndex = 0;
            if (rbWebcam1.Checked) webcamIndex = 1;
            else if (rbWebcam2.Checked) webcamIndex = 2;

            Properties.Settings.Default.AutoShowLast = cbAuto.Checked;
            Properties.Settings.Default.CapturePath = tbPath.Text;
            Properties.Settings.Default.SnapshotFrequency = tbFrequency.Value;
            Properties.Settings.Default.WebcamIndex = webcamIndex;
            Properties.Settings.Default.ResolutionIndex = cbResolution.SelectedIndex;
            Properties.Settings.Default.DiskLimit = (long)nudDiskLimit.Value * CaptureService.MEGABYTE;
            Properties.Settings.Default.Save();
        }

        private void ServiceRunning()
        {
            SafeInvoke(() =>
            {
                rbWebcam0.Enabled = false;
                rbWebcam1.Enabled = false;
                rbWebcam2.Enabled = false;
                cbResolution.Enabled = false;
                tbFrequency.Enabled = false;
                cbAuto.Enabled = true;
                butStopService.Enabled = true;
                butStartService.Enabled = false;
                tbPath.Enabled = false;
                tbImage.Enabled = true;
                DeleteAllButton.Enabled = false;
                PreviousButton.Enabled = true;
                NextButton.Enabled = true;
                nudDiskLimit.Enabled = false;
            });
        }

        private void ServiceStopped()
        {
            SafeInvoke(() =>
            {
                rbWebcam0.Enabled = true;
                rbWebcam1.Enabled = true;
                rbWebcam2.Enabled = true;
                cbResolution.Enabled = true;
                tbFrequency.Enabled = true;
                cbAuto.Enabled = true;
                butStopService.Enabled = false;
                butStartService.Enabled = true;
                tbPath.Enabled = true;
                tbImage.Enabled = true;
                DeleteAllButton.Enabled = true;
                PreviousButton.Enabled = true;
                NextButton.Enabled = true;
                nudDiskLimit.Enabled = true;
            });
        }

        void OnWeakEvent(object sender, StandardMessage m)
        {
            try
            {
                EventData eventData = m.Value as EventData;
                if (eventData.Token == CaptureService.FRAME_CAPTURED)
                {
                    OnNewImage(sender, m);
                }
                else if (eventData.Token == CaptureService.MAX__DIR_SIZE_EXCEEDED)
                {
                    OnMaxDirSizeExceeded(sender, m);
                }
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }   

        void OnMaxDirSizeExceeded(object sender, StandardMessage m)
        {
            try
            {
                EventData eventData = m.Value as EventData;
                if (eventData.Token == CaptureService.MAX__DIR_SIZE_EXCEEDED)
                {                    
                    SafeInvoke(() =>
                    {
                        butStopService_Click(this, EventArgs.Empty);
                        MessageBox.Show($"Capture directory size has exceeded the maximum limit ({this.nudDiskLimit.Value:G} MB). Please delete some files.",
                        "Directory Size Exceeded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    });
                }
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        void OnNewImage(object sender, StandardMessage m)
        {
            try
            {
                EventData eventData = m.Value as EventData;

                if (eventData != null && !string.IsNullOrEmpty(eventData.Data?.ToString()))
                {
                    SafeInvoke(() =>
                    {
                        fileList.Add(eventData.Data.ToString());
                        tbImage.Maximum = fileList.Count - 1;
                        if (cbAuto.Checked)
                        {
                            tbImage.Value = tbImage.Maximum;
                            model.ImageLocation = fileList[tbImage.Maximum];
                            lblImagePath.Text = fileList[tbImage.Maximum];
                            long mbs = (long)eventData.Args / CaptureService.MEGABYTE;
                            lblDirSize.Text = $"{mbs:#,0} MB";
                        };
                    });
                }
                else
                {
                    GetImages();
                }
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        private void Reset()
        {
            tbImage.Minimum = 0;
            tbImage.Maximum = 0;
            tbImage.Value = 0;
            fileList?.Clear();
        }

        private void GetImages()
        {
            try
            {
                Reset();

                fileList = Directory.GetFiles(Properties.Settings.Default.CapturePath).ToList<string>();

                if (fileList.Count > 0)
                {
                    tbImage.Minimum = 0;
                    tbImage.Maximum = fileList.Count - 1;
                    tbImage.Value = tbImage.Maximum;
                    model.ImageLocation = fileList[tbImage.Value];
                }
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (fileList.Count > 0 && tbImage.Value < fileList.Count - 1)
            {
                model.ImageLocation = fileList[tbImage.Value];
                lblImagePath.Text = fileList[tbImage.Value];
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (model.Value < tbImage.Maximum && tbImage.Value < fileList.Count - 1)
            {
                tbImage.Value++;
                model.ImageLocation = fileList[tbImage.Value];
            }
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if (model.Value > tbImage.Minimum)
            {
                tbImage.Value--;
                model.ImageLocation = fileList[tbImage.Value];
            }

        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            GetImages();
        }

        private void DeleteAllButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(Properties.Settings.Default.CapturePath))
                {
                    foreach (var file in Directory.GetFiles(Properties.Settings.Default.CapturePath))
                    {
                        try
                        {
                            File.Delete(file);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error");
                        }
                    }
                    MessageBox.Show("All Files Deleted", "Info");
                    Reset();
                    pictureBox1.ImageLocation = null;
                    lblImagePath.Text = "";
                    GetImages();
                }
                else
                {
                    throw new Exception("Capture directory does not exist");
                }
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
            }
        }

        private void tbFrequency_ValueChanged(object sender, EventArgs e)
        {
            lblFrequency.Text = $"Snapshot Frequency {tbFrequency.Value} seconds";
        }

        private void butStartService_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceRunning();
                int webcamIndex = 0;
                if (rbWebcam1.Checked) webcamIndex = 1;
                else if (rbWebcam2.Checked) webcamIndex = 2;
                SafeInvoke(() => this.Cursor = Cursors.WaitCursor);
                captureService.StartAsync(CancellationToken.None,
                                        webcamIndex,
                                        tbFrequency.Value,
                                        Properties.Settings.Default.CaptureWidth,
                                        Properties.Settings.Default.CaptureHeight,
                                        Properties.Settings.Default.CapturePath,
                                        Properties.Settings.Default.DiskLimit);
                GetImages();
                SafeInvoke(() => this.Cursor = Cursors.Default);
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                ServiceStopped();
            }
        }

        private void butStopService_Click(object sender, EventArgs e)
        {
            try
            {
                SafeInvoke(() => this.Cursor = Cursors.WaitCursor);
                captureService.StopAsync(new CancellationToken());
                ServiceStopped();
                SafeInvoke(() => this.Cursor = Cursors.Default);
            }
            catch (Exception ex)
            {
                SafeInvoke(() => MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                ServiceStopped();
            }
        }

        private void cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            {
                switch (cbResolution.SelectedIndex)
                {
                    case 0:
                        Properties.Settings.Default.CaptureWidth = 320;
                        Properties.Settings.Default.CaptureHeight = 240;
                        break;
                    case 1:
                        Properties.Settings.Default.CaptureWidth = 640;
                        Properties.Settings.Default.CaptureHeight = 480;
                        break;
                    case 2:
                        Properties.Settings.Default.CaptureWidth = 800;
                        Properties.Settings.Default.CaptureHeight = 600;
                        break;
                    case 3:
                        Properties.Settings.Default.CaptureWidth = 1024;
                        Properties.Settings.Default.CaptureHeight = 768;
                        break;
                    case 4:
                        Properties.Settings.Default.CaptureWidth = 1280;
                        Properties.Settings.Default.CaptureHeight = 720;
                        break;
                    case 5:
                        Properties.Settings.Default.CaptureWidth = 1920;
                        Properties.Settings.Default.CaptureHeight = 1080;
                        break;
                    case 6:
                        Properties.Settings.Default.CaptureWidth = 2560;
                        Properties.Settings.Default.CaptureHeight = 1440;
                        break;
                    case 7:
                        Properties.Settings.Default.CaptureWidth = 3840;
                        Properties.Settings.Default.CaptureHeight = 2160;
                        break;
                    default:
                        Properties.Settings.Default.CaptureWidth = 800;
                        Properties.Settings.Default.CaptureHeight = 600;
                        break;
                }
            }
        }

        private void tbPath_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Directory.Exists(tbPath.Text))
            {
                Properties.Settings.Default.CapturePath = tbPath.Text;
            }
            else
            {
                e.Cancel = true;
                SafeInvoke(() => MessageBox.Show("Directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error));
                
            }
        }

        private void SafeInvoke(Action action)
        {
            if (this.IsDisposed || !this.IsHandleCreated)
            {
                return;
            }

            if (this.InvokeRequired)
            {
                this.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
