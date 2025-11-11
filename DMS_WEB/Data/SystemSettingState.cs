using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.Net.Http.Headers;

namespace DMS_WEB.Data
{
    //<History Author = 'Hassan Abbas' Date='2022-09-12' Version="1.0" Branch="master"> System Setting State configured to manage different settings from Admin Portal</History>
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
    }
}
