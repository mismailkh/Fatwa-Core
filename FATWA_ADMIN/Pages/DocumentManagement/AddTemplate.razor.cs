using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.DocumentEditor;
using Syncfusion.Blazor.RichTextEditor;
using System.Text;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_ADMIN.Pages.DocumentManagement
{
    //< History Author = 'Hassan Abbas' Date = '2023-08-29' Version = "1.0" Branch = "master">
    //Add/Edit Template for LOBs,
    //component fot adding new templates with abilility to add labels as #Keys# which can be replaced dynamically with relevent content which cannot be modified,
    //and to manage the header and footers for draft documents
    //</History>
    public partial class AddTemplate : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic? TemplateId { get; set; }
        public int DocumentTemplateId { get { return Convert.ToInt32(TemplateId); } set { TemplateId = value; } }
        #endregion

        #region Variables
        public int ModuleId { get; set; }
        internal string DocumentName { get; set; }
        protected bool IsHeaderFooterTemplate { get; set; } = true;
        public List<ModuleEnumTemp> Modules { get; set; } = new List<ModuleEnumTemp>();

        public class ModuleEnumTemp
        {
            public int ModuleEnumValue { get; set; }
            public string ModuleEnumName { get; set; }
        }
        public List<Subtype> subTypeDetails { get; set; } = new List<Subtype>();
        protected List<CaseTemplateParameter> Parameters { get; set; } = new List<CaseTemplateParameter>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected List<AttachmentType> AllAttachmentTypes { get; set; } = new List<AttachmentType>();
        protected CaseTemplate Template { get; set; } = new CaseTemplate();
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; } = new List<CaseTemplate>();
        #endregion

        #region Compnent Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();  
            if (DocumentTemplateId == 0 || DocumentTemplateId > (int)CaseTemplateEnum.HeaderAr)
            {
                IsHeaderFooterTemplate = false;
            }
            await PopulateModules();
            await PopulateAllAttachmentTypes();
            await PopulateHeaderFooterTemplates();
            if (DocumentTemplateId > 0)
            {
                await PopulateTemplateDetails();
                await PopulateTemplateParameters();
            }
            spinnerService.Hide();
        }

        #endregion

        #region Dropdown Events

        //< History Author = 'Hassan Abbas' Date = '2023-07-11' Version = "1.0" Branch = "master" >Populate Modules</History>
        protected async Task PopulateModules()
        {
            try
            {
                foreach (WorkflowModuleEnum item in Enum.GetValues(typeof(WorkflowModuleEnum)))
                {
                    Modules.Add(new ModuleEnumTemp { ModuleEnumName = translationState.Translate(item.ToString()), ModuleEnumValue = (int)item });
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

        //< History Author = 'Hassan Abbas' Date = '2023-07-11' Version = "1.0" Branch = "master" >Populate SubModules</History>
        protected async Task OnModuleChange(object moduleId)
        {
            try
            {
                if (moduleId != null && (int)moduleId > 0)
                {
                    await PopulateAttachmentTypes((int)moduleId);
                }
                else
                {
                    AttachmentTypes = new List<AttachmentType>();
                    Template.AttachmentTypeId = 0;
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

        protected async Task SetEditorContextMenu(object moduleId)
        {
            try
            {
                if (container != null && container.DocumentEditor != null)
                {
                    SfDocumentEditor documentEditor = container.DocumentEditor;
                    List<MenuItemModel> contentMenuItem = new List<MenuItemModel>();
                    contentMenuItem = new List<MenuItemModel>
                    {
                        new MenuItemModel { Text = translationState.Translate("Add_Labels") , Id= "Add_Labels", IconCss="e-icons e-description" },
                        new MenuItemModel { Text = translationState.Translate("Mark_Read_Only"), Id= "Mark_Read_Only", IconCss="e-icons e-recurrence-edit" }
                    };
                    documentEditor.ContextMenu.AddCustomMenu(contentMenuItem, false, false);
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
        //<History Author = 'Hassan Abbas' Date='2022-11-14' Version="1.0" Branch="master">Populate Attachment Types</History>
        protected async Task PopulateAttachmentTypes(int moduleId)
        {
            var response = await fileUploadService.GetAttachmentTypesByModuleId(moduleId, true);
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
                //if ((int)moduleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                //{
                //    var resultsubtypes = await GetRequestSubtypesByRequestId((int)RequestTypeEnum.Contracts);
                //    if (resultsubtypes.Count() != 0)
                //    {
                //        AttachmentTypes = AttachmentTypes.Where(x => resultsubtypes.Any(y => y.Id == (int?)x.SubTypeId)).ToList();
                //    }
                //}
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-14' Version="1.0" Branch="master">Populate Attachment Types</History>
        protected async Task PopulateAllAttachmentTypes()
        {
            var response = await fileUploadService.GetAllAttachmentTypes();
            if (response.IsSuccessStatusCode)
            {
                AllAttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master">Populate Header Footer Templates</History>
        protected async Task PopulateHeaderFooterTemplates()
        {
            var response = await fileUploadService.GetHeaderFooterTemplates();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-08-21' Version="1.0" Branch="master">Populate Parameters</History>
        protected async Task PopulateTemplateParameters()
        {
            var response = await fileUploadService.GetTemplateParameters(null);
            if (response.IsSuccessStatusCode)
            {
                Parameters = (List<CaseTemplateParameter>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2023-08-24' Version="1.0" Branch="master">Populate Case Template Content</History>
        protected async Task PopulateTemplateDetails()
        {
            if (DocumentTemplateId > 0)
            {
                var response = await fileUploadService.GetCaseTemplate(DocumentTemplateId);
                if (response.IsSuccessStatusCode)
                {
                    Template = (CaseTemplate)response.ResultData;
                    if (Template.AttachmentTypeId > 0)
                    {
                        ModuleId = AllAttachmentTypes.Where(x => x.AttachmentTypeId == Template.AttachmentTypeId).First().ModuleId != null ? (int)AllAttachmentTypes.Where(x => x.AttachmentTypeId == Template.AttachmentTypeId).First().ModuleId : 0;
                        await OnModuleChange(ModuleId);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            StateHasChanged();
        }
        #endregion

        #region Syncfusion Word Processor 
        protected List<object> ToolBarItems = new List<object>() { "New", "Separator", new CustomToolbarItemModel() { Id = "RTL", Text = "Right to Left", PrefixIcon = "e-icons e-de-e-paragraph-mark" }, "Separator", "Undo", "Redo", "Separator", "PageNumber", "Break", "Separator", "Table", "Find", "Separator", "TrackChanges", "Separator", "LocalClipboard" };
        SfDocumentEditorContainer container;

        public void ToolbarClickHandler(ClickEventArgs args)
        {
            if (args.Item.Id == "RTL")
            {
                container.DocumentEditor.Selection.ParagraphFormat.SetBidiAsync(true);
                container.DocumentEditor.Selection.ParagraphFormat.SetTextAlignmentAsync(TextAlignment.Right);
            }
        }
        public void OnExport(object args)
        {
            SfDocumentEditor documentEditor = container.DocumentEditor;
            documentEditor.SaveAsync(DocumentName, Syncfusion.Blazor.DocumentEditor.FormatType.Docx);
        }
        public void Print(object args)
        {
            SfDocumentEditor documentEditor = container.DocumentEditor;
            documentEditor.PrintAsync();
        }

        DocumentEditorSettingsModel settings = new DocumentEditorSettingsModel()
        {
            FontFamilies = new string[2] { "Sultan", "Sultan Medium" }
        };
        public void OnCreated(object args)
        {
            if (DocumentTemplateId > 0 && !String.IsNullOrWhiteSpace(Template.Content))
            {
                Template.Content = Template.Content.Replace("<div class=\"Section0\" style=\"margin: 72px;", "<div class=\"Section0\" style=\"");
                byte[] htmlBytes = Encoding.UTF8.GetBytes(Template.Content);
                string base64String = Convert.ToBase64String(htmlBytes);
                byte[] byteArray = Convert.FromBase64String(base64String);
                Stream stream = new MemoryStream(byteArray);

                if (Thread.CurrentThread.CurrentUICulture.Name == "ar-KW")
                {
                    container.DocumentEditor.Selection.ParagraphFormat.SetBidiAsync(true);
                    container.DocumentEditor.Selection.ParagraphFormat.SetTextAlignmentAsync(TextAlignment.Right);
                }

                container.DocumentEditor.OpenAsync(stream, ImportFormatType.Html);
            }
            else
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "ar-KW")
                {
                    container.DocumentEditor.Selection.ParagraphFormat.SetBidiAsync(true);
                    container.DocumentEditor.Selection.ParagraphFormat.SetTextAlignmentAsync(TextAlignment.Right);
                }
            }
            List<MenuItemModel> contentMenuItem = new List<MenuItemModel>();
            contentMenuItem = new List<MenuItemModel>
            {
                new MenuItemModel { Text = translationState.Translate("Add_Labels"), Id= "Add_Labels", IconCss="e-icons e-description" },
                new MenuItemModel { Text = translationState.Translate("Mark_Read_Only"), Id= "Mark_Read_Only", IconCss="e-icons e-recurrence-edit" }
            };
            container.DocumentEditor.ContextMenu.AddCustomMenu(contentMenuItem, false, false);
            container.DocumentEditor.Selection.SectionFormat.SetPageWidthAsync(595);
            container.DocumentEditor.Selection.SectionFormat.SetPageHeightAsync(841);
            container.DocumentEditor.Selection.CharacterFormat.SetFontFamilyAsync("Sultan"); 
        } 
        public void OnDocumentChange()
        {
            string name = container.DocumentEditor.DocumentName;
            if (name != "")
            {
                DocumentName = name;
            }
        }
        public async Task OnContentMenuSelect(CustomContentMenuEventArgs args)
        {
            if (args.Id.EndsWith("Add_Labels"))
            {
                if (ModuleId > 0)
                {
                    var result = await dialogService.OpenAsync<ParameterSelectionPopup>(translationState.Translate("Add_Labels"),
                    new Dictionary<string, object>()
                    {
                    { "ModuleId", ModuleId },
                    }
                    ,
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });
                    var parameter = (CaseTemplateParameter)result;
                    if (parameter != null)
                    {
                        await container.DocumentEditor.Editor.InsertTextAsync("#" + parameter.PKey + "#");
                        Template.Parameters.Add(parameter);
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Must_Select_Module"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    return;
                }
            }
            else if (args.Id.EndsWith("Mark_Read_Only"))
            {
                string selectedText = await container.DocumentEditor.Selection.GetTextAsync();
                selectedText = selectedText.Replace("[----", "");
                selectedText = selectedText.Replace("----]", "");
                if (selectedText != null)
                {
                    await container.DocumentEditor.Editor.InsertTextAsync("[----" + selectedText + "----]");
                }
            }
        }
        #endregion

        #region Get Request Subtypes By RequestId
        private async Task<List<Subtype>> GetRequestSubtypesByRequestId(int contracts)
        {
            try
            {
                var response = await fileUploadService.GetRequestSubtypesByRequestId(contracts);
                if (response.IsSuccessStatusCode)
                {
                    subTypeDetails = (List<Subtype>)response.ResultData;
                    return subTypeDetails;
                }
                return new List<Subtype>();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }
        #endregion

        #region Syncfusion Rich Editor

        private List<ImageToolbarItemModel> ImageTools = new List<ImageToolbarItemModel>()
{
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Replace },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Align },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Caption },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Remove },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.HorizontalSeparator },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.InsertLink },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Display },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.AltText },
        new ImageToolbarItemModel() { Command = ImageToolbarCommand.Dimension }
    };


        private List<ToolbarItemModel> Tools = new List<ToolbarItemModel>()
    {
        new ToolbarItemModel() { Command = ToolbarCommand.Bold },
        new ToolbarItemModel() { Command = ToolbarCommand.Italic },
        new ToolbarItemModel() { Command = ToolbarCommand.Underline },
        new ToolbarItemModel() { Command = ToolbarCommand.StrikeThrough },
        new ToolbarItemModel() { Command = ToolbarCommand.SuperScript },
        new ToolbarItemModel() { Command = ToolbarCommand.SubScript },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.FontName },
        new ToolbarItemModel() { Command = ToolbarCommand.FontSize },
        new ToolbarItemModel() { Command = ToolbarCommand.FontColor },
        new ToolbarItemModel() { Command = ToolbarCommand.BackgroundColor },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.LowerCase },
        new ToolbarItemModel() { Command = ToolbarCommand.UpperCase },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Formats },
        new ToolbarItemModel() { Command = ToolbarCommand.Alignments },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.NumberFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.BulletFormatList },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Image },
        new ToolbarItemModel() { Command = ToolbarCommand.CreateTable },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.ClearFormat },
        new ToolbarItemModel() { Command = ToolbarCommand.Print },
        new ToolbarItemModel() { Command = ToolbarCommand.FullScreen },
        new ToolbarItemModel() { Command = ToolbarCommand.Separator },
        new ToolbarItemModel() { Command = ToolbarCommand.Undo },
        new ToolbarItemModel() { Command = ToolbarCommand.Redo }
    };
        #endregion

        #region Form Events

        //< History Author = 'Hassan Abbas' Date = '2023-08-20' Version = "1.0" Branch = "master" >Submit Template</History>
        protected async Task SubmitTemplate()
        {
            try
            {
                if (((DocumentTemplateId == 0 || DocumentTemplateId > (int)CaseTemplateEnum.HeaderAr) && Template.AttachmentTypeId <= 0) || String.IsNullOrWhiteSpace(Template.NameEn) || String.IsNullOrWhiteSpace(Template.NameAr))
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    return;
                }
                if (container != null)
                {
                    var allText = container.DocumentEditor.Selection.SelectAllAsync();
                    string selectedText = await container.DocumentEditor.Selection.GetTextAsync();
                    string selectedContent = await container.DocumentEditor.SerializeAsync();
                    if (string.IsNullOrWhiteSpace(selectedText))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Content"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        return;
                    }
                    string base64Data = await container.DocumentEditor.SaveAsBlobAsync(Syncfusion.Blazor.DocumentEditor.FormatType.Docx);
                    byte[] data = Convert.FromBase64String(base64Data);
                    Stream stream = new MemoryStream(data);
                    Syncfusion.DocIO.DLS.WordDocument document = new Syncfusion.DocIO.DLS.WordDocument(stream, Syncfusion.DocIO.FormatType.Docx);
                    using (MemoryStream htmlStream = new MemoryStream())
                    {
                        document.Save(htmlStream, Syncfusion.DocIO.FormatType.Html);
                        document.Close();
                        htmlStream.Position = 0;
                        using (StreamReader reader = new StreamReader(htmlStream))
                        {
                            Template.Content = reader.ReadToEnd();
                        }
                    }
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        Template.Content = Template.Content.Replace("<div class=\"Section0\">", "<div class=\"Section0\" style=\"margin: 72px;\">");
                    }
                    else
                    {
                        Template.Content = Template.Content.Replace("<div class=\"Section0\">", "<div class=\"Section0\" style=\"margin: 72px;direction: rtl;\">");
                    }
                    await FillAndAdjustTemplateLabels();
                }

                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await fileUploadService.SaveCaseTemplate(Template);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        navigationManager.NavigateTo("template-list");
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
        private async Task FillAndAdjustTemplateLabels()
        {
            try
            {
                List<string> labels = new List<string>();
                string pattern = @"#([a-zA-Z]+)#";
                MatchCollection matches = Regex.Matches(Template.Content, pattern);
                foreach (Match match in matches)
                {
                    string label = match.Groups[1].Value;
                    //Template.Content = Template.Content.Replace("#" + label + "#", "<span class=\"temp-label-key\">" + "#" + label + "#" + "</span>");
                    if (DocumentTemplateId > 0)
                    {
                        CaseTemplateParameter parameter = Parameters.Where(p => p.PKey.ToLower() == label.ToLower()).FirstOrDefault();
                        if (parameter != null && !Template.Parameters.Any(p => p.ParameterId == parameter.ParameterId))
                        {
                            Template.Parameters.Add(parameter);
                        }
                    }
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

        #region Redirect Function

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        protected void EditHeader()
        {
            int headerId = HeaderFooterTemplates.Any() ? HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Id).FirstOrDefault() : 0;
            navigationManager.NavigateTo("/template-add/" + headerId, true);
        }
        protected void EditFooter()
        {
            int footerId = HeaderFooterTemplates.Any() ? HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Id).FirstOrDefault() : 0;
            navigationManager.NavigateTo("/template-add/" + footerId, true);
        }
        protected async Task RedirectBack()
        {
            await jSRuntime.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
    }
}
