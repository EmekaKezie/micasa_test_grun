namespace TheEstate.Models.AppModels
{
    public class FileModel
    {
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string FileMime { get; set; } = string.Empty;
    }
}
