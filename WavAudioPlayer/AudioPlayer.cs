using System;
using System.Runtime.InteropServices;

namespace WavAudioPlayer
{
    public class AudioPlayer
    {
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto, EntryPoint = "waveOutSetVolume")]
        private static extern bool WaveOutSetVolume(IntPtr hwo, uint dwVolume);

        // Flags for playing sound
        private const uint SND_ASYNC = 0x0001;
        private const uint SND_FILENAME = 0x00020000;
        public const uint SND_PURGE = 0x0040;

        private bool disposed = false;

        /// <summary>
        /// Plays a WAV file located at the specified file path.
        /// </summary>
        /// <param name="filepath">The path to the WAV file.</param>
        public void PlaySound(string filepath)
        {
            if (string.IsNullOrEmpty(filepath))
            {
                throw new ArgumentException("Filepath cannot be null or empty.", nameof(filepath));
            }

            bool result = PlaySound(filepath, IntPtr.Zero, SND_ASYNC | SND_FILENAME);

            if (!result)
            {
                throw new Exception("Failed to play sound. Ensure the file path is correct and the file is a valid WAV file.");
            }
        }

        public void StopSound()
        {
            PlaySound(null, IntPtr.Zero, SND_PURGE);
        }

        private void SetVolume(int volume)
        {
            volume = Clamp(volume, 0, 100);

            uint scaledVolume = (uint)((volume * 0xFFFF / 100) & 0xFFFF);
            uint volumeAllChannels = scaledVolume | (scaledVolume << 16);

            WaveOutSetVolume(IntPtr.Zero, volumeAllChannels);
        }

        private int Clamp(int value, int min, int max)
        {
            return (value < min) ? min : (value > max) ? max : value;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                StopSound();
                disposed = true;
            }
        }

        ~AudioPlayer()
        {
            Dispose(false);
        }
    }
}
