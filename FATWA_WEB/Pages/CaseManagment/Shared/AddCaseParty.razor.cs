using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-13' Version = "1.0" Branch = "master" >Add case Party</History>
    public partial class AddCaseParty : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic CategoryId { get; set; }
        [Parameter]
        public string ReferenceId { get; set; }
        [Parameter]
        public bool? IsAutoSave { get; set; } = true;
        public int PartyCategoryId { get { return Convert.ToInt32(CategoryId); } set { CategoryId = value; } }

        #endregion

        #region Variables 

        CasePartyLinkVM casePartyLink = new CasePartyLinkVM { Id = Guid.NewGuid() };

        protected string typeValidationMsg = "";
        protected string civilIdValidationMsg = "";

        protected bool ShowIndividualSection { get; set; }
        protected bool ShowCompanySection { get; set; }
        protected bool ShowGovtEntitySection { get; set; }
        protected List<object> CasePartyCategoryOptions { get; set; } = new List<object>();
        protected List<object> CasePartyTypeOptions { get; set; } = new List<object>();
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        protected List<GovernmentEntityRepresentative> GeRepresentatives { get; set; } = new List<GovernmentEntityRepresentative>();
        protected TempAttachementVM uploadedAttachment;
        public class CasePartyCategoryEnumTemp
        {
            public CasePartyCategoryEnum CasePartyCategoryEnumValue { get; set; }
            public string CasePartyCategoryEnumName { get; set; }
        }
        public class CasePartyTypeEnumTemp
        {
            public CasePartyTypeEnum CasePartyTypeEnumValue { get; set; }
            public string CasePartyTypeEnumName { get; set; }
        }

        #endregion

        #region Component Load
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();

                foreach (CasePartyCategoryEnum item in Enum.GetValues(typeof(CasePartyCategoryEnum)))
                {
                    CasePartyCategoryOptions.Add(new CasePartyCategoryEnumTemp { CasePartyCategoryEnumName = translationState.Translate(item.ToString()), CasePartyCategoryEnumValue = item });
                }
                foreach (CasePartyTypeEnum item in Enum.GetValues(typeof(CasePartyTypeEnum)))
                {
                    CasePartyTypeOptions.Add(new CasePartyTypeEnumTemp { CasePartyTypeEnumName = translationState.Translate(item.ToString()), CasePartyTypeEnumValue = item });
                }

                if (PartyCategoryId > 0)
                {
                    casePartyLink.CasePartyCategory = (CasePartyCategoryEnum)PartyCategoryId;
                }
                await PopulateGovernmentEntities();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Dialog Events
        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Submit Form</History>
        //< History Author = 'Aqeel' Date = '2022-09-29' Version = "1.0" Branch = "master" >Fix Civil id issue that is generic but it was not working</History>
        //< History Author = 'Muhammad Zaeem' Date = '2023-12-28' Version = "1.0" Branch = "master" >Change the functionality of auto add parties in add outcome hearing action and also add the flag IsAutoSave</History>
        protected async Task Form0Submit(CasePartyLinkVM args)
        {
            try
            {
                string CivilId = string.Empty;
                if (args.CivilId != null)
                {
                    CivilId = args.CivilId;
                }
                if ((casePartyLink.CasePartyType == CasePartyTypeEnum.Individual) && !IsValidCivilId(CivilId))
                {
                    return;
                }
                if (casePartyLink.CasePartyType > 0)
                {
                    if (await dialogService.Confirm(translationState.Translate("Sure_Add_Party"), translationState.Translate("Submit_Party"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {
                        spinnerService.Show();
                        casePartyLink.ReferenceGuid = Guid.Parse(ReferenceId);
                        if (Convert.ToBoolean(IsAutoSave))
                        {

                            var response = await cmsCaseFileService.CreateCaseParty(casePartyLink);
                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Party_Added_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                await SaveTempAttachementToUploadedDocument();
                                dialogService.Close(casePartyLink);
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                        else
                        {
                            casePartyLink.TypeName_En = translationState.Translate(casePartyLink.CasePartyType.ToString());
                            casePartyLink.TypeName_Ar = translationState.Translate(casePartyLink.CasePartyType.ToString());
                            casePartyLink.CategoryId = (int)casePartyLink.CasePartyCategory;
                            casePartyLink.CategoryName_En = translationState.Translate(casePartyLink.CasePartyCategory.ToString());
                            casePartyLink.CategoryName_Ar = translationState.Translate(casePartyLink.CasePartyCategory.ToString());

                            casePartyLink.TypeId = (int)casePartyLink.CasePartyType;
                            if (casePartyLink.CasePartyType == CasePartyTypeEnum.GovernmentEntity)
                            {
                                casePartyLink.RepresentativeEn = GeRepresentatives.Where(x => x.Id == casePartyLink.RepresentativeId).FirstOrDefault().NameEn;
                                casePartyLink.RepresentativeAr = GeRepresentatives.Where(x => x.Id == casePartyLink.RepresentativeId).FirstOrDefault().NameAr;
                            }
                            casePartyLink.CreatedDate = DateTime.Now;
                            if (uploadedAttachment == null)
                            {
                                casePartyLink.AttachmentCount = 0;
                            }
                            dialogService.Close(casePartyLink);

                        }

                        spinnerService.Hide();
                    }
                }
                else
                {
                    typeValidationMsg = translationState.Translate("Required_Field");
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
        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                List<Guid> partyId = new List<Guid>
                {
                    casePartyLink.Id
                };

                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = partyId,
                    CreatedBy = casePartyLink.CreatedBy,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

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
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        protected async Task AddRepresentative()
        {

            var result = await dialogService.OpenAsync<AddGeRepresentative>(translationState.Translate("Add_Representative"),
            new Dictionary<string, object>()
                {
                    { "GovtEntityId", casePartyLink.EntityId },
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            var geRepresentative = (GovernmentEntityRepresentative)result;
            if (geRepresentative != null)
            {
                await PopulateGeRepresentatives(geRepresentative);
            }
        }

        #endregion

        #region Dropdown Change Events

        //< History Author = 'Hassan Abbas' Date = '2022-09-29' Version = "1.0" Branch = "master" >Render sections basd on party type</History>
        protected async Task OnTypeChange(object args)
        {
            try
            {
                StateHasChanged();
                if ((int)args == 0)
                {
                    ShowIndividualSection = false;
                    ShowCompanySection = false;
                    ShowGovtEntitySection = false;
                    typeValidationMsg = @translationState.Translate("Required_Field");
                    return;
                }
                else
                {
                    typeValidationMsg = "";
                }
                if ((CasePartyTypeEnum)args == CasePartyTypeEnum.Individual)
                {
                    ShowIndividualSection = true;
                    ShowCompanySection = false;
                    ShowGovtEntitySection = false;
                }
                else if ((CasePartyTypeEnum)args == CasePartyTypeEnum.Company)
                {
                    ShowIndividualSection = false;
                    ShowCompanySection = true;
                    ShowGovtEntitySection = false;
                }
                else if ((CasePartyTypeEnum)args == CasePartyTypeEnum.GovernmentEntity)
                {
                    ShowIndividualSection = false;
                    ShowCompanySection = false;
                    ShowGovtEntitySection = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task OnGovtEntityChange(object args)
        {
            try
            {
                if (args != null)
                {
                    var entity = GovtEntities.Where(m => m.EntityId == (int)args).FirstOrDefault();
                    if (entity != null)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            casePartyLink.GovtEntity_En = entity.Name_En;
                        }
                        else
                        {
                            casePartyLink.GovtEntity_Ar = entity.Name_Ar;
                        }
                    }
                }
                else
                {
                    casePartyLink.GovtEntity_En = null;
                    casePartyLink.GovtEntity_Ar = null;
                    casePartyLink.EntityId = 0;
                    casePartyLink.RepresentativeId = Guid.Empty;
                }
                await PopulateGeRepresentatives(null);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Templates</History>
        protected async Task PopulateGovernmentEntities()
        {
            var govtEntityResponse = await lookupService.GetGovernmentEntities();
            if (govtEntityResponse.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)govtEntityResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(govtEntityResponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Ge Representatives</History>
        protected async Task PopulateGeRepresentatives(GovernmentEntityRepresentative? geRepresentative)
        {
            if (geRepresentative != null)
            {
                casePartyLink.RepresentativeId = geRepresentative.Id;
                casePartyLink.EntityId = geRepresentative.GovtEntityId;
            }
            if (casePartyLink.EntityId != null)
            {
                var govtEntityResponse = await lookupService.GetGeRepresentatives((int)casePartyLink.EntityId);
                if (govtEntityResponse.IsSuccessStatusCode)
                {
                    GeRepresentatives = (List<GovernmentEntityRepresentative>)govtEntityResponse.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(govtEntityResponse);
                }
                StateHasChanged();
            }
            else
            {
                GeRepresentatives = new List<GovernmentEntityRepresentative>();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-12-30' Version="1.0" Branch="master">Change Civil Id</History>
        //<History Author = 'Aqeel' Date='2023-10-01' Version="1.0" Branch="master">Fixed wrong assignment</History>

        protected async Task OnChangeCivilId(string civilId)
        {
            if (civilId.Length == 12)
            {
                bool isValid = IsValidCivilId(civilId);
                civilIdValidationMsg = isValid ? "" : translationState.Translate("Enter_Valid_CivilId");
            }
        }

        #endregion

        #region Add Authority Letter

        //<History Author = 'Hassan Abbas' Date='2022-10-13' Version="1.0" Branch="master"> Open a pupup for uploading document with respect to parameters</History>
        protected async Task AddDocument(AttachmentTypeEnum attachmentType)
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
            new Dictionary<string, object>()
                {
                    { "ReferenceGuid", casePartyLink.Id },
                    { "IsViewOnly", false },
                    { "IsUploadPopup", true },
                    { "FileTypes", systemSettingState.FileTypes },
                    { "MaxFileSize", systemSettingState.File_Maximum_Size },
                    { "Multiple", false },
                    { "UploadFrom", "CaseManagement" },
                    { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                    { "MandatoryAttachmentTypeId", (int)attachmentType },
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            uploadedAttachment = (TempAttachementVM)result;
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-13' Version="1.0" Branch="master"> Remove Uplaoded Authority Letter</History>
        protected async Task RemoveDocument()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (uploadedAttachment == null)
                    return;

                result = await fileUploadService.RemoveTempAttachement(uploadedAttachment?.FileName, uploadedAttachment?.UploadedBy, "CaseManagement", "G2G_WEB", uploadedAttachment.AttachmentTypeId);
                if (result)
                {
                    uploadedAttachment = null;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        private async Task OnAttachmentClick(AttachmentTypeEnum attachmentType)
        {
            if (uploadedAttachment != null)
            {
                await RemoveDocument();
            }
            else
            {
                await AddDocument(attachmentType);
            }
            StateHasChanged();
        }
        #endregion

        #region Validate CivilId Pattern

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
    }
}
