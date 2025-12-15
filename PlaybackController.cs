using System;
using AxWMPLib;
using WMPLib;

namespace MediaPlayerApp
{
    public class PlaybackController
    {
        public AxWMPLib.AxWindowsMediaPlayer MediaPlayer { get; private set; }
        public MediaFile CurrentMedia { get; private set; }
        public bool IsPlaying { get; private set; }
        public int Volume { get; private set; }

        public event EventHandler MediaEnded;
        public event EventHandler<TimeSpan> PositionChanged;

        public PlaybackController(AxWMPLib.AxWindowsMediaPlayer playerControl)
        {
            MediaPlayer = playerControl;
            MediaPlayer.PlayStateChange += MediaPlayer_PlayStateChange;

            Volume = 50;
            MediaPlayer.settings.volume = Volume;
        }

        private void MediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            int NewState = e.newState;

            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                IsPlaying = false;
                MediaEnded?.Invoke(this, EventArgs.Empty);
            }
            else if (NewState == (int)WMPLib.WMPPlayState.wmppsPlaying)
            {
                IsPlaying = true;
            }
            else if (NewState == (int)WMPLib.WMPPlayState.wmppsPaused ||
                     NewState == (int)WMPLib.WMPPlayState.wmppsStopped)
            {
                IsPlaying = false;
            }
        }

        public void Play()
        {
            if (CurrentMedia != null)
            {
                MediaPlayer.Ctlcontrols.play();
            }
        }

        public void Pause()
        {
            MediaPlayer.Ctlcontrols.pause();
        }

        public void Stop()
        {
            MediaPlayer.Ctlcontrols.stop();
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Max(0, Math.Min(100, volume));
            MediaPlayer.settings.volume = Volume;
        }

        /// <summary>
        /// Sets the current playback position.
        /// </summary>
        public void Seek(TimeSpan position)
        {
            MediaPlayer.Ctlcontrols.currentPosition = position.TotalSeconds;
        }

        public void LoadMediaFile(MediaFile mediaFile)
        {
            CurrentMedia = mediaFile;
            MediaPlayer.URL = mediaFile.FilePath;
        }

        /// <summary>
        /// Returns the current playback position from the player control.
        /// </summary>
        public TimeSpan GetCurrentPosition()
        {
            return TimeSpan.FromSeconds(MediaPlayer.Ctlcontrols.currentPosition);
        }

        /// <summary>
        /// Returns the total duration of the currently loaded media.
        /// </summary>
        public TimeSpan GetDuration()
        {
            // Use currentMedia?.duration with null coalescing operator (?? 0) for safety
            return TimeSpan.FromSeconds(MediaPlayer.currentMedia?.duration ?? 0);
        }

        /// <summary>
        /// Helper method to be called by a Timer in the main Form, firing the PositionChanged event.
        /// </summary>
        public void UpdatePosition()
        {
            if (IsPlaying)
            {
                PositionChanged?.Invoke(this, GetCurrentPosition());
            }
        }
    }
}