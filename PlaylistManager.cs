using System;
using System.Collections.Generic;
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

        public void LoadPlaylist(string filePath)
        {
            // Load playlist from file
        }

        public void SavePlaylist(Playlist playlist)
        {
            // Save playlist to file
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