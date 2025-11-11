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
using FATWA_WEB.Pages;

namespace FATWA_WEB.Pages
{
    public partial class UsersComponent : ComponentBase
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
        protected TooltipService TooltipService { get; set; }
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }
        [Inject]
        protected DialogService dialogService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService notificationService { get; set; }
 
          

        // 
        //protected FatwaDbService FatwaDb { get; set; }
        protected RadzenDataGrid<FATWA_DOMAIN.Models.AdminModels.UserManagement.User> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                //if (!object.Equals(_search, value))
                //{
                //    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                //    _search = value;
                //    OnPropertyChanged(args);
                //    Reload();
                //}
            }
        }

        IEnumerable<FATWA_DOMAIN.Models.AdminModels.UserManagement.User> _getUsersResult;
        protected IEnumerable<FATWA_DOMAIN.Models.AdminModels.UserManagement.User> getUsersResult
        {
            get
            {
                return _getUsersResult;
            }
            set
            {
                //if (!object.Equals(_getUsersResult, value))
                //{
                //    var args = new PropertyChangedEventArgs(){ Name = "getUsersResult", NewValue = value, OldValue = _getUsersResult };
                //    _getUsersResult = value;
                //    OnPropertyChanged(args);
                //    Reload();
                //}
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            //var fatwaDbGetUsersResult = await FatwaDb.GetUsers(new Query() { Filter = $@"i => i.Id.Contains(@0) || i.UserName.Contains(@1) || i.NormalizedUserName.Contains(@2) || i.Email.Contains(@3) || i.NormalizedEmail.Contains(@4) || i.PasswordHash.Contains(@5) || i.SecurityStamp.Contains(@6) || i.ConcurrencyStamp.Contains(@7) || i.PhoneNumber.Contains(@8)", FilterParameters = new object[] { search, search, search, search, search, search, search, search, search } });
            //getUsersResult = fatwaDbGetUsersResult;
        }

        protected async Task Button0Click(MouseEventArgs args)
        {
           
            var dialogResult = await dialogService.OpenAsync<AddUser>(
                "إضافة المستخدم",
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            //if (args?.Value == "csv")
            //{
            //    await FatwaDb.ExportUsersToCSV(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount" }, $"Users");

            //}

            //if (args == null || args.Value == "xlsx")
            //{
            //    await FatwaDb.ExportUsersToExcel(new Query() { Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "Id,UserName,NormalizedUserName,Email,NormalizedEmail,EmailConfirmed,PasswordHash,SecurityStamp,ConcurrencyStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEnd,LockoutEnabled,AccessFailedCount" }, $"Users");

            //}
        }

        protected async Task Grid0RowSelect(FATWA_DOMAIN.Models.AdminModels.UserManagement.User args)
        {
            //var dialogResult = await dialogService.OpenAsync<EditUser>("Edit User", new Dictionary<string, object>() { {"Id", args.Id} });
            //await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                //if (await dialogService.Confirm("Are you sure you want to delete this record?") == true)
                //{
                //    var fatwaDbDeleteUserResult = await FatwaDb.DeleteUser($"{data.Id}");
                //    if (fatwaDbDeleteUserResult != null)
                //    {
                //        await grid0.Reload();
                //    }
                //}
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                { 
                    Severity = NotificationSeverity.Error,
                    Summary = $"خطأ!",
                    Style = "position: fixed !important; left: 0; margin: auto; ", 
                    Detail = $"تعذر حذف المستخدم!"
                });
            }
        }
    }
}
