using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel.PACIVM;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Enum;
namespace FATWA_WEB.Pages.PACI
{
    public partial class PACIRequestList : ComponentBase
    {
        public PACIRequestList()
        {
            PACIRequestDataForGrid = new List<PACIRequestDataVM>();

        }

        #region Variables
        protected RadzenDataGrid<PACIRequestListVM> grid;
        public string firsttwovalues;
        public PACIRequestDataVM pACIRequestData { get; set; } = new PACIRequestDataVM();
        public PACIRequestVM pACIRequest = new PACIRequestVM();
        public List<PACIRequestDataVM> PACIRequestDataForGrid { get; set; } = new List<PACIRequestDataVM>();
        public RadzenDataGrid<PACIRequestDataVM>? PACIRequestDataeGridRef = new RadzenDataGrid<PACIRequestDataVM>();
        public int selectedIndex { get; set; } = 0;
        public byte[] FileData { get; set; }
        public bool txtAddress { get; set; } = true;
        protected int count { get; set; }
        public string labelRef;
        public bool busyPreviewBtn { get; set; }
        public int Cases { get; set; } = 0;
        public bool TextboxEnable { get; set; }
        public bool isYearEnable { get; set; }
        public bool Txtvalidationvisiable { get; set; }
        public bool TxtYearvalidationvisiable { get; set; }
        public bool isVisible { get; set; }
        public bool YearValidationLable { get; set; }
        public bool ValidationLableAddressType { get; set; }
        public bool CivilIdlimitValidationLable { get; set; }
        public bool NameValidationLable { get; set; }
        public bool NameNumberValidationLable { get; set; }
        protected string TemplateContent { get; set; }
        protected ValidationClass validations { get; set; } = new ValidationClass();
        public int selectedValue = 0;
        public List<int> optionsList;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public SfPdfViewerServer viewer { get; set; } = new SfPdfViewerServer();
        public bool DisplayDocumentViewer { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        #region Validations class

        protected class ValidationClass
        {
            public string CivilId { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string CaseNumber { get; set; } = string.Empty;
            public string Year { get; set; } = string.Empty;
            public string Addresstype { get; set; } = string.Empty;

        }
        #endregion
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                pACIRequests = new PACIRequestVM();
                pACIRequestsData = new PACIRequestDataVM();
                pACIRequest.RequestId = Guid.NewGuid();
                labelRef = "display: inline-block; color: #f3456b !important; font-size: 0.75rem !important; padding: 0;";
                await OnDropDownValueChanged(0);
                await OnChangeRadioButton1(0);
                await GetNewRequestReferenceNumber();
                await GetYearList();
                ////loadpending
                System.Timers.Timer t_loadpending = new System.Timers.Timer();
                t_loadpending.Elapsed += async (s, e) =>
                {
                    await Load();
                    await InvokeAsync(StateHasChanged);
                };
                t_loadpending.Interval = 90000;
                t_loadpending.Interval = 1000 * 120 * 2;
                t_loadpending.Start();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw;
            }





        }
        PACIRequestVM _pACIRequests;

        protected PACIRequestVM pACIRequests
        {
            get
            {
                return _pACIRequests;
            }
            set
            {
                if (!object.Equals(_pACIRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "pACIRequests", NewValue = value, OldValue = _pACIRequests };
                    _pACIRequests = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        PACIRequestDataVM _pACIRequestsData;
        protected PACIRequestDataVM pACIRequestsData
        {
            get
            {
                return _pACIRequestsData;
            }
            set
            {
                if (!object.Equals(_pACIRequestsData, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "pACIRequestsData", NewValue = value, OldValue = _pACIRequestsData };
                    _pACIRequestsData = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected bool lablesValidateBasicDetails()
        {
            bool basicDetailsValid = true;
            if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
            {
                if (String.IsNullOrWhiteSpace(pACIRequest.AddressType))
                {
                    ValidationLableAddressType = true;

                    basicDetailsValid = false;
                }
                else
                {
                    ValidationLableAddressType = false;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(pACIRequest.AddressType))
                {
                    ValidationLableAddressType = true;

                    basicDetailsValid = false;
                }
                else
                {
                    ValidationLableAddressType = false;
                }
            }

            return basicDetailsValid;
        }
        private void ValidateValue(ChangeEventArgs args)
        {
            pACIRequestData.CivilId = args.Value.ToString();
            if (Convert.ToInt32(pACIRequestData.CivilId.Length) < 12 && Convert.ToInt32(pACIRequestData.CivilId.Length) != 0)
            {
                CivilIdlimitValidationLable = true;
            }
            if (!Regex.IsMatch(pACIRequestData.CivilId.Trim(), @"^\d+$"))
            {
                CivilIdlimitValidationLable = true;
            }
            else if (pACIRequestData.CivilId.Trim() == "")
            {
                CivilIdlimitValidationLable = false;
            }
            else
            {
                CivilIdlimitValidationLable = false;
            }
        }

        protected bool lablesValidateIdentificationInfo()
        {
            bool basicDetailsValid = true;

            if (String.IsNullOrWhiteSpace(pACIRequestData.Name))
            {
                NameValidationLable = true;
                basicDetailsValid = false;
                return basicDetailsValid;
            }
            else if (!Regex.IsMatch(pACIRequestData.Name, "^[A-Za-z\u0621-\u064A ]+$"))
            {
                NameNumberValidationLable = true;
                basicDetailsValid = false;
                return basicDetailsValid;
            }
            if (String.IsNullOrWhiteSpace(pACIRequestData.CivilId))
            {
                basicDetailsValid = true;
            }

            else if (!Regex.IsMatch(pACIRequestData.CivilId.Trim(), @"^\d+$"))
            {
                CivilIdlimitValidationLable = true;
                basicDetailsValid = false;
            }

            else
            {
                NameValidationLable = false;
            }
            return basicDetailsValid;
        }


        #endregion
        #region Component Load
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        protected async Task Load()
        {
            try
            {

                translationState.TranslateGridFilterLabels(grid);
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                var username = loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? "Admin" : loginState.Username;
                var response = await pACIRequestService.GetAllPACIRequests(username);
                if (response.IsSuccessStatusCode)
                {
                    getPaciRequestResult = (IEnumerable<PACIRequestListVM>)response.ResultData;
                    FilteredGetPaciRequestList = (IEnumerable<PACIRequestListVM>)response.ResultData;
                    count = getPaciRequestResult.Count();
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        //<History Author = 'ijaz Ahmad' Date='2022-12-26' Version="1.0" Branch="master">Populate Request number</History>
        protected async Task GetNewRequestReferenceNumber()
        {
            var response = await pACIRequestService.GetNewRequestReferenceNumber();
            if (response.IsSuccessStatusCode)
            {
                pACIRequest.RefrenceNumber = Convert.ToInt32(response.ResultData);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        IEnumerable<PACIRequestListVM> _getPaciRequestResult;
        IEnumerable<PACIRequestListVM> FilteredGetPaciRequestList { get; set; } = new List<PACIRequestListVM>();
        protected IEnumerable<PACIRequestListVM> getPaciRequestResult
        {
            get
            {
                return _getPaciRequestResult;
            }
            set
            {
                if (!Equals(_getPaciRequestResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getPaciRequestResult", NewValue = value, OldValue = _getPaciRequestResult };
                    _getPaciRequestResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredGetPaciRequestList = await gridSearchExtension.Filter(getPaciRequestResult, new Query()
                    {
                        Filter = $@"i => (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0)) || (i.Names != null && i.Names.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredGetPaciRequestList = await gridSearchExtension.Filter(getPaciRequestResult, new Query()
                    {
                        Filter = $@"i => (i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0)) || (i.Names != null && i.Names.ToString().ToLower().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region PACI request On Change events 
        protected async Task OnDropDownValueChanged(int value)
        {
            pACIRequest.Year = selectedValue.ToString();
        }
        protected async Task OnChangeRadioButton1(int value)
        {
            if (value != null)
            {


                if (value == (int)DataEnum.Cases)
                {
                    Txtvalidationvisiable = true;
                    TxtYearvalidationvisiable = true;
                    TextboxEnable = false;
                    YearValidationLable = false;
                    pACIRequest.IsCases = true;
                    isYearEnable = true;
                }
                else if (value == (int)DataEnum.UnderFilling)
                {
                    Txtvalidationvisiable = false;
                    TxtYearvalidationvisiable = false;
                    TextboxEnable = true;
                    YearValidationLable = false;
                    pACIRequest.IsCases = false;
                    pACIRequest.Year = "0";
                    selectedValue = 0;
                    pACIRequest.CaseNumber = string.Empty;
                    isYearEnable = true;
                }
            }
        }

        protected async Task OnChangeRadioButton(int value)
        {
            if (value != 0)
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {

                    if (value == (int)RequirdDataEnum.Residential)
                    {
                        txtAddress = true;
                        pACIRequest.AddressType = RequirdDataEnum.Residential.ToString();
                    }
                    else if (value == (int)RequirdDataEnum.WorkAddress)
                    {
                        txtAddress = true;
                        pACIRequest.AddressType = RequirdDataEnum.WorkAddress.ToString();
                    }
                    else if (value == (int)RequirdDataEnum.Others)
                    {
                        pACIRequest.AddressType = "";
                        txtAddress = false;
                    }
                }
                else
                {
                    if (value == (int)RequirdDataEnum.Residential)
                    {
                        txtAddress = true;
                        pACIRequest.AddressType = "عنوان السكن";
                    }
                    else if (value == (int)RequirdDataEnum.WorkAddress)
                    {
                        txtAddress = true;
                        pACIRequest.AddressType = "عنوان العمل";
                    }
                    else if (value == (int)RequirdDataEnum.Others)
                    {
                        pACIRequest.AddressType = "";
                        txtAddress = false;
                    }
                }
            }
        }
        public async void OnTabChange(int index)
        {
            spinnerService.Show();
            search = "";
            selectedIndex = index;
            if (index == 1)
            {
                await Load();
                await GetNewRequestReferenceNumber();
            }
            spinnerService.Hide();
        }

        #endregion

        #region PACI Request button click Events 
        protected async Task AddCivilDetailToGrid()
        {
            bool valid = lablesValidateIdentificationInfo();
            if (valid)
            {
                if (pACIRequestData.CivilId != null && pACIRequestData.CivilId.Trim() != "")
                {
                    if (pACIRequestData.CivilId.Length < 12)
                    {
                        CivilIdlimitValidationLable = true;
                        return;
                    }
                    else
                    {
                        CivilIdlimitValidationLable = false;
                        PACIRequestDataForGrid.Add(new PACIRequestDataVM()
                        {
                            id = Guid.NewGuid(),
                            RequestId = pACIRequest.RequestId,
                            CivilId = pACIRequestData.CivilId,
                            Name = pACIRequestData.Name
                        }); ;
                        pACIRequestData.CivilId = string.Empty;
                        pACIRequestData.Name = string.Empty;

                        NameNumberValidationLable = false;

                    }
                }
                else
                {

                    PACIRequestDataForGrid.Add(new PACIRequestDataVM()
                    {
                        id = Guid.NewGuid(),
                        RequestId = pACIRequest.RequestId,
                        CivilId = pACIRequestData.CivilId,
                        Name = pACIRequestData.Name
                    }); ;
                    pACIRequestData.CivilId = string.Empty;
                    pACIRequestData.Name = string.Empty;
                    NameNumberValidationLable = false;

                    CivilIdlimitValidationLable = false;
                }
            }
            else
            {
                if (String.IsNullOrWhiteSpace(pACIRequestData.Name))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_At_Least_Name"),
                        Style = "position: fixed !important; left: 0; margin: auto; ",
                    });
                    if (PACIRequestDataForGrid.Count() == 0)
                    {

                    }

                }
                else
                {

                }

            }
            pACIRequestData = new PACIRequestDataVM();
            PACIRequestDataeGridRef.Reset();
            StateHasChanged();
            await PACIRequestDataeGridRef.Reload();
            await InvokeAsync(StateHasChanged);

        }

        protected async Task DeleteRequestDataHandler(PACIRequestDataVM args)
        {
            PACIRequestDataForGrid.Remove(args);
            await Task.Delay(200);
            notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Detail = translationState.Translate("Row_has_been_Deleted"),
                Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;",
            });
            await Task.Delay(1000);
            await PACIRequestDataeGridRef.Reload();
            await InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Submit PACI Button Click as a (In Review) flow status
        protected async Task SubmitRequest(PACIRequestVM args)
        {
            try
            {
                bool valid = lablesValidateBasicDetails();

                if (valid)
                {
                    if (PACIRequestDataForGrid.Count() > 0)
                    {
                        pACIRequest.PACIRequestsData = PACIRequestDataForGrid;
                        if (await dialogService.Confirm(translationState.Translate("Conformation_RequestSubmit_Massage"), translationState.Translate("Confirm"), new ConfirmOptions()
                        {
                            OkButtonText = translationState.Translate("OK"),
                            CancelButtonText = translationState.Translate("Cancel")
                        }) == true)
                        {
                            pACIRequest.Year = Convert.ToString(selectedValue);
                            if (pACIRequest.Year == "0")
                            {
                                pACIRequest.Year = string.Empty;
                            }

                            var response = await pACIRequestService.SaveRequest(pACIRequest);
                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Request_Success_Massage"),
                                    Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;",
                                });
                                await Task.Delay(2000);
                                navigationManager.NavigateTo("/paci-address-list");
                                ResetFields();
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Identification_Grid_First"),
                            Style = "position: fixed !important; left: 0; margin: auto;",
                            Duration = 2000
                        });
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        Style = "position: fixed !important; left: 0; margin: auto;",
                        Duration = 900
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void ResetFields()
        {
            pACIRequest.Year = "0";
            pACIRequest.CaseNumber = string.Empty;
            pACIRequest.AddressType = string.Empty;
            pACIRequestData.CivilId = string.Empty;
            pACIRequestData.Name = string.Empty;
            PACIRequestDataeGridRef.Reset();
            PACIRequestDataForGrid.Clear();
            OnDropDownValueChanged(0);
            selectedValue = 0;
            Reload();
            OnTabChange(1);
        }

        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Preview Draft
        //< History Author = 'Nabeel ur Rehman' Date = '2023-11-18' Version = "1.0" Branch = "master" >Preview Draft</History>
        protected async Task PreviewDraft()
        {
            try
            {
                busyPreviewBtn = true;
                StateHasChanged();
                isVisible = true;
                await Task.Run(() => PopulatePdfFromHtml());
                busyPreviewBtn = false;
            }
            catch (Exception ex)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
            }
        }
        //<History Author = 'Nabeel ur Rehman' Date='2023-11-18' Version="1.0" Branch="master">Populate Pdf from Html</History>
        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                converter.Options.EmbedFonts = true;
                TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                FileData = stream.ToArray();
            }
            catch (Exception ex)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region Grid Events
        protected async Task GridDeleteButtonClick(MouseEventArgs args, PACIRequestListVM data)
        {
            if (await dialogService.Confirm(translationState.Translate("Request_Confarmation_Massage"), translationState.Translate("delete"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await pACIRequestService.DeleteRequest(data);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Request_Deleted"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Load();
                    StateHasChanged();
                }
                await Load();
                StateHasChanged();
            }

        }

        protected async Task GridViewDetailClick(MouseEventArgs args, PACIRequestListVM data)
        {
            var dialogResult = await dialogService.OpenAsync<ViewRequestDailog>(
            translationState.Translate("Extracted_Data"),
            new Dictionary<string, object>() { { "RequestId", data.RequestId } },
            new DialogOptions() { Width = "100% !Importent", CloseDialogOnOverlayClick = true })/* + args.RequestId*/;
            await Task.Delay(400);
        }
        #region View File
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> File view</History>
        protected async Task ViewAttachement(PACIRequestListVM pACIRequestVM)
        {
            try
            {

                var response = await pACIRequestService.GetAllPACIRequestbyRequestId(Convert.ToString(pACIRequestVM.RequestId));
                if (response.IsSuccessStatusCode)
                {
                    var attachment = (List<PACIRequestListVM>)response.ResultData;

                    if (attachment != null)
                    {
                        ToolbarItems = new List<ToolbarItem>()
                                {
                                    ToolbarItem.PageNavigationTool,
                                    ToolbarItem.MagnificationTool,
                                    ToolbarItem.SelectionTool,
                                    ToolbarItem.PanTool,
                                    ToolbarItem.SearchOption,
                                    ToolbarItem.PrintOption,
                                    ToolbarItem.DownloadOption
                                };
                        spinnerService.Show();
                        DisplayDocumentViewer = false;
                        busyPreviewBtn = true;
                        await Task.Run(() => DecryptDocument(pACIRequestVM));
                        await Task.Delay(1000);
                        busyPreviewBtn = false;
                        spinnerService.Hide();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("File_Not_Found"),
                            Summary = translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }



                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
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
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task DecryptDocument(PACIRequestListVM pACIRequestVM)
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = pACIRequestVM.RequestStatusId != (int)RequestStatusEnum.PACIResponseReceived && pACIRequestVM.RequestStatusId != (int)RequestStatusEnum.PDFDataExtraction
                    ? Path.Combine(_config.GetValue<string>("dms_file_path") + pACIRequestVM.ResponseDocument).Replace(@"\\", @"\")
                    : Path.Combine(_config.GetValue<string>("dms_file_path") + pACIRequestVM.RequestDocument).Replace(@"\\", @"\");

                }
#else
				{
                 physicalPath = pACIRequestVM.RequestStatusId != (int)RequestStatusEnum.PACIResponseReceived && pACIRequestVM.RequestStatusId != (int)RequestStatusEnum.PDFDataExtraction
                        ? Path.Combine(_config.GetValue<string>("dms_file_path") + pACIRequestVM.ResponseDocument).Replace(@"\\", @"\")
                        : Path.Combine(_config.GetValue<string>("dms_file_path") + pACIRequestVM.RequestDocument).Replace(@"\\", @"\");

                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                }
                else
                {

                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });


                }
            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }
        #endregion
        protected void HandleTextChanged(ChangeEventArgs e)
        {
            if (e.Value is string text)
            {
                bool containsSpecialCharacters = Regex.IsMatch(text, @"[^[0-9]+$");
                if (!string.IsNullOrEmpty(text) && text.Length < 12 && !containsSpecialCharacters)
                {
                    CivilIdlimitValidationLable = true;
                }
                else
                {
                    CivilIdlimitValidationLable = false;
                }
            }


        }
        #endregion
        #region Get year
        //<History Author = 'ijaz Ahmad' Date='2022-07-20' Version="1.0" Branch="master">Populate Year List With Case Number</History>
        private void getfirsttwovalues(ChangeEventArgs args)
        {

            pACIRequest.CaseNumber = args.Value.ToString();
            if (Convert.ToInt32(pACIRequest.CaseNumber.Length) == 8 && Convert.ToInt32(pACIRequest.CaseNumber.Length) != 0)
            {
                string firstTwoValues = pACIRequest.CaseNumber.Substring(0, 2);
                int currentYear = DateTime.Now.Year;
                int endYear = 1950;

                if (endYear < currentYear)
                {
                    int temp = endYear;
                    endYear = currentYear;
                    currentYear = temp;
                }

                int count = endYear - currentYear + 1;
                List<string> optionsList = Enumerable.Range(currentYear, count)
                    .OrderByDescending(x => x)
                    .Select(x => x.ToString().Substring(2))
                    .ToList();

                string matchedYear = optionsList.FirstOrDefault(x => x == firstTwoValues);

                if (matchedYear != null)
                {
                    if (Convert.ToInt32(matchedYear) >= 50 && Convert.ToInt32(matchedYear) <= 99)
                    {
                        //  year is between 50 and 99, assume it belongs to the 1900s
                        string concatenatedValue = "19" + matchedYear.Substring(0, 2);
                        selectedValue = Convert.ToInt32(concatenatedValue);
                    }
                    else
                    {
                        string concatenatedValue = "20" + matchedYear.Substring(0, 2);
                        selectedValue = Convert.ToInt32(concatenatedValue);
                    }
                }


            }
            else
            {
                selectedValue = 0;
            }


        }
        //<History Author = 'ijaz Ahmad' Date='2022-07-10' Version="1.0" Branch="master">Populate Year List</History>
        protected async Task GetYearList()
        {
            try
            {
                int currentYear = DateTime.Now.Year;
                int endYear = 1950;
                if (endYear < currentYear)
                {
                    int temp = endYear;
                    endYear = currentYear;
                    currentYear = temp;
                }

                int count = endYear - currentYear + 1;
                optionsList = Enumerable.Range(currentYear, count).OrderByDescending(x => x).ToList();
            }
            catch (Exception)
            {

                throw;
            }


        }
        #endregion
    }
}
