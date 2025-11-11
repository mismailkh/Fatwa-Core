using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_DOMAIN.Models;
using Microsoft.EntityFrameworkCore;
using FATWA_WEB.Services;
using FATWA_WEB.Data;

namespace FATWA_WEB.Pages
{
    public partial class AddLmsLiteratureComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected LmsLiteratureService FatwaDb { get; set; }

        [Inject]
        protected LmsLiteratureTypeService lmsLiteratureTypesApiService { get; set; }

        [Inject]
        protected LmsLiteratureClassificationService lmsLiteratureClassificationService { get; set; }
        
        IEnumerable<LmsLiteratureType> _getLmsLiteratureTypesForTypeIdResult;
        protected IEnumerable<LmsLiteratureType> getLmsLiteratureTypesForTypeIdResult
        {
            get
            {
                return _getLmsLiteratureTypesForTypeIdResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureTypesForTypeIdResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureTypesForTypeIdResult", NewValue = value, OldValue = _getLmsLiteratureTypesForTypeIdResult };
                    _getLmsLiteratureTypesForTypeIdResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<LmsLiteratureClassification> _getLmsLiteratureClassificationsForClassificationIdResult;
        protected IEnumerable<LmsLiteratureClassification> getLmsLiteratureClassificationsForClassificationIdResult
        {
            get
            {
                return _getLmsLiteratureClassificationsForClassificationIdResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureClassificationsForClassificationIdResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureClassificationsForClassificationIdResult", NewValue = value, OldValue = _getLmsLiteratureClassificationsForClassificationIdResult };
                    _getLmsLiteratureClassificationsForClassificationIdResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        LmsLiterature _lmsliterature;
        protected LmsLiterature lmsliterature
        {
            get
            {
                return _lmsliterature;
            }
            set
            {
                if (!object.Equals(_lmsliterature, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "lmsliterature", NewValue = value, OldValue = _lmsliterature };
                    _lmsliterature = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var fatwaDbGetLmsLiteratureTypesResult = await lmsLiteratureTypesApiService.GetLmsLiteratureTypes();
            getLmsLiteratureTypesForTypeIdResult = fatwaDbGetLmsLiteratureTypesResult;
            var fatwaDbGetLmsLiteratureClassificationsResult = await lmsLiteratureClassificationService.GetLmsLiteratureClassifications();
            getLmsLiteratureClassificationsForClassificationIdResult = fatwaDbGetLmsLiteratureClassificationsResult;
            lmsliterature = new LmsLiterature(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(LmsLiterature args)
        {
            try
            {
                var fatwaDbCreateLmsLiteratureResult = await FatwaDb.CreateLmsLiterature(lmsliterature);
                DialogService.Close(lmsliterature);
            }
            catch (System.Exception fatwaDbCreateLmsLiteratureException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"غير قادر على إنشاء أدب جديد!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
