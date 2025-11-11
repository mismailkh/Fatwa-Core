using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.WorkflowModels;
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
    public partial class EditLiterature : ComponentBase
    {
        public EditLiterature()
        {
            selectedLiteratureIndex = new LmsLiteratureIndex();
            AllLiteratureIndexeDetails = new List<LmsLiteratureIndex>();
            DivisionAisleDetailsList = new List<LmsLiteratureIndexDivisionAisle>();
            DivisionListUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
            AisleListUsingIndexAndDivision = new List<LmsLiteratureIndexDivisionAisle>();
            DivisionDetailsForAisleNumber = new List<LmsLiteratureIndexDivisionAisle>();
            lmsLiteratureAuthorForGrid = new List<LmsLiteratureAuthor>();
            ShowAuthorGrid = true;
            GetAuthorsData = new List<LmsLiteratureAuthor>();

        }

        #region Service Injections










        #endregion

        #region variables declaration
        [Parameter]
        public int LiteratureId { get; set; }
        public Telerik.DataSource.FilterOperator ReusableFilterOperator { get; set; } = Telerik.DataSource.FilterOperator.Contains;
        public bool AddConfirmVisible { get; set; }
        public bool CancelConfirmVisible { get; set; }

        protected bool isLoading;
        public bool ShowWizard { get; set; } = true;
        public bool IsDraftVisiable { get; set; }
        public int Value { get; set; }
        public int SelectedIndexId { get; set; } = 0;
        public int PreviousIndexNo { get; set; } = 0;
        public int SelectedDivisionId { get; set; } = 0;
        public int SelectedAisleNumber { get; set; } = 0;
        public int PreviousDivisionNo { get; set; } = 0;
        public string PreviousCharacters { get; set; } = string.Empty;
        public RadzenDropDown<int> IndexDDRef { get; set; }
        public TelerikDropDownList<LmsLiteratureIndexDivisionAisle, int> DivisionsDDRef { get; set; }
        public TelerikDropDownList<LmsLiteratureIndexDivisionAisle, int> AislesDDRef { get; set; }
        public LmsLiteratureIndex selectedLiteratureIndex { get; set; }
        public LmsLiterature Literature { get; set; } = new LmsLiterature();
        public int IncrementedBarcodeCount { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1950, 1, 1);
        bool isBasicStep = false;
        bool isAuthorStep = false;
        int AuthorId;
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
        public LmsLiteratureAuthor lmsLiteratureAuthor { get; set; } = new LmsLiteratureAuthor();
        public LmsLiteratureDetailsLmsLiteratureAuthor lmsLiteratureDetailsLmsLiteratureAuthor { get; set; }
        public List<LmsLiteratureAuthor> lmsLiteratureAuthorForGrid { get; set; } = new List<LmsLiteratureAuthor>();
        public bool ShowAuthorGrid;
        public RadzenDataGrid<LmsLiteratureAuthor>? AuthorGridRef = new RadzenDataGrid<LmsLiteratureAuthor>();
        DateTime? editionYear = null;
        protected EditContext MyEditContext { get; set; }
        protected string newAuthor { get; set; } = "Existing";
        protected int authorBooks { get; set; }
        protected string filePath1 = "\\images\\lmsLiteratureDetail-1.png";
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

        protected LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivisionAisleDetails { get; set; } = null;

        protected RadzenDataGrid<LiteratureDetailLiteratureTagVM> TagsGrid;
        protected RadzenDataGrid<LmsLiteratureBarcode> BarcodesGrid = new RadzenDataGrid<LmsLiteratureBarcode>();
        protected List<LiteratureTag> ActiveLiteratureTags { get; set; } = new List<LiteratureTag>();
        protected LiteratureDetailLiteratureTagVM tagToInsert;
        public bool IsPrintDialogVisible { get; set; } = false;
        protected PrintCommandEnum PrintCommand { get; set; }
        protected string PrintValue { get; set; }
        protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
        int ExistingSeriesNumber { get; set; } = 0;
        int ExistingSeriesNumberNotChanged { get; set; } = 0;
        protected LmsLiteratureBarcode lmsLiteratureBarcode { get; set; } = new LmsLiteratureBarcode();
        protected bool IsRFIdMatch = false;
        protected bool IsDisabled = false;

        protected Dictionary<int?, RadzenDataGrid<LmsLiteratureBarcode>> myOptionComponents = new Dictionary<int?, RadzenDataGrid<LmsLiteratureBarcode>>();
        protected IEnumerable<LmsLiteratureAuthor> GetAuthorsData { get; set; }

        #endregion

        public class AuthorTypeModel
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
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
            public string Full_Name_Ar { get; set; } = string.Empty;
            public string Full_Name_En { get; set; } = string.Empty;
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

        protected override async Task OnInitializedAsync()
        {
            userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
            spinnerService.Show();
            AuthorTypes.Add(new AuthorTypeModel() { Text = translationState.Translate("Existing"), Value = "Existing" });
            AuthorTypes.Add(new AuthorTypeModel() { Text = translationState.Translate("Add_New"), Value = "New" });

            Literature.LiteratureId = LiteratureId;
            Literature.IsSeries = true;
            Literature = await lmsLiteratureService.GetLmsLiteratureById(LiteratureId);
            if (Literature.IsDraft == true)
            {
                IsDraftVisiable = true;
            }
            else
            {
                IsDraftVisiable = false;
            }
            Literature.LiteratureBarcodes.ForEach(x =>
            {
                if (long.TryParse(x.RFIDValue, out long rfid))
                {
                    x.RFID = rfid;
                }
                else
                {
                    x.RFID = null;
                }
            });
            editionYear = Literature.EditionYear;
            ExistingSeriesNumber = Literature.SeriesNumber;
            ExistingSeriesNumberNotChanged = Literature.SeriesNumber;
            Literature.DeletedAttachementIds = new List<int>();
            currentForm = new LmsLiterature();
            MyEditContext = new EditContext(currentForm);
            //lmsLiteratureBarcode =MyEditContext.where(x)
            Literature.LiteratureAttachements = new();
            await GetLiteratureIndexDetails();
            await GetRemoteAuthorsData();
            //await GetLiteratureIndexDivisionAisleDetails();

            var tagsResponse = await lmsLiteratureService.GetAllActiveLiteratureTags();
            if (tagsResponse.IsSuccessStatusCode)
            {
                ActiveLiteratureTags = (List<LiteratureTag>)tagsResponse.ResultData;
            }
            // for Edit LmsLiterature Authors
            if (Literature.LmsLiteratureAuthors.Count() != 0)
            {
                lmsLiteratureAuthorForGrid = Literature.LmsLiteratureAuthors;
                if (lmsLiteratureAuthorForGrid.Count() != 0)
                {
                    ShowAuthorGrid = true;
                }
            }

            SetIndexData();
            SetAuthorFields();
            await Task.Delay(200);
            StateHasChanged();
            PreviousCharacters = Literature.Characters;
            spinnerService.Hide();
            if (Literature.Purchase_Date == null)
            {
                Literature.Purchase_Date = DateTime.Now;
            }

            Literature.PreviousCopyCount = Literature.CopyCount;
            Literature.PreviousSeriesNumber = Literature.SeriesNumber;
            Literature.PreviousDeweyBookNumber = Literature.DeweyBookNumber;
            Literature.PreviousLmsLiteratureIndex = Literature.LmsLiteratureIndex;
            Literature.PreviousLmsLiteratureBarcode = Literature.LiteratureBarcodes;

        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating basic details</History>

        protected void ValidateAuthorDetailsOnChange()
        {
            if (isAuthorStep)
            {
                ValidatePurchaseAuthorDetails();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating purchase and author details</History>
        protected bool ValidatePurchaseAuthorDetails()
        {
            bool purAuthDetailsValid = true;
            if (string.IsNullOrWhiteSpace(Literature.Publisher))
            {
                purchaseAuthorValidations.Publisher = "k-invalid";
                purAuthDetailsValid = false;
            }
            else
            {
                purchaseAuthorValidations.Publisher = "k-valid";
            }
            if (Thread.CurrentThread.CurrentUICulture.Name == "")
            {
                if (Literature.LmsLiteratureAuthors.Count() == 0)
                {
                    purchaseAuthorValidations.Full_Name_En = "k-invalid";
                    purchaseAuthorValidations.Address_En = "k-invalid";
                    purAuthDetailsValid = false;
                }
                else
                {
                    purchaseAuthorValidations.Full_Name_En = "k-invalid";
                    purchaseAuthorValidations.Address_En = "k-invalid";
                }
            }
            else
            {
                if (Literature.LmsLiteratureAuthors.Count() == 0)
                {
                    purchaseAuthorValidations.Full_Name_En = "k-invalid";
                    purchaseAuthorValidations.Address_En = "k-invalid";
                    purAuthDetailsValid = false;
                }
                else
                {
                    purchaseAuthorValidations.Full_Name_En = "k-invalid";
                    purchaseAuthorValidations.Address_En = "k-invalid";
                }
            }
            return purAuthDetailsValid;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Function for getting Index records for the drop down dynamically from the API</History>
        //<History Author = 'Umer Zaman' Date='2022-07-16' Version="1.0" Branch="master">Change and modified method to get index details from index table</History>
        private async Task<IEnumerable<LmsLiteratureIndex>> GetLiteratureIndexDetails() // GetRemoteIndexesData
        {
            AllLiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
            return AllLiteratureIndexeDetails;
        }
        //<History Author = 'Umer Zaman' Date='2022-07-16' Version="1.0" Branch="master">Change and modified method to get index details from index table</History>
        private async Task<IEnumerable<LmsLiteratureIndexDivisionAisle>> GetLiteratureIndexDivisionAisleDetails() // GetRemoteIndexesData
        {
            DivisionAisleDetailsList = await lmsLiteratureIndexDivisionServices.GetLmsLiteratureIndexDivisions();
            return DivisionAisleDetailsList;
        }


        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Set Index Dependent Fields</History>
        //<History Author = 'Umer Zaman' Date='2022-07-16' Version="1.0" Branch="master">Change method name and get selected index detail from index table by using selected index id</History>
        protected async void OnChangeIndexNumberDropDown()
        {
            if (SelectedIndexId > 0)
            {
                selectedLiteratureIndex = await lmsLiteratureIndexService.GetLiteratureIndexDetailByUsingIndexId(SelectedIndexId);
                Literature.LmsLiteratureIndex = selectedLiteratureIndex;
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

                if (SelectedIndexId <= 0)
                {
                    SelectedIndexId = selectedLiteratureIndex.IndexId;
                }
                StateHasChanged();
            }
            else
            {
                selectedLiteratureIndex = new LmsLiteratureIndex();
                DivisionListUsingIndexId = new List<LmsLiteratureIndexDivisionAisle>();
                AisleListUsingIndexAndDivision = new List<LmsLiteratureIndexDivisionAisle>();
            }
        }
        //<History Author = 'Umer Zaman' Date='2022-07-18' Version="1.0" Branch="master">Create method to get DivisionAisleId by using selected aisle</History>
        protected void OnChangeAisleNumberDropDown()
        {
            if (SelectedIndexId != 0 && SelectedAisleNumber != 0)
            {
                var GetAisleNumber = DivisionDetailsForAisleNumber.Where(x => x.IndexId == SelectedIndexId && x.DivisionAisleId == SelectedAisleNumber).FirstOrDefault();
                if (GetAisleNumber != null)
                {
                    lmsLiteratureIndexDivisionAisleDetails = AisleListUsingIndexAndDivision.Where(x => x.IndexId == GetAisleNumber.IndexId && x.DivisionNumber == GetAisleNumber.DivisionNumber && x.AisleNumber == GetAisleNumber.AisleNumber).FirstOrDefault();
                }
            }
        }
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
        #region Add Author button click
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
                            CreatedBy = loginState.UserDetail.UserName,
                            CreatedDate = DateTime.Now
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
                            FullName_En = lmsLiteratureAuthor.FullName_En,
                            Address_En = lmsLiteratureAuthor.Address_En,
                            CreatedBy = loginState.UserDetail.UserName,
                            CreatedDate = DateTime.Now
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

        //#region Author grid operations
        protected async Task UpdateAuthorHandler(LmsLiteratureAuthor args) //step 3.  grid row record click for update
        {
            var resultObj = args;
            if (resultObj != null)
            {
                lmsLiteratureAuthor.AuthorId = resultObj.AuthorId;
                lmsLiteratureAuthor.FullName_En = resultObj.FullName_En;
                lmsLiteratureAuthor.FullName_Ar = resultObj.FullName_Ar;
                lmsLiteratureAuthor.FirstName_En = resultObj.FirstName_En;
                lmsLiteratureAuthor.ThirdName_En = resultObj.ThirdName_En;
                lmsLiteratureAuthor.Address_En = resultObj.Address_En;
                lmsLiteratureAuthor.FirstName_Ar = resultObj.FirstName_Ar;
                lmsLiteratureAuthor.ThirdName_Ar = resultObj.ThirdName_Ar;
                lmsLiteratureAuthor.Address_Ar = resultObj.Address_Ar;
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
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Function for getting Type records for the drop down dynamically from the API</History>
        protected async Task GetRemoteTypesData(DropDownListReadEventArgs args)
        {
            DataEnvelope<LmsLiteratureType> result = await lmsLiteratureTypeService.GetTypesItems(args.Request);

            args.Data = result.Data;
            args.Total = result.Total;
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master"> Function for getting Type details of the selected item</History>
        protected async Task<LmsLiteratureType> GetTypeModelFromValue(int selectedValue)
        {
            return await lmsLiteratureTypeService.GetItemFromValue(selectedValue);
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-01' Version="1.0" Branch="master"> Function for getting Classification details of the selected item</History>
        protected async Task<LmsLiteratureClassification> GetClassificationtModelFromValue(int selectedValue)
        {
            return await lmsLiteratureClassificationService.GetItemFromValue(selectedValue);
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
        //<History Author = 'Hassan Abbas' Date='2022-04-01' Version="1.0" Branch="master"> Function for getting Author details of the selected item</History>
        protected async Task<LmsLiteratureAuthor> GetAuthorModelFromValue(int selectedValue)
        {
            return await lmsLiteratureService.GetAuthorItemFromValue(selectedValue);
        }
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
                    Member = nameof(LmsLiteratureAuthor.FullName_Ar),
                    Operator = ReusableFilterOperator,
                    Value = searchString
                },
                new Telerik.DataSource.FilterDescriptor() {
                    Member = nameof(LmsLiteratureAuthor.FullName_En),
                    Operator = ReusableFilterOperator,
                    Value = searchString
                },
                new Telerik.DataSource.FilterDescriptor() {
                    Member = nameof(LmsLiteratureAuthor.Address_Ar),
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

        List<string> ThemeColors = new List<string>
        {
            "primary",
            "secondary",
            "tertiary",
            "info",
            "success",
            "warning",
            "error",
            "dark",
            "light",
            "inverse"
        };

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
            //valid = true;
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
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                args.IsCancelled = true;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Set Index Dependent Fields</History>
        protected async void SetIndexData()
        {
            if (Literature.IndexId > 0)
            {
                selectedLiteratureIndex = await lmsLiteratureIndexService.GetLiteratureIndexDetailByUsingIndexId(Literature.IndexId);
                Literature.LmsLiteratureIndex = selectedLiteratureIndex;
                var lmsdivision = await lmsLiteratureIndexDivisionServices.GetDivisionDetailsByUsingIndexAndDivisionId(Literature.DivisionAisleId, Literature.IndexId);
                if (SelectedIndexId <= 0)
                {
                    SelectedIndexId = selectedLiteratureIndex.IndexId;
                }
                if (SelectedDivisionId == 0)
                {
                    if (lmsdivision.Count() != 0)
                    {
                        SelectedDivisionId = lmsdivision.FirstOrDefault().DivisionAisleId;

                    }
                }
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

                PreviousIndexNo = SelectedIndexId;
                PreviousDivisionNo = SelectedDivisionId;
                if (lmsdivision.Count() != 0)
                {
                    SelectedAisleNumber = lmsdivision.FirstOrDefault().DivisionAisleId;
                }
                var GetDivisionNumber = DivisionDetailsForAisleNumber.Where(x => x.DivisionAisleId == SelectedDivisionId).FirstOrDefault();
                if (GetDivisionNumber != null)
                {
                    AisleListUsingIndexAndDivision = DivisionDetailsForAisleNumber.Where(x => x.IndexId == SelectedIndexId && x.DivisionNumber == GetDivisionNumber.DivisionNumber).ToList();
                }
            }
            else
            {
                selectedLiteratureIndex = new LmsLiteratureIndex();
            }
            StateHasChanged();
        }

        protected void OnChangeDivisionDropDown()
        {
            if (SelectedIndexId != 0 && SelectedDivisionId != 0)
            {
                var GetDivisionNumber = DivisionDetailsForAisleNumber.Where(x => x.DivisionAisleId == SelectedDivisionId).FirstOrDefault();
                if (GetDivisionNumber != null)
                {
                    AisleListUsingIndexAndDivision = DivisionDetailsForAisleNumber.Where(x => x.IndexId == SelectedIndexId && x.DivisionNumber == GetDivisionNumber.DivisionNumber).ToList();
                    if (AisleListUsingIndexAndDivision.Count() != 0)
                    {
                        SelectedAisleNumber = 0;
                        AislesDDRef.Rebind();
                    }
                }
            }
            else
            {
                AisleListUsingIndexAndDivision = new List<LmsLiteratureIndexDivisionAisle>();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Set Author Dependent Fields</History>
        protected async void SetAuthorFields()
        {
            if (Literature.Author_Id > 0)
            {
                var lmsAuthor = await lmsLiteratureService.GetLmsLiteratureAuthorById(Literature.Author_Id);
                authorBooks = await lmsLiteratureService.GetLmsLiteratureCountByAuthorId(Literature.Author_Id);
                await Task.Delay(200);
                Literature.Author_FirstName_Ar = lmsAuthor.FirstName_Ar;
                Literature.Author_FirstName_En = lmsAuthor.FirstName_En;
                Literature.Author_ThirdName_Ar = lmsAuthor.ThirdName_Ar;
                Literature.Author_ThirdName_En = lmsAuthor.ThirdName_En;
                Literature.Author_Address_Ar = lmsAuthor.Address_Ar;
                Literature.Author_Address_En = lmsAuthor.Address_En;
                StateHasChanged();
            }
            else
            {
                Literature.Author_FirstName_Ar = string.Empty;
                Literature.Author_FirstName_En = string.Empty;
                Literature.Author_ThirdName_Ar = string.Empty;
                Literature.Author_ThirdName_En = string.Empty;
                Literature.Author_Address_Ar = string.Empty;
                Literature.Author_Address_En = string.Empty;
                StateHasChanged();
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
            bool valid = ValidatePurchaseAuthorDetails();
            //valid = true;
            if (valid)
            {
                int fromSaveCount = args.TargetIndex - 1;
                if (fromSaveCount > 0)
                {
                    Value += 1;
                    await Task.Delay(50);
                    StateHasChanged();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Add_Author_Details_And_Required_Field"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                args.IsCancelled = true;
            }
            if (valid)
            {
                return true;
            }
            return false;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Invoke Barcode step change</History>
        public void OnStickerBarcodeStepChange(WizardStepChangeEventArgs args)
        {
            if (Literature.LiteratureBarcodes.Any())
            {
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
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Atleast_One_Book_Copy"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-20' Version="1.0" Branch="master">Show Add Book confirmation Dialog</History>
        protected async Task SubmitLiterature()
        {
            AddConfirmVisible = true;
        }

        #region Add Book Dialog
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>
        protected void VisibleChangedHandlerAdd(bool currVisible)
        {
            AddConfirmVisible = currVisible;
        }

        //<History Author = 'Nabeel ur Rehman' Date='2023-07-26' Version="1.0" Branch="master">Save As Draft Literature on Comfirmation og Dialog</History>
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
                    if (await dialogService.Confirm(translationState.Translate("Sure_Submit_Literature"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {

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
                        var response = await lmsLiteratureService.UpdateLmsLiterature(Literature);
                        if (response.IsSuccessStatusCode)
                        {
                            AddConfirmVisible = false;
                            // Save Temp Attachement To Uploaded Document in update case
                            LmsLiterature LiteratureData = (LmsLiterature)response.ResultData;
                            Literature.LiteratureIdList = LiteratureData.LiteratureIdList;
                            await SaveTempAttachementToUploadedDocument();
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Book_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            BackToList();


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
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-20' Version="1.0" Branch="master">Submit Literature on Comfirmation og Dialog</History>
        protected async Task OkayAddBookConfirm()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit_Literature"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {

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
                    var response = await lmsLiteratureService.UpdateLmsLiterature(Literature);
                    if (response.IsSuccessStatusCode)
                    {

                        AddConfirmVisible = false;
                        // Save Temp Attachement To Uploaded Document in update case
                        LmsLiterature LiteratureData = (LmsLiterature)response.ResultData;
                        Literature.LiteratureIdList = LiteratureData.LiteratureIdList;
                        await SaveTempAttachementToUploadedDocument();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Book_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        BackToList();


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
        protected async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            //loadedFiles.Clear();

            foreach (var file in e.GetMultipleFiles())
            {
                try
                {
                    Literature.LiteratureAttachements.Add(file);
                }
                catch (Exception ex)
                {
                }
            }
            isLoading = false;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">Add another Book after submission of book</History>
        protected async Task BackToForm()
        {
            //ShowWizard = true; 
            await JSRuntime.InvokeVoidAsync("window.location.reload");
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">redirect to literature list after submission of book</History>
        protected async Task BackToList()
        {
            //ShowWizard = false; 
            navigationManager.NavigateTo("lmsliterature-list");
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-23' Version="1.0" Branch="master">function for validating basic details</History>
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
            if (Literature.TypeId <= 0)
            {
                validations.Type = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.Type = "k-valid";
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
            if (Literature.SeriesNumber > ExistingSeriesNumber) // increase series number
            {
                //IncrementedBarcodeCount = Literature.SeriesNumber - 1;
                var NewlyAddedSeriesNumber = Literature.SeriesNumber - ExistingSeriesNumber;
                IncrementedBarcodeCount = NewlyAddedSeriesNumber * ExistingSeriesNumberNotChanged;
                //IncrementedBarcodeCount = TotalCount - Literature.CopyCount;
                InsertNewBarcodes();
                ExistingSeriesNumber = Literature.SeriesNumber;
            }
            return basicDetailsValid;
        }

        public async Task OnAuthorRadioChangeHandler(object newValue)
        {
            if (newValue.ToString() == "Existing")
            {
                var lmsAuthor = await lmsLiteratureService.GetLmsLiteratureAuthorById(Literature.Author_Id);
                Literature.Author_FullName_En = lmsAuthor.FullName_En;
                Literature.Author_FullName_Ar = lmsAuthor.FullName_Ar;
                Literature.Author_Address_Ar = lmsAuthor.Address_Ar;
                Literature.Author_Address_En = lmsAuthor.Address_En;
            }
            else
            {
                Literature.Author_FullName_En = string.Empty;
                Literature.Author_FullName_Ar = string.Empty;
                Literature.Author_Address_Ar = string.Empty;
                Literature.Author_Address_En = string.Empty;
            }
        }

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
                        Summary = $"!????",
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
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
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
                        Summary = $"!????",
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
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
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
                var resultValue = Literature.LiteratureTags.Where(x => x.TagNo == tag.TagNo).Select(x => x.Value).FirstOrDefault();
                if (resultValue != null)
                {
                    tag.TempValue = resultValue;
                }
                tag.Description = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? ActiveLiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Description : ActiveLiteratureTags.Where(x => x.TagNo == tag.TagNo).FirstOrDefault().Description_Ar;

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
            if (!barcode.IsBorrowed)
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
        }

        protected async Task DeleteBarcode(LmsLiteratureBarcode barcode)
        {
            var result = Literature.LiteratureBarcodes.Where(x => x.BarCodeNumber == barcode.BarCodeNumber).FirstOrDefault();
            if (result != null)
            {
                Literature.DeletedLiteratureBarcodes.Add(result);
                Literature.LiteratureBarcodes.Remove(result);
                await BarcodesGrid.Reload();
                Literature.CopyCount -= 1;
            }
        }


        protected async Task InsertNewBarcodes()
        {
            var response = await lmsLiteratureService.GenerateListofLiteratureBarcode(IncrementedBarcodeCount);
            if (response.IsSuccessStatusCode)
            {
                Literature.LiteratureBarcodes = Literature.LiteratureBarcodes.Concat((List<LmsLiteratureBarcode>)response.ResultData).ToList();
                Literature.CopyCount = Literature.CopyCount + IncrementedBarcodeCount;
                IncrementedBarcodeCount = 0;
                await BarcodesGrid.Reload();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion


        protected async Task EditOptionRow(LmsLiteratureBarcode option)
        {
            IsDisabled = true;
            await BarcodesGrid.EditRow(option);

        }
        protected void OnUpdateRow(LmsLiteratureBarcode barcode)
        {

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
                        //LmsLiteratureBarcode.RFIDValue = int.Empty;
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
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Save Temp Attachement To Uploaded Document
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                // Get a list of IDs for the Legal Principe  itself.
                List<Guid> requestIds = new List<Guid>();
                requestIds.Add((Guid)Literature.Guid);
                // Save Temp Attachement To Uploaded Documents
                if (Literature.LiteratureIdList.Count > 0)
                {
                    // if attachment available and user increament series number
                    var res = await fileUploadService.CheckingAttachementInTemp(new FileUploadVM()
                    {
                        RequestIds = requestIds,
                        CreatedBy = loginState.Username,
                        FilePath = _config.GetValue<string>("dms_file_path"),
                        DeletedAttachementIds = Literature.DeletedAttachementIds,
                        LiteratureId = Literature.LiteratureId,
                        LiteratureIds = Literature.LiteratureIdList
                    });
                    if (res.IsSuccessStatusCode)
                    {
                        var result = (List<TempAttachement>)res.ResultData;
                        if (result.Count == 0)
                        {
                            var docResponse = await fileUploadService.GetUploadedAttachementAndWithNewOne(new FileUploadVM()
                            {
                                RequestIds = requestIds,
                                CreatedBy = loginState.Username,
                                FilePath = _config.GetValue<string>("dms_file_path"),
                                DeletedAttachementIds = Literature.DeletedAttachementIds,
                                LiteratureId = Literature.LiteratureId,
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
                        else
                        {
                            Literature.LiteratureIdList.Add(Literature.LiteratureId);
                            var docResponse = await fileUploadService.SaveLiteratureTempAttachementToUploadedDocument(new FileUploadVM()
                            {
                                RequestIds = requestIds,
                                CreatedBy = loginState.Username,
                                FilePath = _config.GetValue<string>("dms_file_path"),
                                DeletedAttachementIds = Literature.DeletedAttachementIds,
                                LiteratureId = Literature.LiteratureId,
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
                    }

                }
                else
                {
                    var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                    {
                        RequestIds = requestIds,
                        CreatedBy = loginState.Username,
                        FilePath = _config.GetValue<string>("dms_file_path"),
                        DeletedAttachementIds = Literature.DeletedAttachementIds,
                        LiteratureId = Literature.LiteratureId,
                        //LiteratureIds = Literature.LiteratureIdList
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
