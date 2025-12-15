using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using Microsoft.VisualBasic; // Required for Interaction.InputBox
using WMPLib;
using AxWMPLib; // REQUIRED for the AxWindowsMediaPlayer control

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

            // Attaches the Load event handler to ensure ActiveX control is ready
            this.Load += new EventHandler(this.MediaPlayerForm_Load);

            InitializeMediaPlayer();
            InitializeUIComponents();
        }

        private async void MediaPlayerForm_Load(object sender, EventArgs e)
        {
            // Set uiMode after the ActiveX control is fully initialized.
            try
            {
                this.axWindowsMediaPlayer.uiMode = "none";
            }
            catch (Exception ex)
            {
                this.axWindowsMediaPlayer.uiMode = "full";
                System.Diagnostics.Debug.WriteLine($"Error setting uiMode on load: {ex.Message}");
            }

            // Test MusicBrainz Client (for debugging purposes)
            await MusicBrainzClient.TestClientAsync("Nirvana", "Smells Like Teen Spirit");
            await MusicBrainzClient.TestClientAsync("Non-existent Artist", "Test Song");
        }

        private void InitializeMediaPlayer()
        {
            // Initialize the controller with the visual control reference
            playbackController = new PlaybackController(this.axWindowsMediaPlayer);
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
            // ----------------------------------------------------------------
            // 1. Menu Strip: File
            // ----------------------------------------------------------------
            var fileMenu = new ToolStripMenuItem("File");
            var addFileMenuItem = new ToolStripMenuItem("Add File...", null, btnAddFile_Click);
            var addFolderMenuItem = new ToolStripMenuItem("Add Folder...", null, btnAddFolder_Click);
            var exitMenuItem = new ToolStripMenuItem("Exit", null, (s, e) => this.Close());

            fileMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                addFileMenuItem,
                addFolderMenuItem,                
                new ToolStripSeparator(),
                exitMenuItem
            });

            // ----------------------------------------------------------------
            // 2. Menu Strip: Playlist
            // ----------------------------------------------------------------
            var playlistMenu = new ToolStripMenuItem("Playlist");
            var openPlaylistMenuItem = new ToolStripMenuItem("Open Playlist...", null, OpenPlaylist_Click);
            var savePlaylistMenuItem = new ToolStripMenuItem("Save Current Playlist...", null, SavePlaylist_Click); var removeSelectedMenuItem = new ToolStripMenuItem("Remove Selected from Playlist", null, RemoveSelectedFromPlaylist_Click);
            var removeAllMenuItem = new ToolStripMenuItem("Clear Current Playlist", null, RemoveAllFromPlaylist_Click);

            playlistMenu.DropDownItems.AddRange(new ToolStripItem[]
            {
                openPlaylistMenuItem,
                savePlaylistMenuItem,
                removeSelectedMenuItem,
                removeAllMenuItem
            });

            // ----------------------------------------------------------------
            // 3. Menu Strip: Help
            // ----------------------------------------------------------------
            var helpMenu = new ToolStripMenuItem("Help");
            var aboutMenuItem = new ToolStripMenuItem("About", null, About_Click);

            helpMenu.DropDownItems.Add(aboutMenuItem);

            // Add all main menus
            menuStrip.Items.AddRange(new ToolStripItem[] { fileMenu, playlistMenu, helpMenu });

            // Event Handlers
            btnPlay.Click += btnPlay_Click;
            btnPause.Click += btnPause_Click;
            btnStop.Click += btnStop_Click;
            btnNext.Click += btnNext_Click;
            btnPrevious.Click += btnPrevious_Click;
            volumeSlider.ValueChanged += volumeSlider_ValueChanged;
            seekBar.Scroll += seekBar_ValueChanged;
            playlistBox.DoubleClick += playlistBox_DoubleClick;

            // Adding the event for the Shuffle button
            buttonShuffle.Click += buttonShuffle_Click;

            UpdateUI();
            UpdateMetadataDisplay();
        }

        // --- MEDIA LOADING AND PLAYBACK LOGIC ---

        /// <summary>
        /// Loads, plays the media, and sets up the listener for metadata updates.
        /// </summary>
        private void LoadAndPlayMedia(MediaFile newMedia)
        {
            // 1. Unsubscribe the event from the previous file, if any
            if (playbackController.CurrentMedia != null)
            {
                playbackController.CurrentMedia.MetadataUpdated -= CurrentMedia_MetadataUpdated;
            }

            // 2. Load the new media
            playbackController.LoadMediaFile(newMedia);

            // 3. Subscribe the event for the new media (for when MusicBrainz returns)
            playbackController.CurrentMedia.MetadataUpdated += CurrentMedia_MetadataUpdated;

            // 4. Update the UI IMMEDIATELY with initial data
            UpdateMetadataDisplay();

            // 5. Start playback and visualization
            StartVisualization();
            playbackController.Play();
            UpdateUI(); // Update Play/Pause/etc status
        }

        // --- METADATA LOGIC (HANDLING ASYNCHRONOUS DATA) ---

        /// <summary>
        /// Updates the metadata labels on the UI.
        /// This method must be called by all threads, ensuring correct invocation.
        /// </summary>
        private void UpdateMetadataDisplay()
        {
            if (playbackController.CurrentMedia != null)
            {
                MediaFile current = playbackController.CurrentMedia;

                // Update labels with data (now potentially updated by MusicBrainz)
                lblMetadataTitle.Text = "Title: " + current.Title;
                lblMetadataArtist.Text = "Artist: " + current.Artist;
                lblMetadataAlbum.Text = "Album: " + current.Album;
                lblMetadataDuration.Text = "Duration: " + (current.Duration != TimeSpan.Zero ? current.Duration.ToString(@"mm\:ss") : "Unknown");
                lblMetadataYear.Text = "Year: " + (current.Year != 0 ? current.Year.ToString() : "Unknown");

                // Update the playlist item with the new data (which calls ToString())
                // Find the index of the current item and update it, forcing a redraw
                int index = playlistBox.Items.IndexOf(current);
                if (index != -1)
                {
                    playlistBox.Items[index] = current; // Forces the ListBox to use the new ToString()
                    playlistBox.Refresh();
                }
            }
        }

        // Handler for asynchronous metadata updates (MusicBrainz)
        private void CurrentMedia_MetadataUpdated(object sender, EventArgs e)
        {
            // The UI can only be updated on the main thread. MusicBrainzClient runs in the background.
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate { UpdateMetadataDisplay(); }));
            }
            else
            {
                UpdateMetadataDisplay();
            }
        }

        // --- OTHER EVENT HANDLERS ---

        private void PlaybackController_MediaEnded(object sender, EventArgs e)
        {
            // When a track ends, automatically go to the next track
            btnNext_Click(sender, e);
        }

        private void playlistBox_DoubleClick(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex >= 0)
            {
                playlistManager.CurrentPlaylist.CurrentIndex = playlistBox.SelectedIndex;
                var media = playlistManager.CurrentPlaylist.GetCurrentMedia();
                if (media != null)
                {
                    LoadAndPlayMedia(media); // Use the new LoadAndPlayMedia method
                    UpdatePlaylistSelection();
                }
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia == null)
            {
                var currentMedia = playlistManager.CurrentPlaylist.GetCurrentMedia();
                if (currentMedia != null)
                {
                    LoadAndPlayMedia(currentMedia);
                    UpdatePlaylistSelection();
                    return; // Return because LoadAndPlayMedia already called Play
                }
            }
            // If media is already loaded, just continue
            playbackController.Play();
            StartVisualization();
            UpdateUI();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            var nextMedia = playlistManager.CurrentPlaylist.GetNextMedia();
            if (nextMedia != null)
            {
                LoadAndPlayMedia(nextMedia); // Use the new LoadAndPlayMedia method
                UpdatePlaylistSelection();
            }
            else
            {
                playbackController.Stop(); // Stop if there are no more songs
            }
        }

        private void UpdatePlaylistDisplay()
        {
            playlistBox.Items.Clear();

            // The ListBox uses the ToString() method of the MediaFile object for display.
            foreach (var mediaFile in mediaLibrary.AllMedia)
            {
                playlistBox.Items.Add(mediaFile);
            }
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            var prevMedia = playlistManager.CurrentPlaylist.GetPreviousMedia();
            if (prevMedia != null)
            {
                LoadAndPlayMedia(prevMedia); // Use the new LoadAndPlayMedia method
                UpdatePlaylistSelection();
            }
            else
            {
                // If no previous, restart the current track or stop
                playbackController.Seek(TimeSpan.Zero);
            }
        }

        // --- Menu Strip Methods ---

        private void OpenPlaylist_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Playlist Files|*.mpl;*.m3u|All Files|*.*";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // 1. Load file paths from the playlist file
                    List<string> filePaths = playlistManager.LoadPlaylist(ofd.FileName);

                    // 2. If files were found, clear the current library and load the new ones.
                    if (filePaths.Count > 0)
                    {
                        // Clear all files from the current library
                        playlistManager.CurrentPlaylist.MediaFiles.Clear();
                        mediaLibrary.AllMedia.Clear();

                        // Add the new playlist files to the MediaLibrary and current playlist
                        foreach (string filePath in filePaths)
                        {
                            // AddMediaFile validates the file and creates the MediaFile object
                            var mediaFile = new MediaFile(filePath);
                            mediaLibrary.AllMedia.Add(mediaFile);
                            playlistManager.CurrentPlaylist.AddMedia(mediaFile);
                        }

                        // 3. Update the user interface
                        UpdatePlaylist(); // Use UpdatePlaylist which uses CurrentPlaylist

                        // Optional: Select and start playback of the first item
                        if (playlistBox.Items.Count > 0)
                        {
                            playlistManager.CurrentPlaylist.CurrentIndex = 0;
                            playlistBox.SelectedIndex = 0;
                            PlaySelectedMedia();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Could not load the playlist or the playlist is empty/invalid.",
                                        "Playlist Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        // Method to play the currently selected item in the playlistBox
        private void PlaySelectedMedia()
        {
            // 1. Check if anything is selected
            if (playlistBox.SelectedItem == null)
            {
                return;
            }

            // 2. Try to convert the selected item (which is a MediaFile object)
            if (playlistBox.SelectedItem is MediaFile selectedFile)
            {
                // 3. Load and Play the media
                LoadAndPlayMedia(selectedFile); // Use the unified loading method

                // 4. Update the current playlist index to match the selection
                playlistManager.CurrentPlaylist.CurrentIndex = playlistBox.SelectedIndex;
                UpdatePlaylistSelection();
            }
        }

        private void SavePlaylist_Click(object sender, EventArgs e)
        {
            if (playlistManager.CurrentPlaylist.MediaFiles.Count == 0)
            {
                MessageBox.Show("The current playlist is empty. Nothing to save.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = "Media Player Playlist|*.mpl";
                sfd.FileName = playlistManager.CurrentPlaylist.Name;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(sfd.FileName))
                        {
                            writer.WriteLine($"# {playlistManager.CurrentPlaylist.Name}");
                            foreach (var media in playlistManager.CurrentPlaylist.MediaFiles)
                            {
                                writer.WriteLine(media.FilePath);
                            }
                        }
                        MessageBox.Show($"Playlist '{playlistManager.CurrentPlaylist.Name}' saved successfully to:\n{sfd.FileName}", "Save Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving playlist: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void RemoveSelectedFromPlaylist_Click(object sender, EventArgs e)
        {
            if (playlistBox.SelectedIndex >= 0)
            {
                int index = playlistBox.SelectedIndex;
                // Get the MediaFile object directly from the ListBox items
                if (playlistBox.Items[index] is MediaFile mediaToRemove)
                {
                    playlistManager.CurrentPlaylist.RemoveMedia(mediaToRemove);

                    // Also remove from general library if desired, but for playlist management, this is sufficient.
                    // mediaLibrary.RemoveMediaFile(mediaToRemove); 
                }

                UpdatePlaylist();

                if (playlistBox.Items.Count > 0)
                {
                    playlistBox.SelectedIndex = Math.Min(index, playlistBox.Items.Count - 1);
                    playlistManager.CurrentPlaylist.CurrentIndex = playlistBox.SelectedIndex;
                }
            }
            else
            {
                MessageBox.Show("Please select a track to remove.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RemoveAllFromPlaylist_Click(object sender, EventArgs e)
        {
            if (playlistManager.CurrentPlaylist.MediaFiles.Count > 0)
            {
                if (MessageBox.Show("Are you sure you want to clear the entire current playlist?",
                                     "Confirm Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    playlistManager.CurrentPlaylist.MediaFiles.Clear();
                    playbackController.Stop();
                    UpdatePlaylist();
                    UpdateUI();
                    UpdateMetadataDisplay();
                }
            }
            else
            {
                MessageBox.Show("The playlist is already empty.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void About_Click(object sender, EventArgs e)
        {
            MessageBox.Show("MediaPlayerApp\n\nVersion 1.0\nDeveloped by the Bruno Martins.\n\nSODV2201 - Final Project.",
                            "About MediaPlayer", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // --- Other Control Handlers ---

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
            UpdateMetadataDisplay();
        }

        private void buttonShuffle_Click(object sender, EventArgs e)
        {
            playlistManager.CurrentPlaylist.Shuffle = !playlistManager.CurrentPlaylist.Shuffle;

            if (playlistManager.CurrentPlaylist.Shuffle)
            {
                buttonShuffle.Text = "Shuffle 🔀 (ON)";
                // Shuffle the list immediately for a better user experience
                playlistManager.CurrentPlaylist.ShufflePlaylist();
                UpdatePlaylist();
            }
            else
            {
                buttonShuffle.Text = "Shuffle ⇄ (OFF)";
                // Note: To return to the original order, you'd need to store the unshuffled list.
                // For simplicity, we just stop shuffling the 'GetNextMedia' logic.
            }
        }

        private void volumeSlider_ValueChanged(object sender, EventArgs e)
        {
            playbackController.SetVolume(volumeSlider.Value);
        }

        private void seekBar_ValueChanged(object sender, EventArgs e)
        {
            // Only seek when the user moves the scroll bar, not when the timer updates it
            if (playbackController.CurrentMedia != null && seekBar.Capture)
            {
                var duration = playbackController.GetDuration();
                // Assuming TrackBar MaxValue = 1000
                var newPosition = TimeSpan.FromSeconds(duration.TotalSeconds * seekBar.Value / 1000.0);
                playbackController.Seek(newPosition);
            }
        }

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                // Filter should ideally come from FileManager.GetSupportedFormats()
                ofd.Filter = "Media Files|*.mp3;*.wav;*.flac;*.mp4;*.avi;*.mov|All Files|*.*";
                ofd.Multiselect = true;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    foreach (string file in ofd.FileNames)
                    {
                        // Prevent adding duplicates to the current playlist
                        if (!playlistManager.CurrentPlaylist.MediaFiles.Any(m => m.FilePath == file))
                        {
                            var mediaFile = new MediaFile(file);
                            playlistManager.CurrentPlaylist.AddMedia(mediaFile);
                        }
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
                        // Basic file extension check and duplicate prevention
                        if (Path.GetExtension(file).ToLower() != ".exe" && !playlistManager.CurrentPlaylist.MediaFiles.Any(m => m.FilePath == file))
                        {
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
            string playlistName = Interaction.InputBox("Enter playlist name:", "New Playlist", "New Playlist");
            if (!string.IsNullOrWhiteSpace(playlistName))
            {
                playlistManager.CreatePlaylist(playlistName);
                // Optionally switch to the new playlist here: playlistManager.SetCurrentPlaylist(new playlist);
                MessageBox.Show($"Playlist '{playlistName}' created!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    // Assuming the TrackBar has MaxValue = 1000
                    int max = 1000;
                    int val = (int)((current.TotalSeconds / duration.TotalSeconds) * max);

                    if (val <= max && val >= 0)
                    {
                        // Only update the seek bar if the user is not actively scrolling it
                        if (!seekBar.Capture)
                        {
                            seekBar.Value = val;
                        }
                    }
                }

                // Update the duration the first time the real duration is known
                if (playbackController.CurrentMedia.Duration == TimeSpan.Zero && duration.TotalSeconds > 0)
                {
                    playbackController.CurrentMedia.Duration = duration;
                    // No need to call UpdateMetadataDisplay here, as duration is not displayed in the metadata panel.
                }
            }
        }

        private void UpdateUI()
        {
            // Update playback status labels and buttons
            if (playbackController.CurrentMedia != null)
            {
                lblMetadataTitle.Text = playbackController.IsPlaying ?
                                       $"Now Playing: {playbackController.CurrentMedia.Title}" :
                                       $"Paused: {playbackController.CurrentMedia.Title}";

                btnPlay.Enabled = !playbackController.IsPlaying;
                btnPause.Enabled = playbackController.IsPlaying;
                btnStop.Enabled = true;
            }
            else
            {
                lblMetadataTitle.Text = "No media loaded";
                btnPlay.Enabled = playlistManager.CurrentPlaylist.MediaFiles.Count > 0;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
            }
        }

        private void UpdatePlaylist()
        {
            playlistBox.Items.Clear();
            foreach (var media in playlistManager.CurrentPlaylist.MediaFiles)
            {
                // The ListBox directly uses the MediaFile object, relying on its customized ToString()
                playlistBox.Items.Add(media);
            }
            UpdatePlaylistSelection();
        }

        private void UpdatePlaylistSelection()
        {
            if (playlistManager.CurrentPlaylist.MediaFiles.Count > 0 && playlistManager.CurrentPlaylist.CurrentIndex >= 0)
            {
                playlistBox.SelectedIndex = playlistManager.CurrentPlaylist.CurrentIndex;
            }
            else
            {
                playlistBox.SelectedIndex = -1;
            }
        }

        // --- Visualization Logic ---

        private void StartVisualization()
        {
            if (playbackController.CurrentMedia != null)
            {
                if (playbackController.CurrentMedia.Type == MediaType.Video)
                {
                    // VIDEO MODE
                    visualizationTimer.Stop();
                    visualizationBox.Visible = false;
                    axWindowsMediaPlayer.Visible = true;
                    axWindowsMediaPlayer.BringToFront();
                }
                else
                {
                    // AUDIO MODE
                    axWindowsMediaPlayer.Visible = false;
                    visualizationBox.Visible = true;
                    visualizationBox.BringToFront();
                    visualizationTimer.Start();
                }
            }
        }

        private void StopVisualization()
        {
            visualizationTimer.Stop();
            // In video mode, the WMP control remains visible when paused.
        }

        private void ClearVisualization()
        {
            visualizationTimer.Stop();
            visualizationBox.Image?.Dispose();
            visualizationBox.Image = null;

            // Hide the video player and show the audio visualization placeholder
            axWindowsMediaPlayer.Visible = false;
            visualizationBox.Visible = true;
            visualizationBox.BringToFront();
        }

        private void VisualizationTimer_Tick(object sender, EventArgs e)
        {
            if (playbackController.CurrentMedia != null &&
                playbackController.CurrentMedia.Type == MediaType.Audio &&
                playbackController.IsPlaying)
            {
                DrawPsychedelicVisualization();
            }
            else
            {
                // Stop the timer if the condition is no longer true
                visualizationTimer.Stop();
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

                int centerX = width / 2;
                int centerY = height / 2;

                // Drawing 8 orbiting ellipses
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

                // Drawing 5 pulsating concentric rings
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

                // Drawing a complex spiral/star
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

            // Increment visual parameters for animation
            visualAngle += 3;
            visualColorShift = (visualColorShift + 2) % 360;
            if (visualAngle >= 360) visualAngle -= 360;
        }

        /// <summary>
        /// Converts HSV color space values to a System.Drawing.Color object.
        /// </summary>
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

        /// <summary>
        /// Creates a simple placeholder image for album art when none is found.
        /// (Not currently used in this version but useful for future art implementation)
        /// </summary>
        private Image CreateAlbumArtPlaceholder(Size size, string title)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(30, 30, 30));
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                using (Font font = new Font("Segoe UI", 12, FontStyle.Bold))
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    string text = "NO COVER ART\n\n" + title;
                    SizeF textSize = g.MeasureString(text, font);
                    g.DrawString(text, font, brush,
                        (size.Width - textSize.Width) / 2,
                        (size.Height - textSize.Height) / 2);
                }
            }
            return bmp;
        }
    }
}