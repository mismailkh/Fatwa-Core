using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using Syncfusion.Blazor.DocumentEditor;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_DOMAIN.Models;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using System.Text.Json;
using System.Text;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using Syncfusion.Blazor.RichTextEditor;
using DocumentFormat.OpenXml.Spreadsheet;

namespace DMS_WEB.Pages.DocumentManagement
{
    public partial class SaveDraftStamp : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic? TemplateId { get; set; }
        public int DocumentTemplateId { get { return Convert.ToInt32(TemplateId); } set { TemplateId = value; } }
        #endregion
        #region Variable
        protected CmsDraftStamp Template { get; set; } = new CmsDraftStamp();
        protected bool IsHeaderFooterTemplate { get; set; } = true;
        public int ModuleId { get; set; }
        internal string DocumentName { get; set; }
        protected List<CaseTemplateParameter> Parameters { get; set; } = new List<CaseTemplateParameter>();
        #endregion
        #region Compnent Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (DocumentTemplateId == 0 || DocumentTemplateId > (int)CaseTemplateEnum.HeaderAr)
            {
                IsHeaderFooterTemplate = false;
            }
            if (DocumentTemplateId > 0)
            {
                await PopulateTemplateParameters();
            }
            spinnerService.Hide();
        }

        #endregion
        protected async Task PopulateTemplateParameters()
        {
            var response = await fileUploadService.GetTemplateParameters(null);
            if (response.IsSuccessStatusCode)
            {
                Parameters = (List<CaseTemplateParameter>)response.ResultData;
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #region Events
        protected async Task SubmitTemplate()
        {
            try
            {
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
                    Template.Content = Template.Content.Replace("<div class=\"Section0\">", "<div class=\"Section0\" style=\"margin: 72px;\">");

                    await FillAndAdjustTemplateLabels();
                }

                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await fileUploadService.SaveDraftStamp(Template);
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
                        await ReturnBadRequestNotification(response);
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

        #region Syncfusion Word Processor
        protected List<object> ToolBarItems = new List<object>() { "New", "Separator", "Undo", "Redo", "Separator", "PageNumber", "Break", "Separator", "Table", "Find", "Separator", "TrackChanges", "Separator", "LocalClipboard" };
        SfDocumentEditorContainer container;
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
        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Badrequest Notiication

        //<History Author = 'Noman Khan' Date='2024-03-26' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
                {
                    var badRequestResponse = (BadRequestResponse)response.ResultData;
                    if (badRequestResponse.InnerException != null && badRequestResponse.InnerException.ToLower().Contains("violation of unique key"))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Role_Name_Exists"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
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
    }
}
