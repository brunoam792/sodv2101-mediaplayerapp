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
        private Button buttonShuffle;


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
            menuStrip.Padding = new Padding(6, 3, 0, 3);
            menuStrip.Size = new Size(1125, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
            // 
            // lblCurrentMedia
            // 
            lblCurrentMedia.Font = new Font("Arial", 12F, FontStyle.Bold);
            lblCurrentMedia.Location = new Point(12, 716);
            lblCurrentMedia.Name = "lblCurrentMedia";
            lblCurrentMedia.Size = new Size(500, 40);
            lblCurrentMedia.TabIndex = 1;
            lblCurrentMedia.Text = "No media loaded";
            // 
            // lblTime
            // 
            lblTime.Location = new Point(12, 762);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(200, 26);
            lblTime.TabIndex = 2;
            lblTime.Text = "00:00 / 00:00";
            // 
            // seekBar
            // 
            seekBar.Location = new Point(12, 796);
            seekBar.Margin = new Padding(3, 4, 3, 4);
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
            controlPanel.Location = new Point(12, 882);
            controlPanel.Margin = new Padding(3, 4, 3, 4);
            controlPanel.Name = "controlPanel";
            controlPanel.Size = new Size(748, 79);
            controlPanel.TabIndex = 4;
            // 
            // buttonShuffle
            // 
            buttonShuffle.Location = new Point(606, 12);
            buttonShuffle.Margin = new Padding(3, 4, 3, 4);
            buttonShuffle.Name = "buttonShuffle";
            buttonShuffle.Size = new Size(127, 53);
            buttonShuffle.TabIndex = 5;
            buttonShuffle.Text = "Shuffle ⇄";
            buttonShuffle.UseVisualStyleBackColor = true;
            buttonShuffle.Click += buttonShuffle_Click;
            // 
            // btnPrevious
            // 
            btnPrevious.Location = new Point(3, 13);
            btnPrevious.Margin = new Padding(3, 4, 3, 4);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(127, 53);
            btnPrevious.TabIndex = 0;
            btnPrevious.Text = "⏮ Previous";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(140, 13);
            btnPlay.Margin = new Padding(3, 4, 3, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(80, 53);
            btnPlay.TabIndex = 1;
            btnPlay.Text = "▶ Play";
            btnPlay.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            btnPause.Location = new Point(230, 13);
            btnPause.Margin = new Padding(3, 4, 3, 4);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(122, 53);
            btnPause.TabIndex = 2;
            btnPause.Text = "⏸ Pause";
            btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            btnStop.Location = new Point(358, 12);
            btnStop.Margin = new Padding(3, 4, 3, 4);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(109, 53);
            btnStop.TabIndex = 3;
            btnStop.Text = "⏹ Stop";
            btnStop.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Location = new Point(473, 12);
            btnNext.Margin = new Padding(3, 4, 3, 4);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(127, 53);
            btnNext.TabIndex = 4;
            btnNext.Text = "Next ⏭";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // lblVolume
            // 
            lblVolume.Location = new Point(775, 907);
            lblVolume.Name = "lblVolume";
            lblVolume.Size = new Size(100, 26);
            lblVolume.TabIndex = 5;
            lblVolume.Text = "Volume:";
            // 
            // volumeSlider
            // 
            volumeSlider.Location = new Point(881, 907);
            volumeSlider.Margin = new Padding(3, 4, 3, 4);
            volumeSlider.Maximum = 100;
            volumeSlider.Name = "volumeSlider";
            volumeSlider.Size = new Size(200, 69);
            volumeSlider.TabIndex = 6;
            volumeSlider.TickStyle = TickStyle.None;
            volumeSlider.Value = 50;
            // 
            // lblPlaylist
            // 
            lblPlaylist.Location = new Point(12, 53);
            lblPlaylist.Name = "lblPlaylist";
            lblPlaylist.Size = new Size(100, 535);
            lblPlaylist.TabIndex = 7;
            lblPlaylist.Text = "Playlist:";
            // 
            // playlistBox
            // 
            playlistBox.FormattingEnabled = true;
            playlistBox.Location = new Point(12, 85);
            playlistBox.Margin = new Padding(3, 4, 3, 4);
            playlistBox.Name = "playlistBox";
            playlistBox.Size = new Size(154, 604);
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
            videoPanel.Location = new Point(182, 85);
            videoPanel.Margin = new Padding(3, 4, 3, 4);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(899, 601);
            videoPanel.TabIndex = 9;
            // 
            // visualizationBox
            // 
            visualizationBox.Dock = DockStyle.Fill;
            visualizationBox.Location = new Point(0, 0);
            visualizationBox.Margin = new Padding(3, 4, 3, 4);
            visualizationBox.Name = "visualizationBox";
            visualizationBox.Size = new Size(897, 599);
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
            ClientSize = new Size(1125, 1000);
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
            Margin = new Padding(3, 4, 3, 4);
            Name = "MediaPlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Media Player";
            Load += MediaPlayerForm_Load;
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeSlider).EndInit();
            videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)visualizationBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

    }
}