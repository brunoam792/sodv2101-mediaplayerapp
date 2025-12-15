using System;

namespace MediaPlayerApp
{
    public enum MediaType
    {
        Audio,
        Video
    }

    public enum RepeatMode
    {
        None, // Stop when the playlist ends
        One,  // Repeat the current track indefinitely
        All   // Loop the entire playlist
    }
}