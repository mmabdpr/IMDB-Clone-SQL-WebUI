namespace P0.Settings
{
    public static class FileSettings
    {
        public static string DataPath => $"Data/DB/";
        public static string FilmFilePath => $"{DataPath}Film.txt";
        public static string ArtistFilePath => $"{DataPath}Artist.txt";
        public static string ArtistIdIndexFilePath => $"{DataPath}ArtistIDIndex.txt";
        public static string ArtistNameIndexFilePath => $"{DataPath}ArtistNameIndex.txt";
        public static string[] FilePaths => new[] { FilmFilePath, ArtistFilePath, ArtistIdIndexFilePath, ArtistNameIndexFilePath };
    }
}