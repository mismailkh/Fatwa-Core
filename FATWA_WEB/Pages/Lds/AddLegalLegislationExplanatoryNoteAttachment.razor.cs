using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Upload;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegalLegislationExplanatoryNoteAttachment : ComponentBase
    {
        #region Constructor
        public AddLegalLegislationExplanatoryNoteAttachment()
        {
            ShowResultFileNameInGrid = new List<TempAttachement>();
        }
        #endregion

        #region Parameter
        [Parameter]
        public Guid ExplanatoryNoteGuidId { get; set; }
        #endregion

        #region Variables declaration

        public List<TempAttachement> ShowResultFileNameInGrid { get; set; }
        public RadzenDataGrid<TempAttachement> FileGrid { get; set; }

        public string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/SingleUpload";
        public string RemoveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove";
        //public ObservableCollection<TempAttachement> TempFiles { get; set; } = new ObservableCollection<TempAttachement>();
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();
        protected string pathImg = null;
        protected string pathOldImg = null;
        public List<string> ValidFiles { get; set; } = new List<string>() { ".pdf" };
        public int MinFileSize { get; set; } = 1 * 1024; // 1 KB
        public int MaxFileSize { get; set; } = 100 * 1024 * 1024; // 100 MB

        #endregion       

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        LegalExplanatoryNote _legalExplanatoryNote;
        protected LegalExplanatoryNote legalExplanatoryNote
        {
            get
            {
                return _legalExplanatoryNote;
            }
            set
            {
                if (!object.Equals(_legalExplanatoryNote, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "legalExplanatoryNote", NewValue = value, OldValue = _legalExplanatoryNote };
                    _legalExplanatoryNote = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();

            spinnerService.Hide();
        }
        protected async Task Load()
        {
            if (ExplanatoryNoteGuidId != Guid.Empty && ExplanatoryNoteGuidId != null)
            {
                legalExplanatoryNote = new LegalExplanatoryNote()
                {
                    ExplanatoryNoteId = ExplanatoryNoteGuidId
                };
            }
            else
            {
                legalExplanatoryNote = new LegalExplanatoryNote()
                {
                    ExplanatoryNoteId = Guid.NewGuid()
                };
            }
        }
        #endregion

        #region Telerik Image Upload

        protected void OnSelectHandler(UploadSelectEventArgs e)
        {
            try
            {
                foreach (var item in e.Files)
                {
                    if (TempFiles.Where(x => x.FileName == item.Name).Count() > 0)
                    {
                        e.IsCancelled = true;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Legal_Legislation_ExplanatoryNote_file_already_uploaded"),
                            Summary = translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    if (!FilesValidationInfo.Keys.Contains(item.Id))
                    {
                        // nothing is assumed to be valid until the server returns an OK
                        FilesValidationInfo.Add(item.Id, IsSelectedFileValid(item));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        protected void OnCancelHandler(UploadCancelEventArgs e)
        {
            try
            {
                RemoveFailedFilesFromList(e.Files);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void OnRemoveHandler(UploadEventArgs e)
        {
            e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
            e.RequestData.Add("_userName", loginState.Username);
            RemoveFailedFilesFromList(e.Files);
        }

        protected async Task OnUploadHandler(UploadEventArgs e)
        {
            e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
            e.RequestData.Add("_pEntityIdentifierGuid", legalExplanatoryNote.ExplanatoryNoteId);
            e.RequestData.Add("_userName", loginState.Username);
            e.RequestData.Add("_uploadFrom", "LegislationExplanatoryNote");
            e.RequestData.Add("_oldImagePath", pathOldImg);
        }

        protected async Task OnSuccessHandler(UploadSuccessEventArgs e)
        {
            var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
            TempAttachementVM tempAttachement = new TempAttachementVM();
            tempAttachement.DocType = e.Files[0].Extension;
            tempAttachement.FileName = e.Files[0].Name;
            tempAttachement.UploadedBy = loginState.Username;
            TempFiles.Add(tempAttachement);
            await Task.Delay(100);
            //var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
            //TempAttachement tempAttachement = new TempAttachement();
            //tempAttachement.DocType = e.Files[0].Extension;
            //tempAttachement.FileName = e.Files[0].Name;
            //tempAttachement.UploadedBy = loginState.Username;
            //TempFiles.Add(tempAttachement);
            //await Task.Delay(100);

            //var response = await legalPrincipleService.GetAttachmentDetailForGridByUsingPrincipleId(legalExplanatoryNote.ExplanatoryNoteId);
            //if (response.IsSuccessStatusCode)
            //{
            //    var res = (List<TempAttachement>)response.ResultData;
            //    var result = res.OrderByDescending(x => x.Guid).FirstOrDefault();

            //    dialogService.Close(result.Guid);
            //    //navigationManager.NavigateTo("/add-Legalprinciple/" + result.Guid); // when upload file from machine then not show in grid directly navigate to legal legislation add form.
            //    //ShowResultFileNameInGrid.Add((TempAttachement)response.ResultData);
            //}
            //else
            //{
            //    notificationService.Notify(new NotificationMessage()
            //    {
            //        Severity = NotificationSeverity.Error,
            //        Detail = translationState.Translate("Something_Went_Wrong"),
            //        Style = "position: fixed !important; left: 0; margin: auto; "
            //    });
            //}
            dialogService.Close(legalExplanatoryNote.ExplanatoryNoteId);
            //FileGrid?.Reload();
            //StateHasChanged();
        }

        protected async Task OnErrorHandler(Telerik.Blazor.Components.UploadErrorEventArgs e)
        {
            var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
        }

        protected void RemoveFailedFilesFromList(List<UploadFileInfo> files)
        {
            foreach (var file in files)
            {
                if (FilesValidationInfo.Keys.Contains(file.Id))
                {
                    FilesValidationInfo.Remove(file.Id);
                }
            }
        }

        protected bool IsSelectedFileValid(UploadFileInfo file)
        {
            if (file.InvalidExtension || file.InvalidMaxFileSize || file.InvalidMinFileSize)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Upload_Invalid_Field_Message"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Task.Delay(600);
                return false;
            }
            else
            {
                return true;
            }

        }

        #endregion
    }
}
