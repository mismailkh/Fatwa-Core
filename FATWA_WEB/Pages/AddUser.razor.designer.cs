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
using System.ComponentModel;
using FATWA_WEB.Services;

namespace FATWA_WEB.Pages
{
    public partial class AddUserComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(FATWA_WEB.Services.PropertyChangedEventArgs args)
        {
        }


        [Inject]         
        protected TooltipService TooltipService { get; set; }
        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService dialogService { get; set; }


        [Inject]
        protected NotificationService notificationService { get; set; }



        // 
        //protected FatwaDbService FatwaDb { get; set; }

        FATWA_DOMAIN.Models.AdminModels.UserManagement.User _user;
        protected FATWA_DOMAIN.Models.AdminModels.UserManagement.User user
        {
            get
            {
                return _user;
            }
            set
            {
                if (!object.Equals(_user, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "user", NewValue = value, OldValue = _user };
                    _user = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            user = new FATWA_DOMAIN.Models.AdminModels.UserManagement.User(){};
        }

        protected async Task Form0Submit(FATWA_DOMAIN.Models.AdminModels.UserManagement.User args)
        {
            try
            {
                //var fatwaDbCreateUserResult = await FatwaDb.CreateUser(user);
                //dialogService.Close(user);
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"خطأ!",
                    Detail = $"غير قادر على إنشاء مستخدم جديد!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
