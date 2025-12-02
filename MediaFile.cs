using System;
using System.IO;

namespace MediaPlayerApp
{
    public class MediaFile
    {
        public string FilePath { get; set; }
        public string Title { get; set; }
        public string Artist { get; set; }
        public string Album { get; set; }
        public TimeSpan Duration { get; set; }
        public MediaType Type { get; set; }
        public System.Drawing.Image AlbumArt { get; set; }

        public MediaFile(string filePath)
        {
            FilePath = filePath;
            Title = Path.GetFileNameWithoutExtension(filePath);
            LoadMetadata();
        }

        public string GetFileName()
        {
            return Path.GetFileName(FilePath);
        }

        public string GetFileInfo()
        {
            return $"{Title} - {Artist}";
        }

        private void LoadMetadata()
        {
            string ext = Path.GetExtension(FilePath).ToLower();
            Type = (ext == ".mp3" || ext == ".wav" || ext == ".flac") ? MediaType.Audio : MediaType.Video;
            Artist = "Unknown Artist";
            Album = "Unknown Album";
        }
    }
}