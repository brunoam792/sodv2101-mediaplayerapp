using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MediaPlayerApp
{
    public class FileManager
    {
        public string LibraryPath { get; set; }

        public FileManager()
        {
            LibraryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic));
        }

        public List<string> BrowseFiles()
        {
            List<string> files = new List<string>();
            if (Directory.Exists(LibraryPath))
            {
                files.AddRange(Directory.GetFiles(LibraryPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => ValidateFile(f)));
            }
            return files;
        }

        public bool ValidateFile(string filePath)
        {
            string[] supportedFormats = { ".mp3", ".wav", ".flac", ".mp4", ".avi", ".mkv", ".wmv" };
            string ext = Path.GetExtension(filePath).ToLower();
            return supportedFormats.Contains(ext);
        }

        public string GetSupportedFormats()
        {
            return "Audio/Video Files|*.mp3;*.wav;*.flac;*.mp4;*.avi;*.mkv;*.wmv|All Files|*.*";
        }

        public void ImportFiles(List<string> filePaths)
        {
            // Import logic can be extended here
        }

        public void ExportPlaylist(Playlist playlist, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine($"# {playlist.Name}");
                foreach (var media in playlist.MediaFiles)
                {
                    writer.WriteLine(media.FilePath);
                }
            }
        }
    }
}