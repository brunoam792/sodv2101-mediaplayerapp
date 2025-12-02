using System;
using System.Collections.Generic;
using System.Linq;

namespace MediaPlayerApp
{
    public class Playlist
    {
        public string Name { get; set; }
        public List<MediaFile> MediaFiles { get; private set; }
        public int CurrentIndex { get; set; }
        public bool Shuffle { get; set; }
        public RepeatMode RepeatMode { get; set; }

        public Playlist(string name)
        {
            Name = name;
            MediaFiles = new List<MediaFile>();
            CurrentIndex = 0;
            Shuffle = false;
            RepeatMode = RepeatMode.None;
        }

        public void AddMedia(MediaFile mediaFile)
        {
            MediaFiles.Add(mediaFile);
        }

        public void RemoveMedia(MediaFile mediaFile)
        {
            MediaFiles.Remove(mediaFile);
        }

        public MediaFile GetNextMedia()
        {
            if (MediaFiles.Count == 0) return null;

            if (RepeatMode == RepeatMode.One)
            {
                return MediaFiles[CurrentIndex];
            }

            if (Shuffle)
            {
                Random rnd = new Random();
                CurrentIndex = rnd.Next(MediaFiles.Count);
            }
            else
            {
                CurrentIndex++;
                if (CurrentIndex >= MediaFiles.Count)
                {
                    if (RepeatMode == RepeatMode.All)
                        CurrentIndex = 0;
                    else
                        return null;
                }
            }

            return MediaFiles[CurrentIndex];
        }

        public MediaFile GetPreviousMedia()
        {
            if (MediaFiles.Count == 0) return null;

            CurrentIndex--;
            if (CurrentIndex < 0)
            {
                CurrentIndex = RepeatMode == RepeatMode.All ? MediaFiles.Count - 1 : 0;
            }

            return MediaFiles[CurrentIndex];
        }

        public MediaFile GetCurrentMedia()
        {
            if (MediaFiles.Count == 0 || CurrentIndex < 0 || CurrentIndex >= MediaFiles.Count)
                return null;
            return MediaFiles[CurrentIndex];
        }

        public int GetMediaCount()
        {
            return MediaFiles.Count;
        }
    }
}