using System;

namespace MediaPlayerApp
{
    public class PlaybackController
    {
        public WMPLib.WindowsMediaPlayer MediaPlayer { get; private set; }
        public MediaFile CurrentMedia { get; private set; }
        public bool IsPlaying { get; private set; }
        public int Volume { get; set; }
        public TimeSpan CurrentPosition { get; set; }

        public event EventHandler MediaEnded;
        public event EventHandler<TimeSpan> PositionChanged;

        public PlaybackController()
        {
            MediaPlayer = new WMPLib.WindowsMediaPlayer();
            MediaPlayer.PlayStateChange += MediaPlayer_PlayStateChange;
            Volume = 50;
            MediaPlayer.settings.volume = Volume;
        }

        private void MediaPlayer_PlayStateChange(int NewState)
        {
            if (NewState == (int)WMPLib.WMPPlayState.wmppsMediaEnded)
            {
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
                MediaPlayer.URL = CurrentMedia.FilePath;
                MediaPlayer.controls.play();
                IsPlaying = true;
            }
        }

        public void Pause()
        {
            MediaPlayer.controls.pause();
            IsPlaying = false;
        }

        public void Stop()
        {
            MediaPlayer.controls.stop();
            IsPlaying = false;
        }

        public void Next()
        {
            // Handled by MediaPlayerForm
        }

        public void Previous()
        {
            // Handled by MediaPlayerForm
        }

        public void SetVolume(int volume)
        {
            Volume = Math.Max(0, Math.Min(100, volume));
            MediaPlayer.settings.volume = Volume;
        }

        public void Seek(TimeSpan position)
        {
            MediaPlayer.controls.currentPosition = position.TotalSeconds;
            CurrentPosition = position;
        }

        public void LoadMediaFile(MediaFile mediaFile)
        {
            CurrentMedia = mediaFile;
            MediaPlayer.URL = mediaFile.FilePath;
        }

        public TimeSpan GetCurrentPosition()
        {
            return TimeSpan.FromSeconds(MediaPlayer.controls.currentPosition);
        }

        public TimeSpan GetDuration()
        {
            return TimeSpan.FromSeconds(MediaPlayer.currentMedia?.duration ?? 0);
        }
    }
}