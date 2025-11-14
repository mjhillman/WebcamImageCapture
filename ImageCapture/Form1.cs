using HITS.LIB.WeakEvents;

namespace WebcamImageCapture
{
    public partial class Form1 : Form
    {
        private List<string> fileList;
        private ImageViewModel model = new ImageViewModel();
        private CaptureService captureService = new();
        EventMgr mgr = new EventMgr();

        public Form1()
        {
            try
            {
                InitializeComponent();
                pictureBox1.DataBindings.Add("ImageLocation", model, "ImageLocation", true, DataSourceUpdateMode.OnPropertyChanged);
                tbImage.DataBindings.Add("Value", model, "Value", true, DataSourceUpdateMode.OnPropertyChanged);
                GetImages();
                mgr.SubscribeToEvent(this, OnNewImage);
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
            ServiceStopped();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            captureService?.Dispose();
            int webcamIndex = 0;
            if (rbWebcam1.Checked) webcamIndex = 1;
            else if (rbWebcam2.Checked) webcamIndex = 2;

            Properties.Settings.Default.AutoShowLast = cbAuto.Checked;
            Properties.Settings.Default.CapturePath = tbPath.Text;
            Properties.Settings.Default.SnapshotFrequency = tbFrequency.Value;
            Properties.Settings.Default.WebcamIndex = webcamIndex;
            Properties.Settings.Default.ResolutionIndex = cbResolution.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void ServiceRunning()
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
        }

        private void ServiceStopped()
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
            tbImage.Enabled = false;
            DeleteAllButton.Enabled = true;
        }

        void OnNewImage(object sender, StandardMessage m)
        {
            try
            {
                EventData eventData = m.Value as EventData;

                if (eventData != null && !string.IsNullOrEmpty(eventData.Data?.ToString()))
                {
                    // Add to fileList safely (fileList itself is not a UI control, so this is fine)
                    fileList.Add(eventData.Data.ToString());

                    // Marshal ALL UI updates to the UI thread
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)(() =>
                        {
                            tbImage.Maximum = fileList.Count - 1;
                            if (cbAuto.Checked)
                            {
                                tbImage.Value = tbImage.Maximum;
                                model.ImageLocation = fileList[tbImage.Maximum];
                                lblImagePath.Text = fileList[tbImage.Maximum];
                            }
                        }));
                    }
                    else
                    {
                        // Already on UI thread
                        tbImage.Maximum = fileList.Count - 1;
                        if (cbAuto.Checked)
                        {
                            tbImage.Value = tbImage.Maximum;
                            model.ImageLocation = fileList[tbImage.Value];
                        }
                    }
                }
                else
                {
                    GetImages();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                this.Cursor = Cursors.WaitCursor;
                captureService.StartAsync(CancellationToken.None,
                                        webcamIndex,
                                        tbFrequency.Value,
                                        Properties.Settings.Default.CaptureWidth,
                                        Properties.Settings.Default.CaptureHeight,
                                        Properties.Settings.Default.CapturePath);
                GetImages();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServiceStopped();
            }
        }

        private void butStopService_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                captureService.StopAsync(new CancellationToken());
                ServiceStopped();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("Directory does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
