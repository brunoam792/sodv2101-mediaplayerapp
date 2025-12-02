using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaPlayerApp
{
    public class MediaLibrary
    {
        public List<MediaFile> AllMedia { get; private set; }
        public List<MediaFile> RecentlyPlayed { get; private set; }
        public FileManager FileManager { get; private set; }

        public MediaLibrary()
        {
            AllMedia = new List<MediaFile>();
            RecentlyPlayed = new List<MediaFile>();
            FileManager = new FileManager();
        }

        public void AddMediaFile(string filePath)
        {
            if (FileManager.ValidateFile(filePath))
            {
                MediaFile mediaFile = new MediaFile(filePath);
                AllMedia.Add(mediaFile);
            }
        }

        public void RemoveMediaFile(MediaFile mediaFile)
        {
            AllMedia.Remove(mediaFile);
        }

        public List<MediaFile> SearchMedia(string query)
        {
            return AllMedia.Where(m =>
                m.Title.ToLower().Contains(query.ToLower()) ||
                m.Artist.ToLower().Contains(query.ToLower()))
                .ToList();
        }

        public List<MediaFile> FilterByType(MediaType type)
        {
            return AllMedia.Where(m => m.Type == type).ToList();
        }

        public void GetRecentlyPlayed()
        {
            // Returns most recent 20 played items
        }

        public void SetMostPlayed()
        {
            // Track play counts
        }

        public void LoadLibrary()
        {
            var files = FileManager.BrowseFiles();
            foreach (var file in files)
            {
                AddMediaFile(file);
            }
        }

        public void SaveLibrary()
        {
            // Save library state
        }
    }
}