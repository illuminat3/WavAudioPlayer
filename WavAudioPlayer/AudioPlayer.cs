﻿using System;
using System.Runtime.InteropServices;

namespace WavAudioPlayer
{
    public class AudioPlayer
    {
        [DllImport("winmm.dll", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern bool PlaySound(string pszSound, IntPtr hmod, uint fdwSound);

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
