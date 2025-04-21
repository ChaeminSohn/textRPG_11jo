using System.Text.Json;
using System.Text.Json.Serialization;

namespace SpartaDungeon
{
    internal static class ConfigLoader  //json 파일 읽어오기 전용 클래스
    {
        public static bool TryLoad<T>(string path, out T result)
        {
            try
            {
                string json = File.ReadAllText(path);
                var options = new JsonSerializerOptions
                {
                    Converters = { new JsonStringEnumConverter() }  //Enum 대응
                };
                result = JsonSerializer.Deserialize<T>(json, options);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"게임 데이터 로드 중 오류 발생: {ex.Message}");
                result = default(T);
                return false;
            }
        }
    }
}