using System.Text.Json;

namespace SpartaDungeon
{
    public static class SaveLoadManager
    {
        public static void SaveGame(GameSaveData saveData)
        {
            string json = JsonSerializer.Serialize(saveData);
            File.WriteAllText(PathConstants.SaveFilePath, json);
        }

        public static bool TryLoadGame(out GameSaveData saveData)
        {
            if (!ConfigLoader.TryLoad(PathConstants.SaveFilePath, out saveData))
                return false;

            return true;
        }
    }
}