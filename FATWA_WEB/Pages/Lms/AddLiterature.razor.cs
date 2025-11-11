using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using Telerik.Blazor.Components;
using Telerik.DataSource;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages.Lms
{
    //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Contains Add Literature wizard functions, validations, notifications, form submissions, navigations etc</History>
    //<History Author = 'Umer Zaman' Date='2022-07-08' Version="1.0" Branch="master">Modified and change index, division & aisle methods for dropdowns, add literature number auto increment functionality also rearrange variables methods names</History>
    //<History Author = 'Nabeel ur Rehman' Date='2022-07-08' Version="1.0" Branch="master">Add Multiple Authors</History>
    public partial class AddLiterature : ComponentBase
    {
        public AddLiterature()
        {
            selectedLiteratureIndex = new LmsLiteratureIndex();
            AllLiteratureIndexeDetails = new List<LmsLiteratureIndex>();
            DivisionAisleDetailsList = new List<LmsLiteratureIndexDivisionAisle>();
            DivisionListUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
            AisleListUsingIndexAndDivision = new List<LmsLiteratureIndexDivisionAisle>();
            DivisionDetailsForAisleNumber = new List<LmsLiteratureIndexDivisionAisle>();
            lmsLiteratureAuthorForGrid = new List<LmsLiteratureAuthor>();
            GetAuthorsData = new List<LmsLiteratureAuthor>();

        }
        #region Service Injections





        #endregion

        #region variables declaration

        public Telerik.DataSource.FilterOperator ReusableFilterOperator { get; set; } = Telerik.DataSource.FilterOperator.Contains;
        public bool AddConfirmVisible { get; set; }
        public bool CancelConfirmVisible { get; set; }
        protected bool isLoading;
        public bool ShowWizard { get; set; } = true;
        public bool CreateAnother { get; set; } = false;
        public int Value { get; set; } = 0;
        public int SelectedIndexId { get; set; } = 0;
        public int SelectedDivisionId { get; set; } = 0;
        public int SelectedAisleNumber { get; set; } = 0;
        public string AuthorSubTitle { get; set; }
        public string AuthorSubTitleExsist { get; set; }
        public TelerikDropDownList<LmsLiteratureIndexDivisionAisle, int> DivisionsDDRef { get; set; }
        public TelerikDropDownList<LmsLiteratureIndexDivisionAisle, int> AislesDDRef { get; set; }
        public LmsLiteratureIndex selectedLiteratureIndex;
        public LmsLiterature Literature { get; set; } = new LmsLiterature() { EditionYear = null };
        DateTime? editionYear = null;
        public LmsLiteratureAuthor lmsLiteratureAuthor { get; set; } = new LmsLiteratureAuthor();
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1950, 1, 1);
        bool isBasicStep = false;
        bool isAuthorStep = false;
        LmsLiterature currentForm { get; set; }
        protected BasicDetailValidationClasses validations { get; set; } = new BasicDetailValidationClasses();
        protected PurchaseAuthorDetailValidationClasses purchaseAuthorValidations { get; set; } = new PurchaseAuthorDetailValidationClasses();
        protected IEnumerable<LmsLiteratureType> AllLiteratureTypes;
        protected IEnumerable<LmsLiteratureClassification> AllLiteratureClassifications;
        protected IEnumerable<LmsLiteratureIndex> AllLiteratureIndexeDetails { get; set; }
        protected IEnumerable<LmsLiteratureIndexDivisionAisle> DivisionAisleDetailsList { get; set; }
        protected List<LmsLiteratureIndexDivisionAisle> DivisionListUsingIndexId { get; set; }
        protected List<LmsLiteratureIndexDivisionAisle> DivisionDetailsForAisleNumber { get; set; }
        protected IEnumerable<LmsLiteratureIndexDivisionAisle> AisleListUsingIndexAndDivision { get; set; }
        //public LmsLiteratureAuthor lmsLiteratureAuthor { get; set; }
        public LmsLiteratureDetailsLmsLiteratureAuthor lmsLiteratureDetailsLmsLiteratureAuthor { get; set; }
        public List<LmsLiteratureAuthor> lmsLiteratureAuthorForGrid { get; set; } = new List<LmsLiteratureAuthor>();
        public bool ShowAuthorGrid;
        public RadzenDataGrid<LmsLiteratureAuthor>? AuthorGridRef = new RadzenDataGrid<LmsLiteratureAuthor>();
        protected EditContext MyEditContext { get; set; }
        protected string newAuthor { get; set; } = "Existing";
        protected string filePath1 = "\\images\\dual_icon.png";
        protected string filePathHover1 = "\\images\\lmsLiteratureDetail-1.png";

        protected string filePath2 = "\\images\\lmsLiteratureDetail-2.png";
        protected string filePathHover2 = "\\images\\lmsLiteratureDetail-2.png";

        protected string filePath3 = "\\images\\lmsLiteratureDetail-3.png";
        protected string filePathHover3 = "\\images\\lmsLiteratureDetail-3.png";

        protected string filePath4 = "\\images\\lmsLiteratureDetail-4.png";
        protected string filePathHover4 = "\\images\\lmsLiteratureDetail-4.png";

        protected string filePath5 = "\\images\\lmsLiteratureDetail-5.png";
        protected string filePathHover5 = "\\images\\lmsLiteratureDetail-5.png";

        protected string basePath1 = string.Empty;


        public List<AuthorTypeModel> AuthorTypes = new List<AuthorTypeModel>();


        public class AuthorTypeModel
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
        protected LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivisionAisleDetails { get; set; } = null;
        protected RadzenDataGrid<LiteratureDetailLiteratureTagVM> TagsGrid;
        protected RadzenDataGrid<LmsLiteratureBarcode> BarcodesGrid;
        protected List<LiteratureTag> ActiveLiteratureTags { get; set; } = new List<LiteratureTag>();
        protected LiteratureDetailLiteratureTagVM tagToInsert;
        public bool IsPrintDialogVisible { get; set; } = false;
        protected PrintCommandEnum PrintCommand { get; set; }
        protected string PrintValue { get; set; }
        List<int> yearsWithEvents { get; set; } = new List<int>() { 2020, 2021 };
        protected LmsLiteratureBarcode lmsLiteratureBarcode { get; set; } = new LmsLiteratureBarcode();
        protected bool IsRFIdMatch = false;
        protected bool IsDisabled = false;
        protected IEnumerable<LmsLiteratureAuthor> GetAuthorsData { get; set; }
        #endregion

        #region Initialization
        protected override async Task OnInitializedAsync()
        {
            basePath1 = Path.Combine(Directory.GetCurrentDirectory() + filePath1);

            spinnerService.Show();
            AuthorTypes.Add(new AuthorTypeModel() { Text = translationState.Translate("Existing"), Value = "Existing" });
            AuthorTypes.Add(new AuthorTypeModel() { Text = translationState.Translate("Add_New"), Value = "New" });
            currentForm = new LmsLiterature();
            MyEditContext = new EditContext(currentForm);
            Literature.Purchase_Date = DateTime.Now;
            Literature.LiteratureAttachements = new();
            Literature.IsSeries = true;
            Literature.IsViewable = true;
            await GetLiteratureIndexDetails();
            await GetRemoteAuthorsData();
            //await GetLiteratureIndexDivisionAisleDetails();
            Literature.Number = await lmsLiteratureService.GetNewLmsLiteratureNumber();

            var tagsResponse = await lmsLiteratureService.GetAllActiveLiteratureTags();
            if (tagsResponse.IsSuccessStatusCode)
            {
                ActiveLiteratureTags = (List<LiteratureTag>)tagsResponse.ResultData;
            }
            spinnerService.Hide();
        }
        #endregion

        #region Virtual Dropdown (Index, Type, Classification, Divison, Aisle, Author) Functions, Populate Dependent dropdowns, fields and change events
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Function for getting Index records for the drop down dynamically from the API</History>
        //<History Author = 'Umer Zaman' Date='2022-07-16' Version="1.0" Branch="master">Change and modified method to get index details from index table</History>
        private async Task<IEnumerable<LmsLiteratureIndex>> GetLiteratureIndexDetails()
        {
            AllLiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
            return AllLiteratureIndexeDetails;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">set index name, divison, aisle on index select</History>
        //<History Author = 'Umer Zaman' Date='2022-07-16' Version="1.0" Branch="master">Change method name and get selected index detail from index table by using selected index id</History>
        protected async void OnChangeIndexNumberDropDown()
        {
            if (SelectedIndexId > 0)
            {
                selectedLiteratureIndex = await lmsLiteratureIndexService.GetLiteratureIndexDetailByUsingIndexId(SelectedIndexId);
                Literature.LmsLiteratureIndex = selectedLiteratureIndex;
                DivisionDetailsForAisleNumber = DivisionAisleDetailsList.OrderByDescending(u => u.IndexId).Where(x => x.IndexId == SelectedIndexId).ToList();
                var DistinctDivisions = DivisionDetailsForAisleNumber.Select(c => c.DivisionNumber).Distinct().ToList();
                if (DivisionListUsingIndexId.Count() != 0)
                {
                    DivisionListUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
                }
                foreach (var uniqueName in DistinctDivisions)
                {
                    var item = DivisionDetailsForAisleNumber.Where(x => x.DivisionNumber == uniqueName).FirstOrDefault();
                    DivisionListUsingIndexId.Add(item);
                }
                //SelectedDivisionId = 0;
                //DivisionsDDRef.Rebind();
                //SelectedAisleNumber = 0;
                //AislesDDRef.Rebind();
                StateHasChanged();
            }
            else
            {
                selectedLiteratureIndex = new LmsLiteratureIndex();
                DivisionListUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
                AisleListUsingIndexAndDivision = new List<LmsLiteratureIndexDivisionAisle>();
            }
        }


        //<History Author = 'Hassan Abbas' Date='2022-04-01' Version="1.0" Branch="master"> Function for getting Author records for the drop down dynamically from the API</History>
        protected async Task GetRemoteAuthorsData()
        {
            try
            {
                var response = await lmsLiteratureService.GetAuthorItems();
                if (response.IsSuccessStatusCode)
                {
                    GetAuthorsData = (IEnumerable<LmsLiteratureAuthor>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Custom Dropdown filter requests


        DataSourceRequest GenerateCustomFilterRequest(DataSourceRequest oldRequest)
        {
            var filter = (Telerik.DataSource.FilterDescriptor)oldRequest.Filters.FirstOrDefault();
            string searchString = "";

            if (filter != null)
            {
                // extract the search string for the custom FilterDescriptor
                searchString = filter.Value.ToString();
            }
            else
            {
                // no search string, no need to create a FilterDescriptor
                return oldRequest;
            }

            var newRequest = new DataSourceRequest()
            {
                // PageSize and Skip are needed for virtual scrolling
                //PageSize = oldRequest.PageSize,
                //Skip = oldRequest.Skip,
                Filters = new List<IFilterDescriptor>()
            };

            newRequest.Filters.Add(new CompositeFilterDescriptor()
            {
                LogicalOperator = FilterCompositionLogicalOperator.Or,
                FilterDescriptors = new FilterDescriptorCollection() {
                new Telerik.DataSource.FilterDescriptor() {
                    Member = nameof(LmsLiteratureAuthor.FullName_En),
                    Operator = ReusableFilterOperator,
                    Value = searchString
                },
                new Telerik.DataSource.FilterDescriptor() {
                    Member = nameof(LmsLiteratureAuthor.Address_En),
                    Operator = ReusableFilterOperator,
                    Value = searchString
                }
            }
            });

            return newRequest;
        }
        #endregion

        #region Wizard Events: StepChanges, Finish, Validations, Backtoform, BacktoList


        protected void OnWizardFinish()
        {
            ShowWizard = false;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Invoke Below OnBasicDetailsStepChange function on Custom Next button click</History>
        protected async Task OnBasicDetailsStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            await OnBasicDetailsStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Check validations on Basic details step change</History>
        protected async Task OnBasicDetailsStepChange(WizardStepChangeEventArgs args)
        {
            isBasicStep = true;
            Literature.EditionYear = editionYear;
            bool valid = ValidateBasicDetails();
            if (valid)
            {
                Value += 1;
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto;",
                    Duration = 900
                });
                args.IsCancelled = true;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Invoke Below OnPurchaseAuthorStepChange function on Custom Next button click</History>
        protected async Task<bool> OnPurchaseAuthorStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            var result = await OnPurchaseAuthorStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
            return result;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Check validations on Purchase And Author details step change</History>
        protected async Task<bool> OnPurchaseAuthorStepChange(WizardStepChangeEventArgs args)
        {
            isAuthorStep = true;
            bool valid = ValidatePurchaseAuthorDetails();
            bool validPublisher = ValidatePublisher();
            if (valid)
            {
                if (validPublisher)
                {
                    spinnerService.Show();
                    if (!string.IsNullOrEmpty(Literature.Name) && !string.IsNullOrEmpty(Literature.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault()) && !string.IsNullOrEmpty(selectedLiteratureIndex.IndexNumber) && Literature.Number > 0)
                    {
                        Literature.Characters = Literature.Name.Substring(0, 2).ToUpper() + "." + Literature.LmsLiteratureAuthors.Select(x => x.FullName_En).FirstOrDefault().Substring(0, 2).ToUpper() + "/" + selectedLiteratureIndex.IndexNumber.ToString() + "." + Literature.Number.ToString();
                        Literature.Characters82 = selectedLiteratureIndex.IndexNumber.ToString() + "." + Literature.Number.ToString();
                    }
                    await GenerateBarCodeNumber();
                    
                    int fromSaveCount = args.TargetIndex - 1;
                    spinnerService.Hide();
                    if (fromSaveCount > 0)
                    {
                        StateHasChanged();
                        Value += 1;
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {

                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Add_Author_Details_And_Required_Field"),
                        //Summary = $"???!",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    args.IsCancelled = true;
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {

                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Add_Author_Details_And_Required_Field"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                args.IsCancelled = true;
            }
            if (valid && validPublisher)
            {
                return true;
            }
            return false;
        }

        private async Task GenerateBarCodeNumber()
        {
            if (!Literature.LiteratureBarcodes.Any() || Literature.LiteratureBarcodes.Count() != Literature.CopyCount)
            {
                var TotalBarcodeNumber = Literature.SeriesNumber * Literature.CopyCount;
                var apiCallResponse = await lmsLiteratureService.GenerateListofLiteratureBarcode(TotalBarcodeNumber);
                if (apiCallResponse.IsSuccessStatusCode)
                {
                    Literature.LiteratureBarcodes = (List<LmsLiteratureBarcode>)apiCallResponse.ResultData;
                    await Task.Delay(50);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(apiCallResponse);
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Invoke Barcode step change</History>
        public void OnStickerBarcodeStepChange(WizardStepChangeEventArgs args)
        {
            //Value += 1;
            if (IsRFIdMatch != true)
            {
                Value += 1;
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("RFID_Already_Exist"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (args != null)
                    args.IsCancelled = true;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-20' Version="1.0" Branch="master">Show Add Book confirmation Dialog</History>
        protected async Task SubmitLiterature()
        {
            AddConfirmVisible = true;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Add another Book after submission of book</History>
        protected async Task BackToForm()
        {
            //Literature.Guid = Guid.NewGuid();
            //ShowWizard = true; 
            await JSRuntime.InvokeVoidAsync("window.location.reload");
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">redirect to literature list after submission of book</History>
        protected async Task BackToList()
        {
            //ShowWizard = false; 
            navigationManager.NavigateTo("lmsliterature-list");
        }
        #endregion


        #region Add Book Dialog
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>
        protected void VisibleChangedHandlerAdd(bool currVisible)
        {
            AddConfirmVisible = currVisible;
        }
        //<History Author = 'Nabeel ur Rehman' Date='2022-03-20' Version="1.0" Branch="master">Save As Draft Literature on Comfirmation og Dialog</History>
        protected async Task OkaySaveAsDraftBookConfirm(int currentIndex)
        {
            try
            {
                bool isValid = false;
                bool isValidPublisher = false;
                if (currentIndex == 0)
                {
                    Literature.EditionYear = editionYear;
                    isValid = ValidateBasicDetails();
                }
                else if (currentIndex == 1)
                {
                    isValid = await OnPurchaseAuthorStepChangeCustom(0);
                }
                else if (currentIndex == 2 || currentIndex == 3 || currentIndex == 4)
                {
                    isValid = true;
                }    
                if (isValid)
                {
                    if (await dialogService.Confirm(translationState.Translate("Sure_Save_Draft"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        if (newAuthor != "Existing")
                        {
                            Literature.Author_Id = 0;
                        }
                        if (SelectedIndexId != 0)
                        {
                            Literature.IndexId = SelectedIndexId;
                        }
                        if (lmsLiteratureIndexDivisionAisleDetails != null)
                        {
                            Literature.DivisionAisleId = lmsLiteratureIndexDivisionAisleDetails.DivisionAisleId;
                        }
                        spinnerService.Show();
                        Literature.IsBorrowable = false;
                        Literature.IsDraft = true;
                        var response = await lmsLiteratureService.CreateLmsLiterature(Literature);
                        if (response.IsSuccessStatusCode)
                        {
                            Literature = (LmsLiterature)response.ResultData;
                            // Save Temp Attachement To Uploaded Document
                            await SaveTempAttachementToUploadedDocument();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("The_book_has_been_successfully_added_to_the_system"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            if (CreateAnother)
                            {
                                AddConfirmVisible = false;
                                //Literature.Guid = Guid.NewGuid();
                                BackToForm();
                            }
                            else
                            {
                                BackToList();
                            }
                            StateHasChanged();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();

                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        //Summary = $"???!",
                        Style = "position: fixed !important; left: 0; margin: auto;",
                        Duration = 900
                    });
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-20' Version="1.0" Branch="master">Submit Literature on Comfirmation og Dialog</History>
        protected async Task OkayAddBookConfirm()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    if (newAuthor != "Existing")
                    {
                        Literature.Author_Id = 0;
                    }
                    if (SelectedIndexId != 0)
                    {
                        Literature.IndexId = SelectedIndexId;
                    }
                    if (lmsLiteratureIndexDivisionAisleDetails != null)
                    {
                        Literature.DivisionAisleId = lmsLiteratureIndexDivisionAisleDetails.DivisionAisleId;
                    }
                    spinnerService.Show();
                    Literature.IsDraft = false;

                    if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.LMSAdmin)) // role id getting to check the admin
                    {
                        Literature.RoleId = SystemRoles.LMSAdmin;
                    }
                    else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin)) // role id getting to check the admin
                    {
                        Literature.RoleId = SystemRoles.FatwaAdmin;
                    }
                    else // if there is no admin role except LMSAdmin and LMSAdmin then role id will be null
                         // and LmsLiteraturesController in (CreateLmsLiterature method) using there this role Id.
                    {
                        Literature.RoleId = null;
                    }
                    var response = await lmsLiteratureService.CreateLmsLiterature(Literature);
                    if (response.IsSuccessStatusCode)
                    {
                        Literature = (LmsLiterature)response.ResultData;
                        // Save Temp Attachement To Uploaded Document
                        await SaveTempAttachementToUploadedDocument();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("The_book_has_been_successfully_added_to_the_system"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (CreateAnother)
                        {
                            AddConfirmVisible = false;
                            //Literature.Guid = Guid.NewGuid();
                            BackToForm();
                        }
                        else
                        {
                            BackToList();
                        }
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-20' Version="1.0" Branch="master">Close Add Book Confirm Dialog on Click of Cancel Button</History>
        protected void CloseAddBookConfirm()
        {
            AddConfirmVisible = false;
        }

        #endregion

        #region Cancel Adding Book Dialog
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Show Confirm Dialog on cancel adding book details</History>
        protected void CancelAddingBook()
        {
            CancelConfirmVisible = true;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>
        protected void VisibleChangedHandlerCancel(bool currVisible)
        {
            CancelConfirmVisible = currVisible;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">redirect to literature list on Cancel Dialog Confirmation</History>
        protected async Task OkayCancelBookConfirm()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo("lmsliterature-list");
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Close cancel adding book details Confirm Dialog</History>
        protected void CloseCancelBookConfirm()
        {
            CancelConfirmVisible = false;
        }
        #endregion

        #region Validation Events and Classes

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Validation class for first step</History>
        protected class BasicDetailValidationClasses
        {
            public string IndexNumber { get; set; } = string.Empty;
            public string DivisionNo { get; set; } = string.Empty;
            public string AisleNumber { get; set; } = string.Empty;
            public string Type { get; set; } = string.Empty;
            public string Classification { get; set; } = string.Empty;
            public string Name_Ar { get; set; } = string.Empty;
            public string Subject_Ar { get; set; } = string.Empty;
            public string Name_En { get; set; } = string.Empty;
            public string EditionYear { get; set; } = string.Empty;
            public string ISBN { get; set; } = string.Empty;
            public string Characters { get; set; } = string.Empty;
            public string CopyCount { get; set; } = string.Empty;
            public string NoOfPages { get; set; } = string.Empty;
            public string IsSeries { get; set; } = string.Empty;
            public string SeriesNumber { get; set; } = string.Empty;
            public string EditionNumber { get; set; } = string.Empty;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Validation class for second step</History>
        protected class PurchaseAuthorDetailValidationClasses
        {
            public string AuthorId { get; set; } = string.Empty;
            public string Full_Name_En { get; set; } = string.Empty;
            public string Full_Name_Ar { get; set; } = string.Empty;
            public string First_Name_Ar { get; set; } = string.Empty;
            public string First_Name_En { get; set; } = string.Empty;
            public string Second_Name_Ar { get; set; } = string.Empty;
            public string Second_Name_En { get; set; } = string.Empty;
            public string Third_Name_Ar { get; set; } = string.Empty;
            public string Third_Name_En { get; set; } = string.Empty;
            public string Address_Ar { get; set; } = string.Empty;
            public string Address_En { get; set; } = string.Empty;
            public string Location { get; set; } = string.Empty;
            public string Price { get; set; } = string.Empty;
            public string Date { get; set; } = string.Empty;
            public string Publisher { get; set; } = string.Empty;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating basic details</History>

        protected void ValidateBasicDetailsOnChange()
        {

            if (isBasicStep)
            {
                ValidateBasicDetails();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating basic details</History>

        protected void ValidateAuthorDetailsOnChange()
        {
            if (isAuthorStep)
            {
                ValidatePurchaseAuthorDetails();
            }
        }
        protected void ValidatePublisherOnChange()
        {
            if (isAuthorStep)
            {
                ValidatePublisher();
            }
        }

        protected bool ValidateBasicDetails()
        {
            bool basicDetailsValid = true;
            if (SelectedIndexId <= 0)
            {
                validations.IndexNumber = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.IndexNumber = "k-valid";
            }
            if (string.IsNullOrWhiteSpace(Literature.EditionNumber))
            {
                validations.EditionNumber = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.EditionNumber = "k-valid";
            }
            if (string.IsNullOrWhiteSpace(Literature.Name) || (Literature.Name.Length < 2))
            {
                validations.Name_En = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.Name_En = "k-valid";
            }
            if (Literature.EditionYear == null)
            {
                validations.EditionYear = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.EditionYear = "k-valid";
            }
            if (Literature.CopyCount <= 0)
            {
                validations.CopyCount = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.CopyCount = "k-valid";
            }
            if (Literature.NumberOfPages <= 0)
            {
                validations.NoOfPages = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.NoOfPages = "k-valid";
            }
            if (Literature.IsSeries == true && Literature.SeriesNumber <= 0)
            {
                validations.SeriesNumber = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.SeriesNumber = "k-valid";
            }
            return basicDetailsValid;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating purchase and author details</History>
        protected bool ValidatePurchaseAuthorDetails()
        {
            bool purAuthDetailsValid = true;
            if (Literature.LmsLiteratureAuthors.Count() == 0)
            {
                purchaseAuthorValidations.Full_Name_En = "k-invalid";
                purchaseAuthorValidations.Address_En = "k-invalid";
                purAuthDetailsValid = false;
            }
            else
            {

                purchaseAuthorValidations.Full_Name_En = "k-valid";
                purchaseAuthorValidations.Address_En = "k-valid";
            }
            return purAuthDetailsValid;
        }
        protected bool ValidatePublisher()
        {
            bool publisher = true;
            if (string.IsNullOrWhiteSpace(Literature.Publisher))
            {
                purchaseAuthorValidations.Publisher = "k-invalid";
                publisher = false;
            }
            else
            {
                purchaseAuthorValidations.Publisher = "k-valid";
            }
            return publisher;
        }
        #endregion

        #region Print Events

        protected void Print(PrintCommandEnum printCommand, string? value)
        {
            PrintCommand = printCommand;
            PrintValue = value != null ? value : "";
            IsPrintDialogVisible = true;
        }

        protected void PrintSticker()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("pprnm", Literature.Characters.Length * 40, 100);
                pd.PrintPage += new PrintPageEventHandler(PrintStickerPage);
                pd.Print();
                pd.EndPrint += (o, e) =>
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("The_barcode_has_been_printed_successfully"),
                        //Summary = $"????!",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                };
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("The_barcode_has_been_printed_successfully"),
                    //Summary = $"???!", 
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        private void PrintStickerPage(object sender, PrintPageEventArgs ev)
        {
            ev.HasMorePages = false;
            ev.Graphics.DrawString(Literature.Characters, new Font("Microsoft Sans Serif", (float)26.75, FontStyle.Bold, GraphicsUnit.Point), Brushes.Black, 0, 0);
        }

        protected void PrintSticker82()
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("pprnm", Literature.Characters.Length * 40, 100);
                pd.PrintPage += new PrintPageEventHandler(PrintStickerPage82);
                pd.Print();
                pd.EndPrint += (o, e) =>
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("The_barcode_has_been_printed_successfully"),
                        //Summary = $"????!",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                };
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("The_barcode_has_been_printed_successfully"),
                    //Summary = $"???!", 
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        private void PrintStickerPage82(object sender, PrintPageEventArgs ev)
        {
            ev.HasMorePages = false;
            ev.Graphics.DrawString(Literature.Characters82, new Font("Microsoft Sans Serif", (float)26.75, FontStyle.Bold, GraphicsUnit.Point), Brushes.Black, 0, 0);
        }

        protected void PrintBarcode(string barCodeNumber)
        {
            try
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PaperSize = new PaperSize("pprnm", Literature.BarCodeNumber.Length * 40, 100);
                pd.PrintPage += (sender, args) => PrintBarcodePage(barCodeNumber, args);
                pd.Print();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Poster_printed_successfully"),
                    //Summary = $"????!", 
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    //Summary = $"???!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        private void PrintBarcodePage(object sender, PrintPageEventArgs ev)
        {
            Bitmap bitMap = new Bitmap(Literature.BarCodeNumber.Length * 40, 80);

            using (Graphics graphics = Graphics.FromImage(bitMap))
            {
                Font oFont = new Font("IDAutomationHC39M Free Version", 16);
                PointF point = new PointF(2f, 2f);
                SolidBrush blackBrush = new SolidBrush(Color.Black);
                SolidBrush whiteBrush = new SolidBrush(Color.White);
                graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                graphics.DrawString("*" + (string)sender + "*", oFont, blackBrush, point);
            }
            using (MemoryStream ms = new MemoryStream())
            {
                bitMap.Save(ms, ImageFormat.Png);
            }

            ev.HasMorePages = false;
            ev.Graphics.DrawImage(bitMap, 0, 0, Literature.BarCodeNumber.Length * 40, 80);
        }

        #endregion

        #region Add Author button dropdown to Grid click
        protected async Task AddAuthorDetailToGrid()
        {
            try
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    if (!string.IsNullOrWhiteSpace(lmsLiteratureAuthor.FullName_En) && !string.IsNullOrWhiteSpace(lmsLiteratureAuthor.Address_En))
                    {

                        ShowAuthorGrid = true;
                        var gg = new LmsLiteratureAuthor()
                        {

                            AuthorId = lmsLiteratureAuthor.AuthorId,
                            FullName_En = lmsLiteratureAuthor.FullName_En,
                            Address_En = lmsLiteratureAuthor.Address_En,
                        };
                        Literature.LmsLiteratureAuthors.Add(gg);

                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Required_Fields"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (lmsLiteratureAuthorForGrid.Count() == 0)
                        {
                            ShowAuthorGrid = false;
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(lmsLiteratureAuthor.FullName_En) && !string.IsNullOrWhiteSpace(lmsLiteratureAuthor.Address_En))
                    {

                        ShowAuthorGrid = true;
                        var gg = new LmsLiteratureAuthor()
                        {
                            AuthorId = lmsLiteratureAuthor.AuthorId,
                            FullName_Ar = lmsLiteratureAuthor.FullName_En,
                            Address_Ar = lmsLiteratureAuthor.Address_En,
                        };
                        Literature.LmsLiteratureAuthors.Add(gg);

                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Required_Fields"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (lmsLiteratureAuthorForGrid.Count() == 0)
                        {
                            ShowAuthorGrid = false;
                        }
                    }
                }
                lmsLiteratureAuthor = new LmsLiteratureAuthor();
                AuthorGridRef.Reset();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (lmsLiteratureAuthorForGrid.Count() == 0)
                {
                    ShowAuthorGrid = false;
                }
            }

        }
        #endregion

        #region Add Author button click
        protected async Task AddAuthorDropdownDetailToGrid()
        {
            try
            {
                if (Literature.Author_Id > 0)
                {
                    if (Literature.LmsLiteratureAuthors.Where(x => x.AuthorId == Literature.Author_Id).Any())
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Record_Already_Exists"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        var lmsAuthor = await lmsLiteratureService.GetLmsLiteratureAuthorById(Literature.Author_Id);
                        Literature.LmsLiteratureAuthors.Add(lmsAuthor);
                        Literature.Author_Id = 0;
                        StateHasChanged();
                        await AuthorGridRef.Reload();
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    if (lmsLiteratureAuthorForGrid.Count() == 0)
                    {
                        ShowAuthorGrid = false;
                    }
                }
                lmsLiteratureAuthor = new LmsLiteratureAuthor();
                AuthorGridRef.Reset();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (lmsLiteratureAuthorForGrid.Count() == 0)
                {
                    ShowAuthorGrid = false;
                }
            }

        }

        #endregion

        //#region Author grid operations
        protected async Task UpdateAuthorHandler(LmsLiteratureAuthor args) //step 3.  grid row record click for update
        {
            var resultObj = args;
            if (resultObj != null)
            {
                lmsLiteratureAuthor.AuthorId = resultObj.AuthorId;
                lmsLiteratureAuthor.FullName_En = resultObj.FullName_En;
                lmsLiteratureAuthor.Address_En = resultObj.Address_En;
                lmsLiteratureAuthorForGrid.Remove(resultObj);
                await Task.Delay(1000);
                await AuthorGridRef.Reload();
            }
        }
        protected async Task DeleteAuthorHandler(LmsLiteratureAuthor args)
        {
            var DeleteObj = args;
            bool? dialogResponse = await dialogService.Confirm(
                translationState.Translate("Edit_Grid_Article_Confirm_Message"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });
            if (dialogResponse == true)
            {
                // remove from grid list
                Literature.LmsLiteratureAuthors.Remove(DeleteObj);
                await Task.Delay(200);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Author_Delete_Success_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (Literature.LmsLiteratureAuthors.Count() == 0)
                {
                    ShowAuthorGrid = false;
                }
                await Task.Delay(1000);
                await AuthorGridRef.Reload();
            }
        }
        //#endregion
        public async Task OnAuthorRadioChangeHandler(object newValue)
        {
            if (newValue.ToString() == "Existing")
            {
                var lmsAuthor = await lmsLiteratureService.GetLmsLiteratureAuthorById(Literature.Author_Id);
                Literature.Author_FirstName_Ar = lmsAuthor.FirstName_Ar;
                Literature.Author_FullName_En = lmsAuthor.FullName_En;
                Literature.Author_FullName_Ar = lmsAuthor.FullName_Ar;
                Literature.Author_FirstName_En = lmsAuthor.FirstName_En;
                Literature.Author_SecondName_Ar = lmsAuthor.SecondName_Ar;
                Literature.Author_SecondName_En = lmsAuthor.SecondName_En;
                Literature.Author_ThirdName_Ar = lmsAuthor.ThirdName_Ar;
                Literature.Author_ThirdName_En = lmsAuthor.ThirdName_En;
                Literature.Author_Address_Ar = lmsAuthor.Address_Ar;
                Literature.Author_Address_En = lmsAuthor.Address_En;
            }
            else
            {

                Literature.Author_FullName_En = string.Empty;
                Literature.Author_FullName_Ar = string.Empty;
                Literature.Author_FirstName_Ar = string.Empty;
                Literature.Author_FirstName_En = string.Empty;
                Literature.Author_SecondName_Ar = string.Empty;
                Literature.Author_SecondName_En = string.Empty;
                Literature.Author_ThirdName_Ar = string.Empty;
                Literature.Author_ThirdName_En = string.Empty;
                Literature.Author_Address_Ar = string.Empty;
                Literature.Author_Address_En = string.Empty;
            }
        }

        #region Literature Tags
        protected async Task InsertRow()
        {
            try
            {
                tagToInsert = new LiteratureDetailLiteratureTagVM { OpenForEdit = true };
                await TagsGrid.InsertRow(tagToInsert);
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task OnCreateRow(LiteratureDetailLiteratureTagVM tag)
        {
            tag.TagNo = ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().TagNo;
            tag.Description = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().Description : ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().Description_Ar;
            tag.OpenForEdit = false;
            tag.Value = tag.TempValue;
            Literature.LiteratureTags.Add(tag);
        }
        protected async void OnUpdateRow(LiteratureDetailLiteratureTagVM tag)
        {
            if (tag == tagToInsert)
            {
                tagToInsert = null;
            }

            Literature.LiteratureTags.Remove(tag);
            tag.TagNo = ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().TagNo;
            tag.Description = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().Description : ActiveLiteratureTags.Where(x => x.Id == tag.TagId).FirstOrDefault().Description_Ar;
            tag.OpenForEdit = false;
            tag.Value = tag.TempValue;
            Literature.LiteratureTags.Add(tag);
        }

        protected async Task EditRow(LiteratureDetailLiteratureTagVM tag)
        {
            tagToInsert = tag;
            tag.OpenForEdit = true;
            await TagsGrid.EditRow(tag);
        }

        protected async Task SaveRow(LiteratureDetailLiteratureTagVM tag)
        {
            if (tag == tagToInsert)
            {
                tagToInsert = null;
            }
            await TagsGrid.UpdateRow(tag);
        }

        protected void CancelEdit(LiteratureDetailLiteratureTagVM tag)
        {
            if (tag == tagToInsert)
            {
                tagToInsert = null;
            }
            if (tag.TagNo != null)
            {
                tag.TagId = ActiveLiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Id;
                tag.TempValue = Literature.LiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Value;
                tag.Description = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?  ActiveLiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Description : ActiveLiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Description_Ar;
            }
            tag.OpenForEdit = false;
            TagsGrid.CancelEditRow(tag);
        }

        protected async Task DeleteRow(LiteratureDetailLiteratureTagVM tag)
        {
            if (tag == tagToInsert || tagToInsert != null)
            {
                tagToInsert = null;
            }

            if (Literature.LiteratureTags.Contains(tag))
            {
                Literature.LiteratureTags.Remove(tag);
                await TagsGrid.Reload();
            }
            else
            {
                TagsGrid.CancelEditRow(tag);
            }

        }
        #endregion

        #region Barcodes Grid

        protected async Task ToggleActiveBarcode(LmsLiteratureBarcode barcode)
        {
            if (Literature.LiteratureBarcodes.Where(x => x.BarCodeNumber == barcode.BarCodeNumber).FirstOrDefault().Active)
            {
                Literature.LiteratureBarcodes.Where(x => x.BarCodeNumber == barcode.BarCodeNumber).FirstOrDefault().Active = false;
            }
            else
            {
                Literature.LiteratureBarcodes.Where(x => x.BarCodeNumber == barcode.BarCodeNumber).FirstOrDefault().Active = true;
            }
            await BarcodesGrid.Reload();
        }

        protected async Task DeleteBarcode(LmsLiteratureBarcode barcode)
        {
            Literature.LiteratureBarcodes.Remove(Literature.LiteratureBarcodes.Where(x => x.BarCodeNumber == barcode.BarCodeNumber).FirstOrDefault());
            await BarcodesGrid.Reload();
        }

        #endregion


        protected async Task EditOptionRow(LmsLiteratureBarcode option)
        {
            IsDisabled = true;
            await BarcodesGrid.EditRow(option);

        }
        protected async Task SaveRow(LmsLiteratureBarcode oneditRFID)
        {

            if (oneditRFID.RFID != null)
            {
                var response = await lmsLiteratureService.CheckRFIDValueExists(oneditRFID.BarCodeNumber, Convert.ToString(oneditRFID.RFID));
                if (response.IsSuccessStatusCode)
                {

                    var result = (bool)response.ResultData;
                    if (result)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("RFID_Already_Exist"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        IsRFIdMatch = true;
                    }
                    else
                    {
                        IsRFIdMatch = false;
                        if (!Literature.LiteratureBarcodes.Where(x => x.RFIDValue == Convert.ToString(oneditRFID.RFID) && x.BarCodeNumber != oneditRFID.BarCodeNumber).Any())
                        {
                            oneditRFID.RFIDValue = Convert.ToString(oneditRFID.RFID);
                            await BarcodesGrid.UpdateRow(oneditRFID);
                            IsDisabled = false;
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("RFID_Already_Exist"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            IsRFIdMatch = true;
                        }
                    }

                }
                else
                {
                    await BarcodesGrid.UpdateRow(oneditRFID);
                    IsDisabled = false;
                }
            }
            else
            {
                oneditRFID.RFIDValue = Convert.ToString(oneditRFID.RFID);
                await BarcodesGrid.UpdateRow(oneditRFID);
                IsDisabled = false;
                IsRFIdMatch = false;
            }
        }
        protected void OnUpdateRow(LmsLiteratureBarcode barcode)
        {

        }

        #region Redirect Function
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
        //<History Author = 'Ijaz Ahmad' Date='2023-04-2' Version="1.0" Branch="master">Save Temp Attachement To Uploaded Document</History>
        #region Save Temp Attachement To Uploaded Document
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                // Get a list of IDs for the Legal Principe  itself.
                List<Guid> requestIds = new List<Guid>();
                requestIds.Add((Guid)Literature.Guid);
                // Save Temp Attachement To Uploaded Documents              
                var docResponse = await fileUploadService.SaveLiteratureTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = requestIds,
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = Literature.DeletedAttachementIds,
                    LiteratureId = 0,
                    LiteratureIds = Literature.LiteratureIdList
                });

                // If the file upload service returns an error, show a notification message
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        #endregion 
    }
}
