using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.ContactManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Reflection.Metadata;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_WEB.Pages.CreateUser;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;


//< History Author = "Attique ur Rehman" Date = "16-10-23" Version = "1.0" Branch = "master" > Create / Updating  Internal/External Employees for fatwa Portal</ History >
namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class AddEmployee : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string? Id { get; set; }
        [Parameter]
        public int EmployeeType { get; set; }
        #endregion

        #region Variables
        DateTime MaxDoB = DateTime.Today.AddYears(-18);
        public string REGEX_EMAIL_ADDRESS = "^[_A-Za-z0-9-\\+]+(\\.[_A-Za-z0-9-]+)*@[A-Za-z0-9-]+(\\.[A-Za-z0-9]+)*(\\.[A-Za-z]{2,})$";
        public string REGEX_MOBILE_NO = "[4,5,6,9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]";
        public string REGEX_Fingerprint_Id = "[0-6]";
        bool isEditEmployee = false;
        bool isEditEducation = false;
        bool isEditTraining = false;
        bool isEditAddress = false;
        bool isEditContact = false;
        bool IsUserHasAnyTask = false;
        bool isEditWorkExperience = false;
        bool isBusy = false;
        string ExistingCivilId = "";
        string ExistingEmail = "";
        string ExistingEmployeeId = "";
        public bool isCollapseAccordian { get; set; } = true;
        public static string addEducationText = "Add_Education";
        public static string addAddressText = "Add_Address";
        public static string addWorkExperienceText = "Add_Work_Experience";
        public static string addTrainingText = "Add_Training";
        public Guid EducationId { get; set; }
        public Guid AddressId { get; set; }
        public Guid ExperienceId { get; set; }
        public Guid TrainingId { get; set; }
        protected string civilIdValidationMsg = "";
        bool isValidIdCheck;
        bool IsDOJValid;
        string IsDOJValidMsg;

        #region Grids
        protected RadzenDataGrid<UserAdress>? addressGrid = new RadzenDataGrid<UserAdress>();
        protected RadzenDataGrid<UserContactInformation>? contactInformationGrid = new RadzenDataGrid<UserContactInformation>();
        protected RadzenDataGrid<UserWorkExperience>? workExperienceGrid = new RadzenDataGrid<UserWorkExperience>();
        protected RadzenDataGrid<UserTrainingAttended>? userTrainingsGrid = new RadzenDataGrid<UserTrainingAttended>();
        protected RadzenDataGrid<UserEducationalInformation>? educationalInformationGrid = new RadzenDataGrid<UserEducationalInformation>();
        #endregion 

        DateTime value;
        Nationality Nationality = new Nationality();
        Gender Gender = new Gender();
        Company Company { get; set; } = new Company
        {
            City = new City
            {
                Governorate = new Governorate
                {
                    Country = new Country()
                }
            },
        };
        UserEducationalInformation EduInformation = new UserEducationalInformation();
        UserWorkExperience UserWorkExperience = new UserWorkExperience();
        UserTrainingAttended UserTrainingAttended = new UserTrainingAttended();
        UserAdress UserAdressoBJ = new UserAdress();
        IEnumerable<UserDataVM> UsersList { get; set; }
        List<ManagersListVM> ManagersList { get; set; }
        IEnumerable<UserDataVM> SupervisorsList { get; set; }
        UserContactInformation UserContact = new UserContactInformation();
        IEnumerable<City> Cities { get; set; }
        IEnumerable<ContactType> ContactTypes { get; set; }
        IEnumerable<Country> Countries { get; set; }
        IEnumerable<Company> Companies { get; set; }
        IEnumerable<Department> Departments { get; set; }
        IEnumerable<OperatingSectorType> OperatingSectorTypes { get; set; }
        protected IEnumerable<Nationality> Nationalities { get; set; }
        IEnumerable<OperatingSectorType> OriginalSectorslist { get; set; }
        protected IEnumerable<Gender> Genders { get; set; }
        IEnumerable<Grade> EmployeeGradeList { get; set; }
        IEnumerable<GradeType> GradeTypeList { get; set; }
        IEnumerable<GradeType> DuplicateGradeTypeList { get; set; }
        IEnumerable<ContractType> ContractTypeList { get; set; }
        protected IEnumerable<GroupTypeWebSystemVM> groupAccessTypes { get; set; }
        protected List<Group> Groups { get; set; }
        protected List<Role> Roles { get; set; }
        IEnumerable<EmployeeStatus> employeeStatuses { get; set; }
        public List<EmployeeWorkingTime> WorkingTimes { get; set; } = new List<EmployeeWorkingTime>();
        public RadzenDropDown<string?> CompanyDropDown { get; set; }
        public RadzenDropDown<string?> DesignationDropDown { get; set; }
        public IEnumerable<Designation> DesignationList { get; set; }
        protected List<SectorFloor> Floors { get; set; }

        AddEmployeeVM EmployeeVM = new AddEmployeeVM
        {
            UserId = Guid.NewGuid(),
            userPersonalInformation = new UserPersonalInformation(),
            UserAdresses = new List<UserAdress>(),
            UserEmploymentInformation = new UserEmploymentInformation(),
            UserEducationalInformation = new List<UserEducationalInformation>(),
            UserContactInformationList = new List<UserContactInformation>(),
            UserWorkExperiences = new List<UserWorkExperience>(),
            userTrainingAttendeds = new List<UserTrainingAttended>()
        };
        bool RefreshGrid = false;
        string EmployeeTyp = string.Empty;
        IEnumerable<OperatingSectorType> DuplicateOSectorslist { get; set; }
        public bool ShowHyphenHint { get; set; }
        public bool AutoHyphenAdd { get; set; } = false;
        #endregion

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            EmployeeTyp = EmployeeType.ToString();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await GetNationalityData();
            await GetGenders();
            await GetCities();
            await GetCountries();
            await GetEmployeeDepartment();
            await PopulateWorkingTime();
            await GetCompanies();
            await PopulateGroupTypes();
            await PopulateRole();
            await GetEmployeeStatus();
            await GetUserData();
            await GetDesignationList();
            await GetContractTypes();
            await GetContactTypes();

            if (Id != null)
            {
                await LoadEmployeeForm();
            }
            else
            {
                EmployeeVM.UserEmploymentInformation.EmployeeStatusId = (int)EmployeeStatusEnum.Active;
            }
            await GetEmployeeSectortype();
            await GetGradeTypes();
        }

        #endregion

        #region Load Employee Form by Id

        public async Task LoadEmployeeForm()
        {
            isEditEmployee = true;
            var response = await userService.GetEmployeeDetailById(Guid.Parse(Id));

            if (response.IsSuccessStatusCode)
            {
                EmployeeVM = (AddEmployeeVM)response.ResultData;
                ExistingCivilId = EmployeeVM.userPersonalInformation.CivilId;
                ExistingEmail = EmployeeVM.Email;
                IsUserHasAnyTask = EmployeeVM.IsUserHasAnyTask;
                ExistingEmployeeId = EmployeeVM.UserEmploymentInformation.EmployeeId;
                EmployeeVM.GradeTypeId = EmployeeVM.UserEmploymentInformation.Grade.GradeTypeId;
                //await GetEmployeeSectortype();
                await LoadGroups();
                await GetCompanyInfo();
                //await GetEmployeeStatus();
                await GetCompanyList();
                // await GetDesignationList();
                await GetEmployeeGrade();
                await GetContactTypes();
                //await GetUserData();
                await PopulateManagerDropdown();
                GetSupervisorsBySectorId();
                //FilterOperatingSectorsOnDepartmentId();
                StateHasChanged();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion

        #region Validate Date Range for Work Experience / Trainings 
        public async Task TenureValidityCheck()

        {
            if (UserWorkExperience.StartDate > UserWorkExperience.EndDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                UserWorkExperience.StartDate = null;
                UserWorkExperience.EndDate = null;
                return;
            }
            else if (UserWorkExperience.StartDate > EmployeeVM.UserEmploymentInformation.DateOfJoining)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Wrong_Date_Of_Joining_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                UserWorkExperience.StartDate = null;
                EmployeeVM.UserEmploymentInformation.DateOfJoining = null;
                return;
            }

        }

        public async Task ValidTrainingDateCheck()

        {
            if (UserTrainingAttended.StartDate > UserTrainingAttended.EndDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                UserTrainingAttended.StartDate = null;
                UserTrainingAttended.EndDate = null;
                return;
            }

        }
        #endregion

        #region Check Existanse of Civil Id / Email / Employee Id
        bool isCivilIdExist = false;
        bool isEmailExist = false;
        protected async Task<bool> ExistsCivilIdAndEmail()
        {
            if (ExistingCivilId != EmployeeVM.userPersonalInformation.CivilId)
            {
                isCivilIdExist = await userService.CheckCivilIdExists(EmployeeVM.userPersonalInformation.CivilId);
                if (isCivilIdExist == true)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("CivilId_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return isCivilIdExist;
                }
            }
            if (ExistingEmail != EmployeeVM.Email)
            {
                EmployeeVM.IsEmailModified = true;
                isEmailExist = await userService.CheckEmailExists(EmployeeVM.Email);
                if (isEmailExist == true)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Email_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return isEmailExist;
                }

            }
            return isCivilIdExist || isEmailExist;
        }
        protected async Task<bool> ExistsEmployeeId()
        {
            if (ExistingEmployeeId != EmployeeVM.UserEmploymentInformation.EmployeeId)
            {
                bool isExist = await userService.CheckEmployeeIdExist(EmployeeVM.UserEmploymentInformation.EmployeeId);
                if (isExist)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("EmployeeId_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return isExist;
                }
            }
            return false;
        }

        #endregion

        #region Validate CivilId Pattern
        protected async Task OnChangeCivilId(string civilId)
        {
            if (civilId.Length == 12)
            {
                isValidIdCheck = IsValidCivilId(civilId);
                civilIdValidationMsg = isValidIdCheck ? "" : translationState.Translate("Enter_Valid_CivilId");
            }
        }
        public static bool IsValidCivilId(string civilId)
        {
            int[] weights = new int[] { 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            if (!string.IsNullOrEmpty(civilId) && IsNumber(civilId) && civilId.Length == 12)
            {
                int checkDigit = int.Parse(civilId[11].ToString());
                int total = 0;
                for (int index = 0; index < weights.Length; index++)
                {
                    total += (int.Parse(civilId[index].ToString()) * weights[index]);
                }
                return (11 - (total % 11)) == checkDigit;
            }
            return false;
        }
        private static bool IsNumber(string value)
        {
            foreach (char c in value)
            {
                if (!char.IsDigit(c))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region Populate Educational Countries
        public async Task GetCountries()
        {
            var response = await userService.GetCountries();
            if (response.IsSuccessStatusCode)
            {
                Countries = (IEnumerable<Country>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        #endregion

        #region On Change Date Of Joining
        protected async Task OnChangeDOJ()
        {
            if (EmployeeVM.UserEmploymentInformation.DateOfJoining != null && EmployeeVM.userPersonalInformation.DateOfBirth != null)
            {
                if(EmployeeVM.UserEmploymentInformation.DateOfJoining< EmployeeVM.userPersonalInformation.DateOfBirth)
                {
                    IsDOJValid = false;
                    IsDOJValidMsg = translationState.Translate("DOJ_Cannot_less_than_DOB");
                }
                else
                {
                    IsDOJValid = true;
                    IsDOJValidMsg = "";
                }
            }
        }
        #endregion
        #region Handle Submit

        public async Task Form0Submit()
        {
            if (!IsDOJValid) return;

            isBusy = true;
            async Task<bool> ExecuteCheck(Func<Task<bool>> check)
            {
                var result = await check();
                if (result)
                {
                    isBusy = false; // Ensure isBusy is set to false before returning
                    return true;
                }
                return false;
            }

            if (await ExecuteCheck(ExistsCivilIdAndEmail)) return;
            if (await ExecuteCheck(ExistsEmployeeId)) return;
           

            if (EmployeeVM.UserContactInformationList.Count > 0)
            {
                if (await ExecuteCheck(CheckPrimaryContact)) return;
            }

            #region Mandatory Employee Address
            if (EmployeeType == (int)EmployeeTypeEnum.Internal)
            {
                if (EmployeeVM.UserAdresses.Count == 0)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Address_Required"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulateAddresses();
                    isBusy = false;
                    return;
                }
            }
            #endregion

            #region Check primary contact existence before submit with deletion -> Edit Employee.
            if (Id != null && EmployeeVM.UserContactInformationList.Count > 0 && EmployeeVM.UserContactInformationList != null)
            {
                if (EmployeeVM.UserContactInformationList.All(contact => contact.IsPrimary == false))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Add_One_primary_Contact"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    isBusy = false; // Ensure isBusy is set to false before returning
                    return;
                }
            }
            #endregion

            bool? dialogResponse = await dialogService.Confirm(
                    Id == null ? translationState.Translate("Confirm_Add_Employee") : translationState.Translate("Confirm_Update_Employee"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });

            if (dialogResponse == true)
            {
                spinnerService.Show();

                if (Id == null)
                {
                    EmployeeVM.UserEmploymentInformation.EmployeeTypeId = EmployeeType;
                    EmployeeVM.CreatedBy = loginState.UserDetail.Email;
                    EmployeeVM.CreatedDate = DateTime.Now;
                    SetNotificationParams();
                    var response = await userService.AddEmployee(EmployeeVM);

                    if (response.IsSuccessStatusCode)
                    {
                        spinnerService.Hide();
                        var responseSuccessData = (EmployeeSuccessVM)response.ResultData;
                        await dialogService.OpenAsync<EmployeeAddedDialog>(translationState.Translate("Employee_Added_Successfully"),
                           new Dictionary<string, object>()
                           {
                     {"userId",responseSuccessData.userId},
                     {"UserName",responseSuccessData.UserName},
                     {"EmployeeTypeId",responseSuccessData.EmployeeTypeId},
                     {"EmployeeId",responseSuccessData.EmployeeId}
                           },
                           new DialogOptions() { Width = "36% !important", CloseDialogOnOverlayClick = false });
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    EmployeeVM.ModifiedBy = loginState.UserDetail.Email;
                    var response = await userService.EditEmployee(EmployeeVM);
                    if (response.IsSuccessStatusCode)
                    {
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Employee_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        navigationManager.NavigateTo("/employee-list");
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                StateHasChanged();
            }
            else
            {
                isBusy = false;
            }
        }
        #endregion

        #region Set Notification Params
        private async void SetNotificationParams()
        {
            //await GetEmployeeSectortype();
            EmployeeVM.NotificationParameters.EmployeeName = EmployeeVM.userPersonalInformation.FirstName_En + " " + EmployeeVM.userPersonalInformation.SecondName_En + " " + EmployeeVM.userPersonalInformation.LastName_En + "/" + EmployeeVM.userPersonalInformation.FirstName_Ar + " " + EmployeeVM.userPersonalInformation.SecondName_Ar + " " + EmployeeVM.userPersonalInformation.LastName_Ar;
            EmployeeVM.NotificationParameters.SectorFrom = DuplicateOSectorslist.Where(x => x.Id == EmployeeVM.UserEmploymentInformation.SectorTypeId).Select(x => x.Name_En).FirstOrDefault().ToString() + "/" + DuplicateOSectorslist.Where(x => x.Id == EmployeeVM.UserEmploymentInformation.SectorTypeId).Select(x => x.Name_Ar).FirstOrDefault().ToString();
            EmployeeVM.NotificationParameters.CreatedDate = DateTime.Now;
            EmployeeVM.NotificationParameters.SenderName = loginState.UserDetail.FullNameEn + "/" + loginState.UserDetail.FullNameAr;
        }
        #endregion

        #region Handle form Invalid Submit
        protected async Task FormInvalidSubmit()
        {
            try
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Required_Field"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Employee Contact Management

        #region Populate Employee Contact Grid
        //< History Author = "Ammaar Naveed" Date = "30-01-2024" Version = "1.0" Branch = "master" > Add employee contact information in contact grid.</ History >
        public async Task AddEmployeeContactInformation()
        {
            var dialogResult = await dialogService.OpenAsync<AddContact>(
                translationState.Translate("Add_Contact"),
                null,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false }
            );

            if (dialogResult != null)
            {
                UserContact = dialogResult;
                if (UserContact.IsPrimary == true)
                {
                    await CheckExistingPrimaryContacts();

                }
                UserContact.UserId = EmployeeVM.UserId.ToString();
                EmployeeVM.UserContactInformationList.Add(UserContact);
                UserContact = new UserContactInformation();
                RefreshGrid = true;
                StateHasChanged();
                await Task.Delay(100);
                RefreshGrid = false;
            }
        }

        #endregion

        #region Existence Check of At Least One Primary Contact.
        //< History Author = "Ammaar Naveed" Date = "01-02-2024" Version = "1.0" Branch = "master" > Checking existence of primary contact while adding employee.</ History >
        protected async Task<bool> CheckPrimaryContact()
        {
            bool isContactListHasNoPrimary = EmployeeVM.UserContactInformationList.All(pContact => pContact.IsPrimary == null) || EmployeeVM.UserContactInformationList.All(contact => contact.IsPrimary == false);
            if (isContactListHasNoPrimary == true)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Atleast_One_Primary_Contact"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return isContactListHasNoPrimary;
            }
            return false;
        }
        #endregion

        #region Checking existence of more than one primary contacts.
        //< History Author = "Ammaar Naveed" Date = "01-02-2024" Version = "1.0" Branch = "master" > Check existence of primary contacts & making newly added contact as non-primary on choice.</ History >
        protected async Task CheckExistingPrimaryContacts()
        {
            if (EmployeeVM.UserContactInformationList.Count >= 1 && EmployeeVM.UserContactInformationList.Any(contactList => contactList.IsPrimary == true))
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Primary_Contact_Exists"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("Yes"),
                        CancelButtonText = @translationState.Translate("No")
                    });

                if (dialogResponse == true)

                {   //Making all existing contacts non-primary
                    foreach (var primaryContacts in EmployeeVM.UserContactInformationList)
                    {
                        primaryContacts.IsPrimary = false;
                    }
                    UserContact.IsPrimary = true;
                }
                else
                {
                    UserContact.IsPrimary = false;
                }
            }
        }
        #endregion        

        #endregion

        #region Populate Address Grid
        public async Task PopulateAddresses()
        {
            var dialogResult = await dialogService.OpenAsync<AddAddress>
              (
                  translationState.Translate("Add_Address"),
                  null,
                  new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
              );
            if (dialogResult != null)
            {
                UserAdressoBJ = dialogResult;
                UserAdressoBJ.UserId = EmployeeVM.UserId.ToString();
                UserAdressoBJ.City = Cities.Where(x => x.CityId == UserAdressoBJ.CityId).FirstOrDefault();
                EmployeeVM.UserAdresses.Add(UserAdressoBJ);
                UserAdressoBJ = new UserAdress();
                RefreshGrid = true;
                StateHasChanged();
                await Task.Delay(100);
                RefreshGrid = false;
            }
            else { addAddressText = "Add_Address"; }

        }
        #endregion

        #region Populate Contact Types Dropdown
        //< History Author = "Ammaar Naveed" Date = "30-01-2024" Version = "1.0" Branch = "master" > Get contact types from lookup table.</ History >
        protected async Task GetContactTypes()
        {
            var response = await userService.GetContactTypes();
            if (response.IsSuccessStatusCode)

            {
                ContactTypes = (IEnumerable<ContactType>)response.ResultData;
            }
            StateHasChanged();
        }
        #endregion

        #region DDL
        private async Task PopulateGroupTypes()
        {
            var response = await groupService.GetGroupAccessTypes();
            if (response.IsSuccessStatusCode)
            {
                groupAccessTypes = (IEnumerable<GroupTypeWebSystemVM>)response.ResultData;
                if (EmployeeType == (int)EmployeeTypeEnum.External)
                    groupAccessTypes = groupAccessTypes.Where(x => !x.WebSystemsEn.Contains("Active Directory")).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task LoadGroups()
        {
            var response = await userService.GetGroupsByGroupTypeId(EmployeeVM.GroupTypeId);
            if (response.IsSuccessStatusCode)
            {
                Groups = (List<Group>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetNationalityData()
        {
            var response = await userService.GetNationalityData();
            if (response.IsSuccessStatusCode)
            {
                Nationalities = (IEnumerable<Nationality>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetGenders()
        {
            var response = await userService.GetGenders();
            if (response.IsSuccessStatusCode)
            {
                Genders = (IEnumerable<Gender>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetCities()
        {
            var response = await userService.GetCities();
            if (response.IsSuccessStatusCode)
            {
                Cities = (IEnumerable<City>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateRole()
        {
            var response = await userService.GetRoles();
            if (response.IsSuccessStatusCode)
            {
                Roles = (List<Role>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        #region Get Users, Supervisors & Managers Data
        //< History Author = "Ammaar Naveed" Date = "07/05/2024" Version = "1.0" Branch = "master" >Managers hierarchy implemented</ History >
        //< History Author = "Ammaar Naveed" Date = "06/05/2024" Version = "1.0" Branch = "master" >Filtered managers on the basis of system role -> Vice Hos.</ History >
        //< History Author = "Ammaar Naveed" Date = "04/03/2024" Version = "1.0" Branch = "master" >Managers and Supervisors dropdown population when sector is selected.</ History >

        private async Task PopulateManagerDropdown()
        {
            if (EmployeeVM.UserEmploymentInformation.SectorTypeId != null && (EmployeeVM.UserEmploymentInformation.DesignationId == (int)DesignationEnum.HeadOfSector || EmployeeVM.UserEmploymentInformation.DesignationId == (int)DesignationEnum.Supervisor || EmployeeVM.UserEmploymentInformation.DesignationId == (int)DesignationEnum.Lawyer))
            {
                var response = await userService.GetManagersList(EmployeeVM.UserEmploymentInformation.SectorTypeId, EmployeeVM.UserEmploymentInformation.DesignationId);
                if (response.IsSuccessStatusCode)
                {
                    ManagersList = ((List<ManagersListVM>)response.ResultData);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
        }
        //< History Author = "Ammaar Naveed" Date = "04/03/2024" Version = "1.0" Branch = "master" > Filter users -> Get supervisors by sector type Id</ History >
        IEnumerable<UserDataVM> FilteredUsers { get; set; }
        IEnumerable<UserDataVM> AllUsers { get; set; }
        public void GetSupervisorsBySectorId()
        {
            if (EmployeeVM.UserEmploymentInformation.SectorTypeId > 0)
            {
                FilteredUsers = AllUsers.Where(user =>
                    SystemRoles.Supervisor.Equals(user.RoleId) ||
                    SystemRoles.ComsSupervisor.Equals(user.RoleId));

                SupervisorsList = FilteredUsers.Where(user =>
                    user.SectorTypeId == EmployeeVM.UserEmploymentInformation.SectorTypeId);
            }
            else
            {
                UsersList = Enumerable.Empty<UserDataVM>().ToList();
            }
            StateHasChanged();
        }
        public async Task GetUserData()
        {
            var response = await userService.GetUserData();
            if (response.IsSuccessStatusCode)
            {
                AllUsers = FilteredUsers = ((IEnumerable<UserDataVM>)response.ResultData);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        public async Task GetCompanies()
        {
            var response = await userService.GetCompanies();
            if (response.IsSuccessStatusCode)
            {
                Companies = (IEnumerable<Company>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        public async Task FilterOperatingSectorsOnDepartmentId()
        {
            if (EmployeeVM.UserEmploymentInformation.DepartmentId > 0)
            {
                OperatingSectorTypes = DuplicateOSectorslist.Where(x => x.DepartmentId == EmployeeVM.UserEmploymentInformation.DepartmentId);
                GradeTypeList = DuplicateGradeTypeList.Where(x => x.DepartmentId == EmployeeVM.UserEmploymentInformation.DepartmentId);
                EmployeeVM.UserEmploymentInformation.SectorTypeId = 0;
                EmployeeVM.GradeTypeId = 0;
                EmployeeVM.UserEmploymentInformation.GradeId = 0;
            }
            else
            {
                OperatingSectorTypes = Enumerable.Empty<OperatingSectorType>().ToList();
                GradeTypeList = Enumerable.Empty<GradeType>().ToList();
                EmployeeVM.UserEmploymentInformation.SectorTypeId = 0;
                EmployeeVM.GradeTypeId = 0;
                EmployeeVM.UserEmploymentInformation.GradeId = 0;
            }
            EmployeeGradeList = Enumerable.Empty<Grade>().ToList();
            StateHasChanged();
        }
        public async Task GetEmployeeDepartment()
        {
            var response = await userService.GetEmployeeDepartment();
            if (response.IsSuccessStatusCode)
            {
                Departments = (IEnumerable<Department>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetEmployeeSectortype()
        {
            var response = await userService.GetEmployeeSectortype();
            if (response.IsSuccessStatusCode)
            {
                DuplicateOSectorslist = (IEnumerable<OperatingSectorType>)response.ResultData;

                if (EmployeeVM.UserEmploymentInformation.DepartmentId > 0)
                    OperatingSectorTypes = DuplicateOSectorslist.Where(x => x.DepartmentId == EmployeeVM.UserEmploymentInformation.DepartmentId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetEmployeeGrade(bool IsChangeTrigger = false)
        {
            if (IsChangeTrigger)
                EmployeeVM.UserEmploymentInformation.GradeId = 0;
            var response = await userService.GetEmployeeGrade();
            if (response.IsSuccessStatusCode)
            {
                EmployeeGradeList = (IEnumerable<Grade>)response.ResultData;
                if (EmployeeVM.GradeTypeId != 0)
                {
                    EmployeeGradeList = EmployeeGradeList.Where(x => x.GradeTypeId == EmployeeVM.GradeTypeId).ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetGradeTypes()
        {
            var response = await userService.GetGradeTypes();
            if (response.IsSuccessStatusCode)
            {
                DuplicateGradeTypeList = (IEnumerable<GradeType>)response.ResultData;
                if (EmployeeVM.UserEmploymentInformation.DepartmentId > 0)
                {
                    GradeTypeList = DuplicateGradeTypeList.Where(x => x.DepartmentId == EmployeeVM.UserEmploymentInformation.DepartmentId);
                    if (!GradeTypeList.Any(x => x.Id == EmployeeVM.GradeTypeId))
                    {
                        EmployeeVM.GradeTypeId = 0;
                        EmployeeVM.UserEmploymentInformation.GradeId = 0;
                        EmployeeGradeList = Enumerable.Empty<Grade>().ToList();
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetContractTypes()
        {
            var response = await userService.GetContractTypes();
            if (response.IsSuccessStatusCode)
            {
                ContractTypeList = (IEnumerable<ContractType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task GetEmployeeStatus()
        {
            var response = await userService.GetEmployeeStatus();
            if (response.IsSuccessStatusCode)
            {
                employeeStatuses = (IEnumerable<EmployeeStatus>)response.ResultData;
                if (EmployeeType == (int)EmployeeTypeEnum.Internal)
                {
                    employeeStatuses = employeeStatuses.Where(x => x.Name_En != EmployeeStatusEnum.InActive.ToString()).ToList();
                }
                else
                {
                    employeeStatuses = employeeStatuses.Where(x => x.Name_En == EmployeeStatusEnum.Active.ToString() || x.Name_En == EmployeeStatusEnum.InActive.ToString())
                    .ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateWorkingTime()
        {
            var response = await userService.GetWorkingTime();
            if (response.IsSuccessStatusCode)
            {
                WorkingTimes = (List<EmployeeWorkingTime>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task GetCompanyInfo()
        {
            if (EmployeeVM.UserEmploymentInformation.CompanyId == null)
            {
                Company = new Company
                {
                    City = new City
                    {
                        Governorate = new Governorate
                        {
                            Country = new Country()
                        }
                    }
                };
            }
            else { Company = Companies.Where(x => x.CompanyId == EmployeeVM.UserEmploymentInformation.CompanyId).FirstOrDefault(); }
        }

        #endregion

        #region Add Education/Trainings/Work Experience 
        public async Task AddEducation()
        {
            if (string.IsNullOrEmpty(EduInformation.MajoringName)
            || string.IsNullOrEmpty(EduInformation.UniversityName)
            || EduInformation.GraduationYear.Equals(null))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Detail = translationState.Translate("Additional_Info_Null_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                if (!isEditEducation)
                {
                    if ((EduInformation.GraduationYear.HasValue)
                        || !string.IsNullOrEmpty(EduInformation.Percentage_Grade)
                        || !string.IsNullOrEmpty(EduInformation.Comments)
                        || !string.IsNullOrEmpty(EduInformation.MajoringName)
                        || !string.IsNullOrEmpty(EduInformation.UniversityName)
                        || !string.IsNullOrEmpty(EduInformation.UniversityCountry)
                        || !string.IsNullOrEmpty(EduInformation.UniversityCity)
                        || !string.IsNullOrEmpty(EduInformation.UniversityAddress)
                        )
                    {
                        var newEducation = new UserEducationalInformation
                        {
                            EducationId = Guid.NewGuid(),
                            GraduationYear = EduInformation.GraduationYear,
                            Percentage_Grade = EduInformation.Percentage_Grade,
                            Comments = EduInformation.Comments,
                            MajoringName = EduInformation.MajoringName,
                            UniversityName = EduInformation.UniversityName,
                            UniversityCountry = EduInformation.UniversityCountry,
                            UniversityCity = EduInformation.UniversityCity,
                            UniversityAddress = EduInformation.UniversityAddress
                        };
                        EmployeeVM.UserEducationalInformation.Add(newEducation);
                        EduInformation = new UserEducationalInformation();
                        RefreshGrid = true;
                        StateHasChanged();
                        await Task.Delay(100);
                        RefreshGrid = false;
                    }
                }
                if (isEditEducation)
                {
                    EduInformation = new UserEducationalInformation();
                    addEducationText = translationState.Translate("Add_Education");
                    RefreshGrid = true;
                    StateHasChanged();
                    isEditEducation = false;
                    RefreshGrid = false;
                }
            }

        }
        public async Task AddTrainings()
        {
            if (string.IsNullOrEmpty(UserTrainingAttended.TrainingName)
            || UserTrainingAttended.StartDate.Equals(null) && UserTrainingAttended.EndDate.Equals(null))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Detail = translationState.Translate("Additional_Info_Null_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                if (!isEditTraining)
                {
                    if (!string.IsNullOrEmpty(UserTrainingAttended.TrainingName)
                          || !string.IsNullOrEmpty(UserTrainingAttended.TrainingCenterName)
                          || (UserTrainingAttended.StartDate.HasValue && UserTrainingAttended.EndDate.HasValue)
                          || !string.IsNullOrEmpty(UserTrainingAttended.TrainingLocation)
                       )
                    {
                        var newTrainings = new UserTrainingAttended
                        {
                            TrainingId = Guid.NewGuid(),
                            TrainingName = UserTrainingAttended.TrainingName,
                            TrainingCenterName = UserTrainingAttended.TrainingCenterName,
                            StartDate = UserTrainingAttended.StartDate,
                            EndDate = UserTrainingAttended.EndDate,
                            TrainingLocation = UserTrainingAttended.TrainingLocation,
                            Percentage_Grade = UserTrainingAttended.Percentage_Grade,
                            Comments = UserTrainingAttended.Comments
                        };
                        EmployeeVM.userTrainingAttendeds.Add(newTrainings);
                        UserTrainingAttended = new UserTrainingAttended();

                        RefreshGrid = true;
                        StateHasChanged();
                        await Task.Delay(100);
                        RefreshGrid = false;
                    }
                }
                if (isEditTraining)
                {
                    UserTrainingAttended = new UserTrainingAttended();
                    addTrainingText = translationState.Translate("Add_Training");
                    RefreshGrid = true;
                    StateHasChanged();
                    isEditTraining = false;
                    RefreshGrid = false;
                }
            }

        }
        public async Task AddWorkExperience()
        {
            if (string.IsNullOrEmpty(UserWorkExperience.CompanyName)
            || string.IsNullOrEmpty(UserWorkExperience.JobTitle)
            || string.IsNullOrEmpty(UserWorkExperience.Experience))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Warning,
                    Detail = translationState.Translate("Additional_Info_Null_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                if (!isEditWorkExperience)
                {
                    if (!string.IsNullOrEmpty(UserWorkExperience.CompanyName)
                    || !string.IsNullOrEmpty(UserWorkExperience.JobTitle)
                    || !string.IsNullOrEmpty(UserWorkExperience.JobExperience)
                    || (UserWorkExperience.StartDate.HasValue && UserWorkExperience.EndDate.HasValue)
                    || !string.IsNullOrEmpty(UserWorkExperience.Experience))
                    {
                        var newWorkExperience = new UserWorkExperience
                        {
                            ExperienceId = Guid.NewGuid(),
                            CompanyName = UserWorkExperience.CompanyName,
                            JobTitle = UserWorkExperience.JobTitle,
                            JobExperience = UserWorkExperience.JobExperience,
                            StartDate = UserWorkExperience.StartDate,
                            EndDate = UserWorkExperience.EndDate
                        };
                        EmployeeVM.UserWorkExperiences.Add(newWorkExperience);
                        UserWorkExperience = new UserWorkExperience();

                        RefreshGrid = true;
                        StateHasChanged();
                        await Task.Delay(100);
                        RefreshGrid = false;
                    }
                }
                if (isEditWorkExperience)
                {
                    UserWorkExperience = new UserWorkExperience();
                    addWorkExperienceText = translationState.Translate("Add_Work_Experience");
                    RefreshGrid = true;
                    StateHasChanged();
                    isEditWorkExperience = false;
                    RefreshGrid = false;
                }
            }

        }
        #region Add New Company
        protected async Task AddNewCompany(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddCompany>(
                translationState.Translate("Add_Company"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(100);
                var resultTags = (Company)dialogResult;
                if (resultTags != null)
                {
                    await GetCompanyList();
                    CompanyDropDown?.Reset();
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task GetCompanyList()
        {
            var response = await lookupService.GetCompanyList();
            if (response.IsSuccessStatusCode)
            {
                Companies = (List<Company>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion
        #region Add new Designation
        protected async Task AddDesignationButtonClick(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddDesignation>(
                translationState.Translate("Add_Designation"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(100);
                var resultTags = (Designation)dialogResult;
                if (resultTags != null)
                {
                    await GetDesignationList();
                    DesignationDropDown.Reset();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        private async Task GetDesignationList()
        {
            var response = await lookupService.GetDesignationList();
            if (response.IsSuccessStatusCode)
            {
                DesignationList = (IEnumerable<Designation>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }

        #endregion

        #region Grid Edit Buttons For Education/Address/Work Experience/Trainings/Contacts
        protected async Task EditEducation(MouseEventArgs args, Guid EducationId)
        {
            try
            {
                var emp = EmployeeVM.UserEducationalInformation.Where(c => c.EducationId == EducationId).FirstOrDefault();
                this.EducationId = EducationId;
                if (emp != null)
                {
                    EduInformation = emp;
                    isEditEducation = true;
                    addEducationText = "Edit_Education";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task EditAddress(MouseEventArgs args, Guid AddressId)
        {
            try
            {
                var emp = EmployeeVM.UserAdresses.Where(c => c.AddressId == AddressId).FirstOrDefault();
                var index = EmployeeVM.UserAdresses.FindIndex(c => c.AddressId == AddressId);
                if (emp != null)
                {
                    isEditAddress = true;
                    addAddressText = "Edit_Address";
                    var parameters = new Dictionary<string, object>
                    {
                        { "userId", emp.UserId },
                        { "AddressId", AddressId },
                        { "Address", emp.Address },
                        { "CityId", emp.CityId},
                    };
                    var dialogResult = await dialogService.OpenAsync<AddAddress>
                      (
                          translationState.Translate(addAddressText),
                             parameters,
                          new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                      );

                    if (dialogResult != null)
                    {
                        UserAdressoBJ = (UserAdress)dialogResult;
                        UserAdressoBJ.City = Cities.Where(x => x.CityId == UserAdressoBJ.CityId).FirstOrDefault();
                        EmployeeVM.UserAdresses.RemoveAt(index);
                        EmployeeVM.UserAdresses.Insert(index, UserAdressoBJ);
                        isEditAddress = true;
                        addAddressText = "Add_Address";
                        RefreshGrid = true;
                        StateHasChanged();
                        await Task.Delay(100);
                        RefreshGrid = false;
                    }
                    else { addAddressText = "Add_Address"; }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task EditWorkExperience(MouseEventArgs args, Guid WorkExperienceId)
        {
            try
            {
                var emp = EmployeeVM.UserWorkExperiences.Where(c => c.ExperienceId == WorkExperienceId).FirstOrDefault();
                this.ExperienceId = WorkExperienceId;
                if (emp != null)
                {
                    UserWorkExperience = emp;
                    isEditWorkExperience = true;
                    addWorkExperienceText = "Edit_WorkExperience";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task EditTraining(MouseEventArgs args, Guid TrainingId)
        {
            try
            {
                var emp = EmployeeVM.userTrainingAttendeds.Where(c => c.TrainingId == TrainingId).FirstOrDefault();
                this.TrainingId = TrainingId;
                if (emp != null)
                {
                    UserTrainingAttended = emp;
                    isEditTraining = true;
                    addTrainingText = "Edit_Training";
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task EditContactInformation(MouseEventArgs args, Guid Id)
        {
            try
            {
                var employeeContact = EmployeeVM.UserContactInformationList.Where(c => c.Id == Id).FirstOrDefault();
                var index = EmployeeVM.UserContactInformationList.FindIndex(c => c.Id == Id);
                if (employeeContact != null)
                {
                    isEditContact = true;
                    var parameters = new Dictionary<string, object>
                 {
                     { "UserId", employeeContact.UserId },
                     { "Id", Id },
                     { "ContactNumber", employeeContact.ContactNumber },
                     { "ContactType", employeeContact.ContactTypeId },
                     { "IsPrimary", employeeContact.IsPrimary },
                 };
                    var dialogResult = await dialogService.OpenAsync<AddContact>
                      (
                          translationState.Translate("Edit_Contact"),
                             parameters,
                          new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                      );

                    if (dialogResult != null)
                    {
                        UserContact = (UserContactInformation)dialogResult;
                        EmployeeVM.UserContactInformationList.RemoveAt(index);
                        if (UserContact.IsPrimary == true)
                        {
                            await CheckExistingPrimaryContacts();
                        }
                        EmployeeVM.UserContactInformationList.Insert(index, UserContact);
                        isEditContact = true;
                        RefreshGrid = true;
                        StateHasChanged();
                        await Task.Delay(100);
                        RefreshGrid = false;
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Grid Delete Buttons For Education / Address / Work Experience / Training / Contacts
        private async Task DeleteTraining(MouseEventArgs args, Guid TrainingId)
        {
            if (TrainingId != Guid.Empty)
            {
                EmployeeVM.userTrainingAttendeds.RemoveAll(x => x.TrainingId == TrainingId);
                await userTrainingsGrid.Reload();
                StateHasChanged();
                Task.Delay(50).Wait();
            }
        }
        private async Task DeleteWorkExperience(MouseEventArgs args, Guid ExperienceId)
        {
            if (ExperienceId != Guid.Empty)
            {
                EmployeeVM.UserWorkExperiences.RemoveAll(x => x.ExperienceId == ExperienceId);
                await workExperienceGrid.Reload();
                StateHasChanged();
                Task.Delay(50).Wait();
            }
        }
        private async Task DeleteAddress(MouseEventArgs args, Guid AddressId)
        {
            if (AddressId != Guid.Empty)
            {
                EmployeeVM.UserAdresses.RemoveAll(x => x.AddressId == AddressId);
                await addressGrid.Reload();
                StateHasChanged();
                Task.Delay(50).Wait();
            }
        }
        private async Task DeleteEducation(MouseEventArgs args, Guid EducationId)
        {
            if (EducationId != Guid.Empty)
            {
                EmployeeVM.UserEducationalInformation.RemoveAll(x => x.EducationId == EducationId);
                await educationalInformationGrid.Reload();
                StateHasChanged();
                Task.Delay(50).Wait();
            }
        }
        private async Task DeleteContactNumberOnButtonClick(MouseEventArgs args, Guid Id)
        {
            if (Id != Guid.Empty)
            {
                EmployeeVM.UserContactInformationList.RemoveAll(x => x.Id == Id);
                await contactInformationGrid.Reload();
                StateHasChanged();
                Task.Delay(50).Wait();
            }
        }
        #endregion

        #region Deactivate User
        protected async Task OpenConfirmEmployeeDeactivationDialog()
        {
            if (EmployeeVM.UserEmploymentInformation.EmployeeStatusId != 1 || EmployeeVM.IsUserHasAnyTask == false)
            {
                await DeactivateCurrentUser();
            }
            else
            {
                bool IsUserHasAnyTask = EmployeeVM.IsUserHasAnyTask;
                bool? dialogResponse = await dialogService.Confirm(
                       IsUserHasAnyTask == true ? translationState.Translate("Task_Employee_Deactivate_Confirm_Dialog_Text") : translationState.Translate("Employee_Deactivate_Confirm_Dialog_Text"),
                        translationState.Translate("Employee_Confirm_Deactivate_Dialog_Title"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("Deactivate"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        });
                if (dialogResponse == true)
                {
                    await DeactivateCurrentUser();
                }
            }
        }
        protected async Task DeactivateCurrentUser()
        {
            string RoleId = EmployeeVM.RoleId;
            int? SectorTypeId = EmployeeVM.UserEmploymentInformation.SectorTypeId;
            var allSelectedUsers = new List<EmployeeVM>();
            var currentUser = new EmployeeVM()
            {
                UserId = EmployeeVM.UserEmploymentInformation.UserId,
                EmployeeTypeId = EmployeeVM.UserEmploymentInformation.EmployeeTypeId,
                EmployeeStatusId = EmployeeVM.UserEmploymentInformation.EmployeeStatusId
            };
            allSelectedUsers.Add(currentUser);

            var result = await dialogService.OpenAsync<DeactivateEmployees>(EmployeeVM.UserEmploymentInformation.EmployeeStatusId == 1 ? translationState.Translate("Deactivate_Employee") : translationState.Translate("Activate_Employee"),
                    new Dictionary<string, object>()
                    {
                        { "SelectedUsersForDeactivation", allSelectedUsers },
                        { "RoleId", RoleId},
                        {"SectorTypeId", SectorTypeId },
                        {"EmployeeId", Id },
                        {"EmployeeStatusId", EmployeeVM.UserEmploymentInformation.EmployeeStatusId },
                        {"IsUserHasAnyTask", IsUserHasAnyTask },
                        {"DesignationId", EmployeeVM.UserEmploymentInformation.DesignationId },
                        {"EmployeeTypeId", EmployeeVM.UserEmploymentInformation.EmployeeTypeId }
                    },
                    new DialogOptions() { Width = "30% !important" }
                );
        }
        #endregion

        #region Redirect Function
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/employee-list");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        private void CheckForPrefix()
        {
            if (EmployeeVM.userPersonalInformation.LastName_En != null)
            {

                if (!EmployeeVM.userPersonalInformation.LastName_En.Contains("-") && AutoHyphenAdd == false)
                {
                    AutoHyphenAdd = true;
                    string[] prefixes = { "al", "ibn", "bin" };
                    string matchingPrefix = prefixes.FirstOrDefault(prefix => EmployeeVM.userPersonalInformation.LastName_En.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));
                    if (!string.IsNullOrEmpty(matchingPrefix))
                    {
                        EmployeeVM.userPersonalInformation.LastName_En = EmployeeVM.userPersonalInformation.LastName_En.Insert(matchingPrefix.Length, "-");
                        ShowHyphenHint = true;
                    }
                    else
                    {
                        ShowHyphenHint = false;
                    }
                }
            }
        }
    }
}
