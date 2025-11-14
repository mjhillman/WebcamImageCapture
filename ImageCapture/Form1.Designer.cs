namespace WebcamImageCapture
{
    partial class Form1
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
            pictureBox1 = new PictureBox();
            splitContainer1 = new SplitContainer();
            cbResolution = new ComboBox();
            label2 = new Label();
            lblPath = new Label();
            tbPath = new TextBox();
            cbAuto = new CheckBox();
            butStopService = new Button();
            butStartService = new Button();
            rbWebcam0 = new RadioButton();
            rbWebcam2 = new RadioButton();
            rbWebcam1 = new RadioButton();
            lblFrequency = new Label();
            tbFrequency = new TrackBar();
            label1 = new Label();
            DeleteAllButton = new Button();
            PreviousButton = new Button();
            NextButton = new Button();
            tbImage = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tbFrequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)tbImage).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1267, 668);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(cbResolution);
            splitContainer1.Panel1.Controls.Add(label2);
            splitContainer1.Panel1.Controls.Add(lblPath);
            splitContainer1.Panel1.Controls.Add(tbPath);
            splitContainer1.Panel1.Controls.Add(cbAuto);
            splitContainer1.Panel1.Controls.Add(butStopService);
            splitContainer1.Panel1.Controls.Add(butStartService);
            splitContainer1.Panel1.Controls.Add(rbWebcam0);
            splitContainer1.Panel1.Controls.Add(rbWebcam2);
            splitContainer1.Panel1.Controls.Add(rbWebcam1);
            splitContainer1.Panel1.Controls.Add(lblFrequency);
            splitContainer1.Panel1.Controls.Add(tbFrequency);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(DeleteAllButton);
            splitContainer1.Panel1.Controls.Add(PreviousButton);
            splitContainer1.Panel1.Controls.Add(NextButton);
            splitContainer1.Panel1.Controls.Add(tbImage);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(pictureBox1);
            splitContainer1.Size = new Size(1267, 844);
            splitContainer1.SplitterDistance = 172;
            splitContainer1.TabIndex = 1;
            // 
            // cbResolution
            // 
            cbResolution.FormattingEnabled = true;
            cbResolution.Items.AddRange(new object[] { "", "QVGA\t320 × 240", "VGA\t640 × 480", "SVGA\t800 × 600", "XGA\t1024 × 768", "HD\t1280 × 720", "Full HD \t1920 × 1080", "", "QHD \t2560 × 1440", "4K UHD\t3840 × 2160" });
            cbResolution.Location = new Point(100, 55);
            cbResolution.Name = "cbResolution";
            cbResolution.Size = new Size(218, 25);
            cbResolution.TabIndex = 22;
            cbResolution.SelectedIndexChanged += cbResolution_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(19, 58);
            label2.Name = "label2";
            label2.Size = new Size(72, 17);
            label2.TabIndex = 21;
            label2.Text = "Resolution:";
            // 
            // lblPath
            // 
            lblPath.AutoSize = true;
            lblPath.Location = new Point(854, 58);
            lblPath.Name = "lblPath";
            lblPath.Size = new Size(86, 17);
            lblPath.TabIndex = 19;
            lblPath.Text = "Capture Path:";
            // 
            // tbPath
            // 
            tbPath.Location = new Point(946, 56);
            tbPath.Name = "tbPath";
            tbPath.Size = new Size(294, 25);
            tbPath.TabIndex = 18;
            // 
            // cbAuto
            // 
            cbAuto.AutoSize = true;
            cbAuto.Location = new Point(508, 70);
            cbAuto.Name = "cbAuto";
            cbAuto.Size = new Size(206, 21);
            cbAuto.TabIndex = 17;
            cbAuto.Text = "Automatically Show Last Image";
            cbAuto.UseVisualStyleBackColor = true;
            // 
            // butStopService
            // 
            butStopService.Location = new Point(982, 15);
            butStopService.Name = "butStopService";
            butStopService.Size = new Size(112, 25);
            butStopService.TabIndex = 16;
            butStopService.Text = "Stop Snapshots";
            butStopService.UseVisualStyleBackColor = true;
            butStopService.Click += butStopService_Click;
            // 
            // butStartService
            // 
            butStartService.Location = new Point(846, 11);
            butStartService.Name = "butStartService";
            butStartService.Size = new Size(111, 28);
            butStartService.TabIndex = 15;
            butStartService.Text = "Start Snapshots";
            butStartService.UseVisualStyleBackColor = true;
            butStartService.Click += butStartService_Click;
            // 
            // rbWebcam0
            // 
            rbWebcam0.AutoSize = true;
            rbWebcam0.Location = new Point(19, 19);
            rbWebcam0.Name = "rbWebcam0";
            rbWebcam0.Size = new Size(87, 21);
            rbWebcam0.TabIndex = 14;
            rbWebcam0.Text = "Webcam 0";
            rbWebcam0.UseVisualStyleBackColor = true;
            // 
            // rbWebcam2
            // 
            rbWebcam2.AutoSize = true;
            rbWebcam2.Location = new Point(231, 19);
            rbWebcam2.Name = "rbWebcam2";
            rbWebcam2.Size = new Size(87, 21);
            rbWebcam2.TabIndex = 13;
            rbWebcam2.Text = "Webcam 2";
            rbWebcam2.UseVisualStyleBackColor = true;
            // 
            // rbWebcam1
            // 
            rbWebcam1.AutoSize = true;
            rbWebcam1.Checked = true;
            rbWebcam1.Location = new Point(123, 19);
            rbWebcam1.Name = "rbWebcam1";
            rbWebcam1.Size = new Size(87, 21);
            rbWebcam1.TabIndex = 12;
            rbWebcam1.TabStop = true;
            rbWebcam1.Text = "Webcam 1";
            rbWebcam1.UseVisualStyleBackColor = true;
            // 
            // lblFrequency
            // 
            lblFrequency.AutoSize = true;
            lblFrequency.Location = new Point(508, 15);
            lblFrequency.Name = "lblFrequency";
            lblFrequency.Size = new Size(188, 17);
            lblFrequency.TabIndex = 7;
            lblFrequency.Text = "Snapshot Frequency 1 seconds";
            // 
            // tbFrequency
            // 
            tbFrequency.AutoSize = false;
            tbFrequency.Location = new Point(358, 36);
            tbFrequency.Maximum = 60;
            tbFrequency.Minimum = 1;
            tbFrequency.Name = "tbFrequency";
            tbFrequency.Size = new Size(469, 39);
            tbFrequency.TabIndex = 6;
            tbFrequency.Value = 3;
            tbFrequency.ValueChanged += tbFrequency_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(612, 135);
            label1.Name = "label1";
            label1.Size = new Size(73, 17);
            label1.TabIndex = 5;
            label1.Text = "Image Path";
            // 
            // DeleteAllButton
            // 
            DeleteAllButton.Location = new Point(1165, 11);
            DeleteAllButton.Name = "DeleteAllButton";
            DeleteAllButton.Size = new Size(75, 28);
            DeleteAllButton.TabIndex = 4;
            DeleteAllButton.Text = "Delete All";
            DeleteAllButton.UseVisualStyleBackColor = true;
            DeleteAllButton.Click += DeleteAllButton_Click;
            // 
            // PreviousButton
            // 
            PreviousButton.Location = new Point(19, 133);
            PreviousButton.Name = "PreviousButton";
            PreviousButton.Size = new Size(75, 28);
            PreviousButton.TabIndex = 2;
            PreviousButton.Text = "Previous";
            PreviousButton.UseVisualStyleBackColor = true;
            PreviousButton.Click += PreviousButton_Click;
            // 
            // NextButton
            // 
            NextButton.Location = new Point(1165, 133);
            NextButton.Name = "NextButton";
            NextButton.Size = new Size(75, 28);
            NextButton.TabIndex = 1;
            NextButton.Text = "Next";
            NextButton.UseVisualStyleBackColor = true;
            NextButton.Click += NextButton_Click;
            // 
            // tbImage
            // 
            tbImage.AutoSize = false;
            tbImage.BackColor = Color.WhiteSmoke;
            tbImage.Location = new Point(12, 95);
            tbImage.Name = "tbImage";
            tbImage.Size = new Size(1228, 37);
            tbImage.TabIndex = 0;
            tbImage.ValueChanged += trackBar1_ValueChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(1267, 844);
            Controls.Add(splitContainer1);
            Name = "Form1";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Webcam Image Capture";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tbFrequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)tbImage).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private SplitContainer splitContainer1;
        private Button PreviousButton;
        private Button NextButton;
        private Button DeleteAllButton;
        private Label label1;
        private TrackBar tbImage;
        private Label lblFrequency;
        private TrackBar tbFrequency;
        private RadioButton rbWebcam0;
        private RadioButton rbWebcam2;
        private RadioButton rbWebcam1;
        private Button butStopService;
        private Button butStartService;
        private CheckBox cbAuto;
        private TextBox tbPath;
        private Label lblPath;
        private ComboBox cbResolution;
        private Label label2;
    }
}
