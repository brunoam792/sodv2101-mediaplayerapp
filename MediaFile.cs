using System;
using System.IO;
using System.Threading.Tasks;
using MediaPlayerApp.MusicBrainz;

namespace MediaPlayerApp
{
    public class MediaFile
    {
        // Event the UI subscribes to know when metadata has been updated (asynchronously)
        public event EventHandler MetadataUpdated;

        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }

        public string Year { get; set; }
        public TimeSpan Duration { get; set; }
        public MediaType Type { get; set; }
        public System.Drawing.Image AlbumArt { get; set; }

        // Internal flag to track if online fetching has been attempted (optional)
        // private bool _metadataFetchAttempted = false; 

        public MediaFile(string filePath)
        {
            FilePath = filePath;
            // Initial Title from filename before metadata is loaded
            Title = Path.GetFileNameWithoutExtension(filePath);

            LoadLocalMetadata();

            // Start online fetch asynchronously immediately after initialization
            // Task.Run is used here to avoid blocking the main UI thread
            Task.Run(() => FetchOnlineMetadata());
        }

        private void LoadLocalMetadata()
        {
            string ext = Path.GetExtension(FilePath).ToLower();

            // Basic type determination
            Type = (ext == ".mp3" || ext == ".wav" || ext == ".flac" || ext == ".m4a") ? MediaType.Audio : MediaType.Video;

            // Default values
            Artist = "Unknown Artist";
            Album = "Unknown Album";
            Duration = TimeSpan.Zero;
            Year = "Unknown Year";
            // TODO: Implement TagLib# to read local ID3 tags here
        }

        /// <summary>
        /// Attempts to fetch rich metadata for the file from MusicBrainz.
        /// </summary>
        public async Task FetchOnlineMetadata()
        {
            // If local ID3 tags were read, they should be used here for higher query accuracy.
            // Since we rely on filename parsing:

            string query = $"recording:\"{Title}\"";
            string fileName = Path.GetFileNameWithoutExtension(FilePath);

            // Logic to parse "Artist - Title" from the filename
            if (fileName.Contains(" - "))
            {
                var parts = fileName.Split(new string[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                {
                    string artistPart = parts[0].Trim();
                    string titlePart = parts[1].Trim();
                    query = $"artist:\"{artistPart}\" AND recording:\"{titlePart}\"";
                }
            }

            var result = await MusicBrainzClient.SearchRecording(query);
            bool wasUpdated = false;

            if (result != null)
            {
                // Update properties if better data is found
                if (result.Title != Title)
                {
                    Title = result.Title;
                    wasUpdated = true;
                }
                if (result.PrimaryArtist != Artist)
                {
                    Artist = result.PrimaryArtist;
                    wasUpdated = true;
                }

                if (result.Releases?.Count > 0)
                {
                    if (Album != result.Releases[0].Title)
                    {
                        Album = result.Releases[0].Title;
                        wasUpdated = true;
                    }
                }

                if (result.LengthMilliseconds > 0)
                {
                    Duration = TimeSpan.FromMilliseconds(result.LengthMilliseconds);
                    // Duration update usually doesn't require notifying the ListBox, 
                    // but it completes the data set.
                }
            }

            // Invoke the event if any metadata was successfully retrieved and updated
            if (wasUpdated)
            {
                MetadataUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Provides a display string for UI elements (e.g., ListBox).
        /// </summary>
        public override string ToString()
        {
            // Display full format only if a proper artist name has been set
            if (Artist != null && Artist != "Unknown Artist")
            {
                return $"{Artist} - {Title}";
            }

            // Otherwise, just display the title derived from the filename
            return Title;
        }
    }
}