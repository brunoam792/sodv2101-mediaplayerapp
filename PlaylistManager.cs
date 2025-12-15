using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaPlayerApp
{
    public class PlaylistManager
    {
        public List<Playlist> Playlists { get; private set; }
        public Playlist CurrentPlaylist { get; private set; }

        public PlaylistManager()
        {
            Playlists = new List<Playlist>();
            CreatePlaylist("Default");
            CurrentPlaylist = Playlists[0];
        }

        public void CreatePlaylist(string name)
        {
            Playlist playlist = new Playlist(name);
            Playlists.Add(playlist);
        }

        public void DeletePlaylist(Playlist playlist)
        {
            Playlists.Remove(playlist);
        }

        /// <summary>
        /// Reads an M3U/MPL file and extracts media file paths.
        /// </summary>
        public List<string> LoadPlaylist(string filePath)
        {
            List<string> filePaths = new List<string>();

            if (!File.Exists(filePath))
            {
                return filePaths;
            }

            try
            {
                var lines = File.ReadAllLines(filePath);
                string playlistDirectory = Path.GetDirectoryName(filePath);

                foreach (var line in lines)
                {
                    string trimmedLine = line.Trim();

                    // Ignore empty lines or comments (M3U tags)
                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    string fullPath = trimmedLine;

                    // Handle relative paths
                    if (!Path.IsPathRooted(fullPath))
                    {
                        fullPath = Path.Combine(playlistDirectory, fullPath);
                    }

                    // Add the path if the file exists
                    if (File.Exists(fullPath))
                    {
                        filePaths.Add(fullPath);
                    }
                }
            }
            catch (Exception ex)
            {
                // Error reading file
                System.Diagnostics.Debug.WriteLine($"Error reading playlist: {ex.Message}");
            }

            return filePaths;
        }

        public List<Playlist> GetAllPlaylists()
        {
            return Playlists;
        }

        public void SetCurrentPlaylist(Playlist playlist)
        {
            CurrentPlaylist = playlist;
        }
    }
}