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
        private AxWMPLib.AxWindowsMediaPlayer axWindowsMediaPlayer;
        private Label lblMetadataTitle;
        private Label lblMetadataArtist;
        private Label lblMetadataAlbum;
        private Label lblMetadataYear;
        private Label lblMetadataDuration;
        // ----------------------------------------------


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MediaPlayerForm));
            menuStrip = new MenuStrip();
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
            axWindowsMediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            visualizationBox = new PictureBox();
            visualizationTimer = new System.Windows.Forms.Timer(components);
            lblMetadataDuration = new Label();
            lblMetadataYear = new Label();
            lblMetadataAlbum = new Label();
            lblMetadataArtist = new Label();
            lblMetadataTitle = new Label();
            ((System.ComponentModel.ISupportInitialize)seekBar).BeginInit();
            controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeSlider).BeginInit();
            videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer).BeginInit();
            ((System.ComponentModel.ISupportInitialize)visualizationBox).BeginInit();
            SuspendLayout();
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(24, 24);
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Padding = new Padding(6, 3, 0, 3);
            menuStrip.Size = new Size(1099, 24);
            menuStrip.TabIndex = 0;
            menuStrip.Text = "menuStrip1";
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
            lblPlaylist.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPlaylist.Location = new Point(12, 44);
            lblPlaylist.Name = "lblPlaylist";
            lblPlaylist.Size = new Size(100, 37);
            lblPlaylist.TabIndex = 7;
            lblPlaylist.Text = "Playlist:";
            // 
            // playlistBox
            // 
            playlistBox.FormattingEnabled = true;
            playlistBox.Location = new Point(12, 85);
            playlistBox.Margin = new Padding(3, 4, 3, 4);
            playlistBox.Name = "playlistBox";
            playlistBox.Size = new Size(318, 604);
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
            videoPanel.Controls.Add(axWindowsMediaPlayer);
            videoPanel.Controls.Add(visualizationBox);
            videoPanel.Location = new Point(336, 85);
            videoPanel.Margin = new Padding(3, 4, 3, 4);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(745, 601);
            videoPanel.TabIndex = 9;
            // 
            // axWindowsMediaPlayer
            // 
            axWindowsMediaPlayer.Dock = DockStyle.Fill;
            axWindowsMediaPlayer.Enabled = true;
            axWindowsMediaPlayer.Location = new Point(0, 0);
            axWindowsMediaPlayer.Name = "axWindowsMediaPlayer";
            axWindowsMediaPlayer.OcxState = (AxHost.State)resources.GetObject("axWindowsMediaPlayer.OcxState");
            axWindowsMediaPlayer.Size = new Size(743, 599);
            axWindowsMediaPlayer.TabIndex = 11;
            // 
            // visualizationBox
            // 
            visualizationBox.Dock = DockStyle.Fill;
            visualizationBox.Location = new Point(0, 0);
            visualizationBox.Margin = new Padding(3, 4, 3, 4);
            visualizationBox.Name = "visualizationBox";
            visualizationBox.Size = new Size(743, 599);
            visualizationBox.TabIndex = 0;
            visualizationBox.TabStop = false;
            // 
            // visualizationTimer
            // 
            visualizationTimer.Interval = 50;
            // 
            // lblMetadataDuration
            // 
            lblMetadataDuration.Font = new Font("Arial", 9F);
            lblMetadataDuration.Location = new Point(775, 693);
            lblMetadataDuration.Name = "lblMetadataDuration";
            lblMetadataDuration.Size = new Size(158, 32);
            lblMetadataDuration.TabIndex = 5;
            lblMetadataDuration.Text = "Duration:";
            // 
            // lblMetadataYear
            // 
            lblMetadataYear.Font = new Font("Arial", 9F);
            lblMetadataYear.Location = new Point(939, 693);
            lblMetadataYear.Name = "lblMetadataYear";
            lblMetadataYear.Size = new Size(141, 25);
            lblMetadataYear.TabIndex = 4;
            lblMetadataYear.Text = "Year:";
            // 
            // lblMetadataAlbum
            // 
            lblMetadataAlbum.Font = new Font("Arial", 11F);
            lblMetadataAlbum.Location = new Point(398, 690);
            lblMetadataAlbum.Name = "lblMetadataAlbum";
            lblMetadataAlbum.Size = new Size(373, 35);
            lblMetadataAlbum.TabIndex = 3;
            lblMetadataAlbum.Text = "Album:";
            // 
            // lblMetadataArtist
            // 
            lblMetadataArtist.Font = new Font("Arial", 11F);
            lblMetadataArtist.Location = new Point(15, 693);
            lblMetadataArtist.Name = "lblMetadataArtist";
            lblMetadataArtist.Size = new Size(377, 32);
            lblMetadataArtist.TabIndex = 2;
            lblMetadataArtist.Text = "Artist:";
            // 
            // lblMetadataTitle
            // 
            lblMetadataTitle.Font = new Font("Arial Black", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblMetadataTitle.Location = new Point(336, 47);
            lblMetadataTitle.Name = "lblMetadataTitle";
            lblMetadataTitle.Size = new Size(745, 35);
            lblMetadataTitle.TabIndex = 1;
            lblMetadataTitle.Text = "Title:";
            // 
            // MediaPlayerForm
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1099, 1000);
            Controls.Add(lblMetadataDuration);
            Controls.Add(lblMetadataYear);
            Controls.Add(videoPanel);
            Controls.Add(lblMetadataAlbum);
            Controls.Add(playlistBox);
            Controls.Add(lblMetadataArtist);
            Controls.Add(lblPlaylist);
            Controls.Add(lblMetadataTitle);
            Controls.Add(volumeSlider);
            Controls.Add(lblVolume);
            Controls.Add(controlPanel);
            Controls.Add(seekBar);
            Controls.Add(lblTime);
            Controls.Add(menuStrip);
            MainMenuStrip = menuStrip;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MediaPlayerForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Media Player";
            ((System.ComponentModel.ISupportInitialize)seekBar).EndInit();
            controlPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeSlider).EndInit();
            videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)axWindowsMediaPlayer).EndInit();
            ((System.ComponentModel.ISupportInitialize)visualizationBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}