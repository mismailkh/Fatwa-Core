using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Data;
using HarfBuzzSharp;
using Radzen;
using Syncfusion.XlsIO;
using Syncfusion.XlsIO.FormatParser.FormatTokens;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using Telerik.DataSource.Extensions;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_WEB.Services
{
    public class ExcelExportService
    {
        private readonly DialogService _dialogService;
        private readonly TranslationState _translationState;
        private readonly userService _userService;
        private readonly GroupService _groupService;
        private readonly LookupService _lookupService;

        protected IEnumerable<Nationality> Nationalities { get; set; }
        protected IEnumerable<EmployeeType> EmployeeTypes { get; set; }
        protected IEnumerable<Gender> Genders { get; set; }
        protected IEnumerable<Designation> Designations { get; set; }
        protected IEnumerable<Company> Companies { get; set; }
        protected IEnumerable<Department> Departments { get; set; }
        protected IEnumerable<Grade> EmployeeGradeList { get; set; }
        protected IEnumerable<OperatingSectorType> OperatingSectorTypes { get; set; }
        protected List<EmployeeWorkingTime> WorkingTimes { get; set; }
        protected IEnumerable<GroupTypeWebSystemVM> GroupAccessTypes { get; set; }
        protected List<Group> Groups { get; set; }
        protected List<Role> Roles { get; set; }

        public ExcelExportService(DialogService dialogService, TranslationState translationState, userService userService, GroupService groupService, LookupService lookupService)
        {
            _dialogService = dialogService;
            _translationState = translationState;
            _userService = userService;
            _groupService = groupService;
            _lookupService = lookupService;
        }

        public async Task<byte[]> ExportToExcel()
        {
            await PopulateLookupValues();
            ExcelEngine excelEngine = new ExcelEngine();
            IApplication application = excelEngine.Excel;

            IWorkbook workbook = application.Workbooks.Create(1);
            IWorksheet worksheet = workbook.Worksheets[0];

            // Get column names from EF model
            var columnNames = GetColumnNamesFromEFModels();

            // Write column names to Excel
            for (int i = 0; i < columnNames.Count; i++)
            {
                if (columnNames[i] != "CreatedBy")
                    worksheet[1, i + 1].Value = columnNames[i];
            }

            // Make dropdowns for lookup columns (assuming you have a method GetLookupValuesForColumn)
            foreach (var columnName in columnNames)
            {
                if (columnName.StartsWith("LKP"))
                {
					var columnNumber = columnNames.IndexOf(columnName) + 1;
					var validationValuesWorksheet = workbook.Worksheets.Create(columnName);
                    if (columnName == "LKP_SectorType")
                    {
                        int i = 1;
                        foreach (Department dep in Departments)
                        {
                            int startingValue = i;
                            for (int j = 0; j < dep.SectorTypes.Count; i++, j++)
                            {
                                validationValuesWorksheet[i, 1].Value = dep.Id.ToString();
                                validationValuesWorksheet[i, 2].Value = dep.SectorTypes.ElementAt(j).Id + "," +dep.SectorTypes.ElementAt(j).Name_Ar;
                            }
                            if (dep.SectorTypes.Count > 0)
                            {
                                // Values Range
                                var validationValuesRange = validationValuesWorksheet.Range["B" + startingValue + ":B" + (i - 1)];
                                IName valueRange = workbook.Names.Add("_" + dep.Id + "_Sectors_range");
                                valueRange.RefersToRange = validationValuesRange;
                                // Validation
                                var range = worksheet.Range[2, columnNumber, 10000, columnNumber];
                                IDataValidation dataValidation = range.DataValidation;
                                dataValidation.ListOfValues = new string[] { };
                                dataValidation.CompareOperator = ExcelDataValidationComparisonOperator.Between;
                                //Switching values based on department
                                var depColNum = columnNames.IndexOf("LKP_Department") + 1;
                                dataValidation.FirstFormula = "=INDIRECT(\"_\"&LEFT(INDIRECT(ADDRESS(ROW(),"+ depColNum + ")), FIND(\",\", INDIRECT(ADDRESS(ROW(),"+ depColNum + "))) - 1)&\"_Sectors_range\")";


                            }
                        }
                    }
                    //else if (columnName == "LKP_Group") //Replaced used name instead of custom text for now
                    //{
                    //    Dictionary<string, List<string>> dict = GetDependantGroups();
                    //    int i = 1;
                    //    foreach (string key in dict.Keys)
                    //    {
                    //        int startingValue = i;
                    //        for (int j = 0; j < dict[key].Count; i++, j++)
                    //        {
                    //            validationValuesWorksheet[i, 1].Value = key;
                    //            validationValuesWorksheet[i, 2].Value = dict[key][j];
                    //        }
                    //        if (dict[key].Count > 0)
                    //        {
                    //            var validationValuesRange = validationValuesWorksheet.Range["B" + startingValue + ":B" + (i - 1)];
                    //            var nameRange = key.Replace(" ", "_");
                    //            IName valueRange = workbook.Names.Add(nameRange);
                    //            valueRange.RefersToRange = validationValuesRange;
                    //            Validation
                    //           var range = worksheet.Range[2, columnNumber, 10000, columnNumber];
                    //            IDataValidation dataValidation = range.DataValidation;
                    //            dataValidation.ListOfValues = new string[] { };
                    //            dataValidation.CompareOperator = ExcelDataValidationComparisonOperator.Between;
                    //            Switching values based on department
                    //            var grpTypeNum = columnNames.IndexOf("LKP_GroupType") + 1;
                    //            dataValidation.FirstFormula = "=INDIRECT(SUBSTITUTE(INDIRECT(ADDRESS(ROW()," + grpTypeNum + ")),\" \",\"_\"))";
                    //        }
                    //    }
                    //}
                    else
                    {
                        var lookupValues = await GetLookupValuesForColumn(columnName);
                        if (lookupValues != null && lookupValues.Count > 0)
                        {
                            for (int i = 1; i <= lookupValues.Count; i++)
                            {
                                validationValuesWorksheet[i, 1].Value = lookupValues[i - 1].Id + "," + lookupValues[i - 1].NameAr;
                            }
                            var validationValuesRange = validationValuesWorksheet.Range["A1:A" + lookupValues.Count];
                            IName namedRange = workbook.Names.Add(columnName + "_range");
                            namedRange.RefersToRange = validationValuesRange;
                            var range = worksheet.Range[2, columnNumber, 10000, columnNumber];
                            IDataValidation dataValidation = range.DataValidation;
                            dataValidation.ListOfValues = new string[] { };
                            dataValidation.FirstFormula = "=" + columnName + "_range";
                        }
                    }
                }
            }
            worksheet.UsedRange.AutofitColumns();
            // Save the file
            FileStream stream = new FileStream("Output.xlsx", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            workbook.SaveAs(stream);
            workbook.Close();
            stream.Close();
            excelEngine.Dispose();
            var content = File.ReadAllBytes("Output.xlsx");
            File.Delete("Output.xlsx");
            return content;
        }

        private List<string> GetColumnNamesFromEFModels()
        {
            // Implement logic to get column names from your EF models
            // For example, you can use reflection to get property names
            // Replace this with your actual implementation
            return typeof(ImportEmployeeTemplate).GetProperties().Select(p => p.Name).ToList();
        }

        private async Task<List<LKPMapper>> GetLookupValuesForColumn(string columnName)
        {
            // Implement logic to get lookup values for the specified column
            // Replace this with your actual implementation
            switch (columnName)
            {
                case "LKP_Nationality":
                   return Nationalities.Select(x => new LKPMapper { Id = x.Id.ToString() ,NameAr = x.Name_Ar }).ToList();
                case "LKP_Gender":
                    return Genders.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_EmployeeType":
                    return EmployeeTypes.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
				case "LKP_Grade":
                    return EmployeeGradeList.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_Designation":
                    return Designations.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_Company":
                    return Companies.Select(x => new LKPMapper { Id = x.CompanyId.ToString(), NameAr = x.NameAr }).ToList();
                case "LKP_WorkingTime":
                    return WorkingTimes.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_Department":
                    return Departments.Select(x => new LKPMapper { Id = x.Id.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_GroupType":
                    return GroupAccessTypes.Select(x => new LKPMapper { Id = x.GroupTypeId.ToString(), NameAr = x.Name }).ToList();
                case "LKP_Group":
                    return Groups.Select(x => new LKPMapper { Id = x.GroupId.ToString(), NameAr = x.Name_Ar }).ToList();
                case "LKP_Role":
                    return Roles.Select(x => new LKPMapper { Id = 1.ToString(), NameAr = x.NameAr }).ToList();
                default:
                    return null;
            }
        }

        private class LKPMapper
        {
            public string Id { get; set; }
            public string NameAr { get; set; }
        }
        private async Task PopulateLookupValues()
        {
            await GetNationalityData();
            await GetGenders();
            await GetEmployeeDesignation();
            await GetEmployeeDepartment();
            await GetEmployeeSectortype();
            await GetEmployeeGrade();
            await PopulateWorkingTime();
            await GetCompanies();
            await PopulateGroupTypes();
            await PopulateGroups();
            await GetEmployeeType();
            await PopulateRole();
		}

        private async Task PopulateGroupTypes()
        {
            var response = await _groupService.GetGroupAccessTypes();
            if (response.IsSuccessStatusCode)
            {
                GroupAccessTypes = (IEnumerable<GroupTypeWebSystemVM>)response.ResultData;
            }
        }

        private async Task PopulateGroups()
        {
            var response = await _groupService.GetGroups();
            if (response.IsSuccessStatusCode)
            {
                Groups = (List<Group>)response.ResultData;
            }
        }

        protected async Task PopulateRole()
        {
            var response = await _userService.GetRoles();
            if (response.IsSuccessStatusCode)
            {
                Roles = (List<Role>)response.ResultData;
            }
        }

        private async Task GetNationalityData()
        {
            var response = await _userService.GetNationalityData();
            if (response.IsSuccessStatusCode)
            {
                Nationalities = (IEnumerable<Nationality>)response.ResultData;
            }
        }
        private async Task GetEmployeeType()
        {
            var response = await _userService.GetEmployeeType();
            if (response.IsSuccessStatusCode)
            {
                EmployeeTypes = (IEnumerable<EmployeeType>)response.ResultData;
            }
        }

        private async Task GetGenders()
        {
            var response = await _userService.GetGenders();
            if (response.IsSuccessStatusCode)
            {
                Genders = (IEnumerable<Gender>)response.ResultData;
            }
        }
        private async Task GetEmployeeDesignation()
        {
            var response = await _lookupService.GetDesignationList();
            if (response.IsSuccessStatusCode)
            {
                Designations = (IEnumerable<Designation>)response.ResultData;
            }
        }
        private async Task GetCompanies()
        {
            var response = await _userService.GetCompanies();
            if (response.IsSuccessStatusCode)
            {
                Companies = (IEnumerable<Company>)response.ResultData;
            }

        }
        private async Task GetEmployeeDepartment()
        {
            var response = await _userService.GetEmployeeDepartment();
            if (response.IsSuccessStatusCode)
            {
                Departments = (IEnumerable<Department>)response.ResultData;
            }
        }
        public async Task GetEmployeeSectortype()
        {
            var response = await _lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                OperatingSectorTypes = (IEnumerable<OperatingSectorType>)response.ResultData;
            }

        }
        private async Task GetEmployeeGrade()
        {
            var response = await _userService.GetEmployeeGrade();
            if (response.IsSuccessStatusCode)
            {
                EmployeeGradeList = (IEnumerable<Grade>)response.ResultData;
            }
        }


        private async Task PopulateWorkingTime()
        {
            var response = await _userService.GetWorkingTime();
            if (response.IsSuccessStatusCode)
            {
                WorkingTimes = (List<EmployeeWorkingTime>)response.ResultData;
            }
        }

        private Dictionary<string, List<string>> GetDependantGroups()
        {
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();

            foreach (GroupTypeWebSystemVM accessType in GroupAccessTypes)
            {
                dict.Add(accessType.Name, Groups.Where(x => x.GroupTypeId == accessType.GroupTypeId).Select(x => x.Name_En).ToList());
            }
            return dict;

        }

    }
}
