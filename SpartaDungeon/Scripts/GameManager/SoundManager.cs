using System.Net;
using NAudio.Wave;

namespace SpartaDungeon
{
    public static class SoundManager
    {
        static IWavePlayer? waveOutDevice;
        static AudioFileReader? audioFileReader;
        static string? currentBgmName;  //현재 재생중인 BGM

        public static void PlayBgm(string fileName)
        {
            // 이미 같은 음악을 재생 중이면 아무것도 하지 않음
            if (currentBgmName == fileName && waveOutDevice?.PlaybackState == PlaybackState.Playing)
            {
                return;
            }
            StopBgm();
            string path = Path.Combine(PathConstants.AudioFolder, fileName);
            if (File.Exists(path))
            {
                try
                {
                    audioFileReader = new AudioFileReader(path);
                    var loop = new LoopStream(audioFileReader);
                    waveOutDevice = new WaveOutEvent();
                    waveOutDevice.Init(loop);
                    waveOutDevice.Play();
                    currentBgmName = fileName;
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