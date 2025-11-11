using FATWA_ADMIN.Data;
using FATWA_ADMIN.Services.General;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.UserManagement.Users
{
    public partial class UserTransferDetail : ComponentBase
    {
        [Parameter]
        public string UserId { get; set; }
        
        #region inject Service 
         
        [Inject]
        protected TransferUserService TransferUserService { get; set; } 

        #endregion
         
        #region Variables
        protected RadzenDataGrid<UserTransferVM> grid0;

        public UserTransferVM TransferUserVM = new UserTransferVM();
        #endregion

        public int count { get; set; }

        protected static IEnumerable<UserTransferVM> getUSerGroupResult = new List<UserTransferVM>();



        public IList<UserTransferVM> SelectTransferVM;
        IEnumerable<UserTransferVM> _getUserTransferDetailByIdResult;
        protected IEnumerable<UserTransferVM> getUserTransferDetailByIdResult
        {
            get
            {
                return _getUserTransferDetailByIdResult;
            }
            set
            {
                if (!object.Equals(_getUserTransferDetailByIdResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRolesResult", NewValue = value, OldValue = _getUserTransferDetailByIdResult };
                    _getUserTransferDetailByIdResult = value;

                    Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            //   await  TransferUserService.GetTransferUserById(UserId);
            await Load();
            translationState.TranslateGridFilterLabels(grid0);
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
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }

        protected async Task Load()
        {
            try
            {
                spinnerService.Show();

                getUserTransferDetailByIdResult = await TransferUserService.GetUmsUserTransfer(UserId);

                count = getUserTransferDetailByIdResult.Count();
                await Task.Delay(100);

                await InvokeAsync(StateHasChanged);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            //    try
            //    {
            //        spinnerService.Show();
            //        if (string.IsNullOrEmpty(search))
            //        {
            //            search = "";
            //        }
            //        getUserTransferDetailByIdResult = await TransferUserService.GetTransferUserById(new Query()
            //        {
            //            Filter = $@"i => i.FirstNameEnglish.ToLower().Contains(@0) || i.FirstNameArabic.ToLower().Contains(@1) || i.DepartmentArabic.ToLower().Contains(@2) || i.DepartmentEnglish.ToLower().Contains(@3) || i.UserTypeArabic.ToLower().Contains(@4) || i.UserTypeEnglish.ToLower().Contains(@5) || i.MobileNumber.ToLower().Contains(@6)",
            //            FilterParameters = new object[] { search, search, search, search, search, search, search }
            //        });
            //        count = getUserTransferDetailByIdResult.Count();
            //        spinnerService.Hide();

            //    }
            //    catch (Exception ex)
            //    {
            //        throw new Exception(ex.Message);
            //    }
        }

    }
}
