namespace SpartaDungeon
{
    public static class PathConstants
    {
        public const string ResourceFolder = @"..\..\..\resources";
        public const string AudioFolder = @"..\..\..\Audio";
        public static string SaveFilePath => Path.Combine(ResourceFolder, "saveData.json");
        public static string ItemConfigPath => Path.Combine(ResourceFolder, "items_config.json");
        public static string MonsterConfigPath => Path.Combine(ResourceFolder, "monster_config.json");
        public static string QuestConfigPath => Path.Combine(ResourceFolder, "quest_config.json");
        public static string AudioFolderPath => AudioFolder;
    }
}