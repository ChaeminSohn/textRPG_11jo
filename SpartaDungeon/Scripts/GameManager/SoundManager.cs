using System.Net;
using NAudio.Wave;

namespace SpartaDungeon
{
    public static class SoundManager
    {
        static IWavePlayer? waveOutDevice;
        static AudioFileReader? audioFileReader;

        public static void PlayBgm(string path)
        {
            StopBgm();

            if (File.Exists(path))
            {
                try
                {
                    audioFileReader = new AudioFileReader(path);
                    var loop = new LoopStream(audioFileReader);
                    waveOutDevice = new WaveOutEvent();
                    waveOutDevice.Init(loop);
                    waveOutDevice.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"BGM 재생 오류: {ex.Message}");
                }
            }
        }

        public static void StopBgm()
        {
            if (waveOutDevice != null)
            {
                waveOutDevice.Stop();
                waveOutDevice.Dispose();
                waveOutDevice = null;
            }
            if (audioFileReader != null)
            {
                audioFileReader.Dispose();
                audioFileReader = null;
            }
        }
    }
}