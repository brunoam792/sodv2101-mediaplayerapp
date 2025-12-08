namespace MediaPlayerApp
{
    partial class MediaPlayerForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;
        private Button btnNext;
        private Button btnPrevious;
        private TrackBar volumeSlider;
        private TrackBar seekBar;
        private ListBox playlistBox;
        private Label lblCurrentMedia;
        private Label lblTime;
        private System.Windows.Forms.Timer updateTimer;
        private MenuStrip menuStrip;
        private Panel controlPanel;
        private Label lblVolume;
        private Label lblPlaylist;
        private Panel videoPanel;
        private PictureBox visualizationBox;
        private System.Windows.Forms.Timer visualizationTimer;

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
            components = new System.ComponentModel.Container();
            menuStrip = new MenuStrip();
            lblCurrentMedia = new Label();
            lblTime = new Label();
            seekBar = new TrackBar();
            controlPanel = new Panel();
            buttonShuffle = new Button();
            btnPrevious = new Button();
            btnPlay = new Button();
            btnPause = new Button();
            btnStop = new Button();
            btnNext = new Button();
            lblVolume = new Label();
            volumeSlider = new TrackBar();
            lblPlaylist = new Label();
            playlistBox = new ListBox();
            updateTimer = new System.Windows.Forms.Timer(components);
            videoPanel = new Panel();
            visualizationBox = new PictureBox();
            visualizationTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)seekBar).BeginInit();
            controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeSlider).BeginInit();
            videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)visualizationBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(1130, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // lblCurrentMedia
            // 
            lblCurrentMedia.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblCurrentMedia.Location = new Point(12, 541);
            lblCurrentMedia.Name = "lblCurrentMedia";
            lblCurrentMedia.Size = new Size(500, 30);
            lblCurrentMedia.TabIndex = 1;
            lblCurrentMedia.Text = "No media loaded";
            // 
            // lblTime
            // 
            lblTime.Location = new Point(12, 576);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(200, 20);
            lblTime.TabIndex = 2;
            lblTime.Text = "00:00 / 00:00";
            // 
            // seekBar
            // 
            seekBar.Location = new Point(12, 601);
            seekBar.Maximum = 10000;
            seekBar.Name = "seekBar";
            seekBar.Size = new Size(1068, 69);
            seekBar.TabIndex = 3;
            seekBar.TickStyle = TickStyle.None;
            // 
            // controlPanel
            // 
            controlPanel.Controls.Add(buttonShuffle);
            controlPanel.Controls.Add(btnPrevious);
            controlPanel.Controls.Add(btnPlay);
            controlPanel.Controls.Add(btnPause);
            controlPanel.Controls.Add(btnStop);
            controlPanel.Controls.Add(btnNext);
            controlPanel.Location = new Point(12, 667);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(748, 60);
            controlPanel.TabIndex = 4;
            // 
            // buttonShuffle
            // 
            buttonShuffle.Location = new Point(606, 9);
            buttonShuffle.Name = "buttonShuffle";
            buttonShuffle.Size = new Size(127, 40);
            buttonShuffle.TabIndex = 5;
            buttonShuffle.Text = "Shuffle ⇄";
            buttonShuffle.UseVisualStyleBackColor = true;
            buttonShuffle.Click += buttonShuffle_Click;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(3, 10);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(127, 40);
            btnPrevious.TabIndex = 0;
            btnPrevious.Text = "⏮ Previous";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(140, 10);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(80, 40);
            btnPlay.TabIndex = 1;
            btnPlay.Text = "▶ Play";
            btnPlay.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(230, 10);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(122, 40);
            btnPause.TabIndex = 2;
            btnPause.Text = "⏸ Pause";
            btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(358, 9);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(109, 40);
            btnStop.TabIndex = 3;
            btnStop.Text = "⏹ Stop";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(473, 9);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(127, 40);
            btnNext.TabIndex = 4;
            btnNext.Text = "Next ⏭";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // lblVolume
            // 
            lblVolume.Location = new Point(775, 686);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new Size(100, 20);
            lblVolume.TabIndex = 5;
            lblVolume.Text = "Volume:";
            // 
            // volumeSlider
            // 
            volumeSlider.Location = new Point(881, 686);
            volumeSlider.Maximum = 100;
            volumeSlider.Name = "volumeSlider";
            volumeSlider.Size = new Size(200, 69);
            volumeSlider.TabIndex = 6;
            volumeSlider.TickStyle = TickStyle.None;
            volumeSlider.Value = 50;
            // 
            // lblPlaylist
            // 
            lblPlaylist.Location = new Point(12, 40);
            lblPlaylist.Name = "lblPlaylist";
            lblPlaylist.Size = new Size(100, 404);
            lblPlaylist.TabIndex = 7;
            lblPlaylist.Text = "Playlist:";
            // 
            // playlistBox
            // 
            playlistBox.FormattingEnabled = true;
            playlistBox.Location = new Point(12, 65);
            playlistBox.Name = "playlistBox";
            playlistBox.Size = new Size(154, 454);
            playlistBox.TabIndex = 8;
            // 
            // updateTimer
            // 
            updateTimer.Enabled = true;
            // 
            // videoPanel
            // 
            videoPanel.BackColor = Color.Black;
            videoPanel.BorderStyle = BorderStyle.FixedSingle;
            videoPanel.Controls.Add(visualizationBox);
            videoPanel.Location = new Point(182, 65);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(899, 454);
            videoPanel.TabIndex = 9;
            // 
            // visualizationBox
            // 
            visualizationBox.Dock = DockStyle.Fill;
            visualizationBox.Location = new Point(0, 0);
            visualizationBox.Name = "visualizationBox";
            visualizationBox.Size = new Size(897, 452);
            visualizationBox.SizeMode = PictureBoxSizeMode.Zoom;
            visualizationBox.TabIndex = 0;
            visualizationBox.TabStop = false;
            // 
            // visualizationTimer
            // 
            visualizationTimer.Interval = 50;
            // 
            // MediaPlayerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1130, 776);
            Controls.Add(videoPanel);
            Controls.Add(playlistBox);
            Controls.Add(lblPlaylist);
            Controls.Add(volumeSlider);
            Controls.Add(lblVolume);
            Controls.Add(controlPanel);
            Controls.Add(seekBar);
            Controls.Add(lblTime);
            Controls.Add(lblCurrentMedia);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Name = "MediaPlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Media Player";
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeSlider).EndInit();
            videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)visualizationBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button buttonShuffle;
    }
}