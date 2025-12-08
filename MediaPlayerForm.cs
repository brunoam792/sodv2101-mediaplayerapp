using System;
using System.Drawing;
using System.Windows.Forms;

namespace MediaPlayerApp
{
    public partial class MediaPlayerForm : Form
    {
        private PlaybackController playbackController;
        private MediaLibrary mediaLibrary;
        private PlaylistManager playlistManager;
        private Random visualRandom;
        private float visualAngle;
        private int visualColorShift;

        public MediaPlayerForm()
        {
            InitializeComponent();
            InitializeMediaPlayer();
            InitializeUIComponents();
        }

        private void InitializeMediaPlayer()
        {
            playbackController = new PlaybackController();
            mediaLibrary = new MediaLibrary();
            playlistManager = new PlaylistManager();
            visualRandom = new Random();
            visualAngle = 0;
            visualColorShift = 0;

            playbackController.MediaEnded += PlaybackController_MediaEnded;

            updateTimer.Interval = 500;
            updateTimer.Tick += UpdateTimer_Tick;
            updateTimer.Start();

            visualizationTimer.Interval = 50;
            visualizationTimer.Tick += VisualizationTimer_Tick;
        }

        private void InitializeUIComponents()
        {
            // Menu Strip
            var fileMenu = new ToolStripMenuItem("File");
            var addFileMenuItem = new ToolStripMenuItem("Add File", null, btnAddFile_Click);
            var addFolderMenuItem = new ToolStripMenuItem("Add Folder", null, btnAddFolder_Click);
            var exitMenuItem = new ToolStripMenuItem("Exit", null, (s, e) => this.Close());
            fileMenu.DropDownItems.AddRange(new ToolStripItem[] { addFileMenuItem, addFolderMenuItem, new ToolStripSeparator(), exitMenuItem });

            var playlistMenu = new ToolStripMenuItem("Playlist");
            var newPlaylistMenuItem = new ToolStripMenuItem("New Playlist", null, btnNewPlaylist_Click);
            playlistMenu.DropDownItems.Add(newPlaylistMenuItem);

            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, playlistMenu });

            // Event Handlers
            btnPlay.Click += btnPlay_Click;
            btnPause.Click += btnPause_Click;
            btnStop.Click += btnStop_Click;
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            volumeSlider.ValueChanged += volumeSlider_ValueChanged;
            seekBar.Scroll += seekBar_ValueChanged;
            playlistBox.DoubleClick += playlistBox_DoubleClick;

            UpdateUI();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia == null)
            {
                var currentMedia = playlistManager.CurrentPlaylist.GetCurrentMedia();
                if (currentMedia != null)
                {
                    playbackController.LoadMediaFile(currentMedia);
                }
            }
            playbackController.Play();
            StartVisualization();
            UpdateUI();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            playbackController.Pause();
            StopVisualization();
            UpdateUI();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            playbackController.Stop();
            StopVisualization();
            ClearVisualization();
            UpdateUI();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var nextMedia = playlistManager.CurrentPlaylist.GetNextMedia();
            if (nextMedia != null)
            {
                playbackController.LoadMediaFile(nextMedia);
                playbackController.Play();
                UpdatePlaylistSelection();
                UpdateUI();
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var prevMedia = playlistManager.CurrentPlaylist.GetPreviousMedia();
            if (prevMedia != null)
            {
                playbackController.LoadMediaFile(prevMedia);
                playbackController.Play();
                UpdatePlaylistSelection();
                UpdateUI();
            }
        }

        private void volumeSlider_ValueChanged(object sender, EventArgs e)
        {
            playbackController.SetVolume(volumeSlider.Value);
        }

        private void seekBar_ValueChanged(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia != null)
            {
                var duration = playbackController.GetDuration();
                var newPosition = TimeSpan.FromSeconds(duration.TotalSeconds * seekBar.Value / 1000.0);
                playbackController.Seek(newPosition);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = mediaLibrary.FileManager.GetSupportedFormats();
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        mediaLibrary.AddMediaFile(file);
                        var mediaFile = new MediaFile(file);
                        playlistManager.CurrentPlaylist.AddMedia(mediaFile);
                    }
                    UpdatePlaylist();
                }
            }
        }

        private void btnAddFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    var files = System.IO.Directory.GetFiles(fbd.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories);
                    foreach (string file in files)
                    {
                        if (mediaLibrary.FileManager.ValidateFile(file))
                        {
                            mediaLibrary.AddMediaFile(file);
                            var mediaFile = new MediaFile(file);
                            playlistManager.CurrentPlaylist.AddMedia(mediaFile);
                        }
                    }
                    UpdatePlaylist();
                }
            }
        }

        private void btnNewPlaylist_Click(object sender, EventArgs e)
        {
            string playlistName = Microsoft.VisualBasic.Interaction.InputBox("Enter playlist name:", "New Playlist", "New Playlist");
            if (!string.IsNullOrWhiteSpace(playlistName))
            {
                playlistManager.CreatePlaylist(playlistName);
                MessageBox.Show($"Playlist '{playlistName}' created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void playlistBox_DoubleClick(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex >= 0)
            {
                playlistManager.CurrentPlaylist.CurrentIndex = playlistBox.SelectedIndex;
                var media = playlistManager.CurrentPlaylist.GetCurrentMedia();
                if (media != null)
                {
                    playbackController.LoadMediaFile(media);
                    playbackController.Play();
                    UpdateUI();
                }
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia != null && playbackController.IsPlaying)
            {
                var current = playbackController.GetCurrentPosition();
                var duration = playbackController.GetDuration();

                lblTime.Text = $"{current:mm\\:ss} / {duration:mm\\:ss}";

                if (duration.TotalSeconds > 0)
                {
                    seekBar.Value = (int)((current.TotalSeconds / duration.TotalSeconds) * 1000);
                }
            }
        }

        private void PlaybackController_MediaEnded(object sender, EventArgs e)
        {
            btnNext_Click(sender, e);
        }

        private void UpdateUI()
        {
            if (playbackController.CurrentMedia != null)
            {
                lblCurrentMedia.Text = $"Now Playing: {playbackController.CurrentMedia.Title}";
            }
            else
            {
                lblCurrentMedia.Text = "No media loaded";
            }
        }

        private void UpdatePlaylist()
        {
            playlistBox.Items.Clear();
            foreach (var media in playlistManager.CurrentPlaylist.MediaFiles)
            {
                playlistBox.Items.Add(media.Title);
            }
        }

        private void UpdatePlaylistSelection()
        {
            playlistBox.SelectedIndex = playlistManager.CurrentPlaylist.CurrentIndex;
        }

        private void StartVisualization()
        {
            if (playbackController.CurrentMedia != null)
            {
                if (playbackController.CurrentMedia.Type == MediaType.Video)
                {
                    // For video, show video placeholder
                    videoPanel.Controls.Clear();
                    videoPanel.Controls.Add(visualizationBox);

                    // Create a simple video indicator
                    Bitmap videoBmp = new Bitmap(videoPanel.Width, videoPanel.Height);
                    using (Graphics g = Graphics.FromImage(videoBmp))
                    {
                        g.Clear(Color.Black);
                        using (Font font = new Font("Arial", 20, FontStyle.Bold))
                        using (SolidBrush brush = new SolidBrush(Color.White))
                        {
                            string text = "🎬 VIDEO PLAYING\n\n" + playbackController.CurrentMedia.Title;
                            SizeF textSize = g.MeasureString(text, font);
                            g.DrawString(text, font, brush,
                                (videoPanel.Width - textSize.Width) / 2,
                                (videoPanel.Height - textSize.Height) / 2);
                        }
                    }
                    visualizationBox.Image = videoBmp;
                    visualizationTimer.Stop();
                }
                else
                {
                    // For audio, show psychedelic visualization
                    videoPanel.Controls.Clear();
                    videoPanel.Controls.Add(visualizationBox);
                    visualizationTimer.Start();
                }
            }
        }

        private void StopVisualization()
        {
            visualizationTimer.Stop();
        }

        private void ClearVisualization()
        {
            visualizationBox.Image = null;
            videoPanel.Controls.Clear();
            videoPanel.Controls.Add(visualizationBox);
        }

        private void VisualizationTimer_Tick(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia != null &&
                playbackController.CurrentMedia.Type == MediaType.Audio &&
                playbackController.IsPlaying)
            {
                DrawPsychedelicVisualization();
            }
        }

        private void DrawPsychedelicVisualization()
        {
            int width = visualizationBox.Width;
            int height = visualizationBox.Height;

            if (width <= 0 || height <= 0) return;

            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Black);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Draw multiple rotating circles with gradient colors
                int centerX = width / 2;
                int centerY = height / 2;

                for (int i = 0; i < 8; i++)
                {
                    float angle = visualAngle + (i * 45);
                    float radius = 80 + (i * 20);

                    int x = (int)(centerX + Math.Cos(angle * Math.PI / 180) * radius);
                    int y = (int)(centerY + Math.Sin(angle * Math.PI / 180) * radius);

                    int colorValue = (visualColorShift + i * 30) % 360;
                    Color color = ColorFromHSV(colorValue, 0.8, 0.9);

                    using (SolidBrush brush = new SolidBrush(Color.FromArgb(150, color)))
                    {
                        g.FillEllipse(brush, x - 40, y - 40, 80, 80);
                    }
                }

                // Draw pulsating circles
                for (int i = 0; i < 5; i++)
                {
                    int size = 50 + (int)(Math.Sin((visualAngle + i * 72) * Math.PI / 180) * 30);
                    int colorVal = (visualColorShift + i * 72) % 360;
                    Color color = ColorFromHSV(colorVal, 1.0, 1.0);

                    using (Pen pen = new Pen(Color.FromArgb(100, color), 3))
                    {
                        g.DrawEllipse(pen, centerX - size, centerY - size, size * 2, size * 2);
                    }
                }

                // Draw spiral lines
                for (int i = 0; i < 360; i += 10)
                {
                    float angle1 = i + visualAngle;
                    float angle2 = i + visualAngle + 10;
                    float r1 = 10 + (i / 2);
                    float r2 = 10 + ((i + 10) / 2);

                    int x1 = (int)(centerX + Math.Cos(angle1 * Math.PI / 180) * r1);
                    int y1 = (int)(centerY + Math.Sin(angle1 * Math.PI / 180) * r1);
                    int x2 = (int)(centerX + Math.Cos(angle2 * Math.PI / 180) * r2);
                    int y2 = (int)(centerY + Math.Sin(angle2 * Math.PI / 180) * r2);

                    int colorVal = (visualColorShift + i) % 360;
                    Color color = ColorFromHSV(colorVal, 0.9, 0.8);

                    using (Pen pen = new Pen(Color.FromArgb(180, color), 2))
                    {
                        g.DrawLine(pen, x1, y1, x2, y2);
                    }
                }
            }

            visualizationBox.Image?.Dispose();
            visualizationBox.Image = bmp;

            visualAngle += 3;
            visualColorShift = (visualColorShift + 2) % 360;
            if (visualAngle >= 360) visualAngle -= 360;
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            if (hi == 0)
                return Color.FromArgb(255, v, t, p);
            else if (hi == 1)
                return Color.FromArgb(255, q, v, p);
            else if (hi == 2)
                return Color.FromArgb(255, p, v, t);
            else if (hi == 3)
                return Color.FromArgb(255, p, q, v);
            else if (hi == 4)
                return Color.FromArgb(255, t, p, v);
            else
                return Color.FromArgb(255, v, p, q);
        }

        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            playlistManager.CurrentPlaylist.Shuffle = !playlistManager.CurrentPlaylist.Shuffle;
            buttonShuffle.BackColor = playlistManager.CurrentPlaylist.Shuffle ? Color.LightGreen : SystemColors.Control;
        }
    }
}