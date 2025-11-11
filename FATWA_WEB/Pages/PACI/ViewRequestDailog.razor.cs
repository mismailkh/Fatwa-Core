using FATWA_DOMAIN.Models.ViewModel.PACIVM;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.PACI
{
    public partial class ViewRequestDailog : ComponentBase
    {
        #region Paramters
        [Parameter]
        public dynamic RequestId { get; set; } 
        #endregion

        #region Variable
        protected PACIRequestDataDetailsVM RequestdDataVm = new PACIRequestDataDetailsVM();
        DateTime? formattedBirthDate = null;
        #endregion

        #region AutoComplete

        protected RadzenDataGrid<PACIRequestDataGridVM> grid0;

        #endregion

        #region Load
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        PACIRequestDataDetailsVM _pACIRequestDataVM;
        protected PACIRequestDataDetailsVM pACIRequestDataVM
        {
            get
            {
                return _pACIRequestDataVM;
            }
            set
            {
                if (!object.Equals(_pACIRequestDataVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "pACIRequestDataVM", NewValue = value, OldValue = _pACIRequestDataVM };
                    _pACIRequestDataVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        IEnumerable<PACIRequestDataGridVM> _pACIRequestDataGridVM;
        protected IEnumerable<PACIRequestDataGridVM> pACIRequestDataGridVM
        {
            get
            {
                return _pACIRequestDataGridVM;
            }
            set
            {
                if (!object.Equals(_pACIRequestDataGridVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "pACIRequestDataVM", NewValue = value, OldValue = _pACIRequestDataGridVM };
                    _pACIRequestDataGridVM = value;
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

            await Load();
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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }

        protected async Task Load()
        {
            try
            {
                RequestdDataVm.RequestId = RequestId;
                var result = await pACIRequestService.GetAllPACIRequestData(RequestdDataVm.RequestId);

                if (result.IsSuccessStatusCode)
                {
                    pACIRequestDataVM = (PACIRequestDataDetailsVM)result.ResultData;
                    formattedBirthDate = DateTime.Parse(pACIRequestDataVM?.BirthDate);
                    await PopulateRequestDataGrid(RequestId);

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task PopulateRequestDataGrid(Guid RequestId)
        {
            var historyResponse = await pACIRequestService.GetAllPACIRequestDataGrid(RequestId);
            if (historyResponse.IsSuccessStatusCode)
            {
                pACIRequestDataGridVM = (List<PACIRequestDataGridVM>)historyResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }
        protected async Task GridViewDetailClick(MouseEventArgs args, PACIRequestDataGridVM data)
        {
            var result = await pACIRequestService.GetAllPACIRequestDatawidthCivilId(RequestdDataVm.RequestId, data.Id);
            if (result.IsSuccessStatusCode)
            {
                pACIRequestDataVM = (PACIRequestDataDetailsVM)result.ResultData;

                await PopulateRequestDataGrid(RequestId);

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

        }

        #endregion
    }
}
