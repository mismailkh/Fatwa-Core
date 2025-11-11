namespace FATWA_ADMIN.Data
{
    public class SystemSettingState
    {
        public bool IsInitialized { get; set; }
        public int Grid_Pagination { get; set; }
        public int Book_Copy_Count { get; set; }
        public int Eligible_Count { get; set; }
        public int Borrow_Period { get; set; }
        public int Extension_Period { get; set; }
        public int File_Minimum_Size { get; set; }
        public int File_Maximum_Size { get; set; }
        public IEnumerable<int> Page_Size_Options { get; set; } = new List<int> { 10, 25, 50, 100 };
        public IEnumerable<string> FileTypes { get; set; } = new List<string>();
    }
}
