using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class ListNeedForMoreInfomation : ComponentBase
    {
        [Parameter]
        public string ReferenceId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }

        #region Variables
        public int SubModuleid { get { return Convert.ToInt32(SubModuleId); } set { SubModuleId = value; } }
        protected RadzenDataGrid<CommunicationListVM>? communicationListgrid = new RadzenDataGrid<CommunicationListVM>();
        public int SectorTypeid { get; set; } = 0;
        public IEnumerable<CommunicationListVM> FiletergetCommunicationList = new List<CommunicationListVM>();
		protected IEnumerable<CommunicationListVM> getCommunicationList { get; set; }
		protected RadzenDataGrid<CommunicationListVM> grid = new RadzenDataGrid<CommunicationListVM>();
		public int count { get; set; } = 0;
		string _search;
		
        string redirectUrl;

		#endregion
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
					OnPropertyChanged(args);
					Reload();
				}
			}
		}
		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}
		#region Component Load
		protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            SectorTypeid = int.Parse(SectorTypeId);
            await Load();
            translationState.TranslateGridFilterLabels(communicationListgrid);
            spinnerService.Hide();
        }
        #endregion

        #region Grid Events

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }


        protected async Task Load()
        {
            try
            {
				if (string.IsNullOrEmpty(search))
				{
					search = "";
				}
				else
				{
					search = search.ToLower();
				}
				if (SubModuleid == (int)SubModuleEnum.CaseFile)
                {
                    redirectUrl = "/casefile-view/" + ReferenceId + "";
                    await GetCommunicationListByCaseFileId();
                }
                else if (SubModuleid == (int)SubModuleEnum.RegisteredCase)
                {
                    redirectUrl = "/case-view/" + ReferenceId + "";
                    await GetCommunicationListByRegisteredCaseId();
                }
                else if (SubModuleid == (int)SubModuleEnum.ConsultationFile)
                {
                    redirectUrl = "/consultationfile-view/" + ReferenceId + "/" + SectorTypeid;
                    await PopulateConslutationFileCommunicationGrid();
                }

                await communicationListgrid.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region view response
        protected async Task ViewResponse(CommunicationListVM item)
        {
            var RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
            navigationManager.NavigateTo(RedirectURL);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected void NeedMoreInfo(MouseEventArgs args)
        {
            if (SubModuleid == (int)SubModuleEnum.CaseFile)
            {
                navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + false);
            } else if (SubModuleid == (int)SubModuleEnum.RegisteredCase)
            {
                navigationManager.NavigateTo("/Case-Request-For-More-Information/" + ReferenceId);
            }
        }

        protected void SendReminder(CommunicationListVM args)
        {
            if (SubModuleid == (int)SubModuleEnum.CaseFile)
            {
                navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + false + "/" + args.CommunicationId + "/" + true);
            }
            else if (SubModuleid == (int)SubModuleEnum.RegisteredCase)
            {
                navigationManager.NavigateTo("/Case-Request-For-More-Information/" + ReferenceId);
            }
            else if (SubModuleid == (int)SubModuleEnum.ConsultationFile)
            {
                navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ReferenceId + "/" + args.CommunicationId + "/" + true);
            }
        }
        #endregion
        protected async Task GetCommunicationListByCaseFileId()
        {
            var response = await communicationService.GetCommunicationListByCaseFileId(Guid.Parse(ReferenceId), (int)CommunicationCorrespondenceTypeEnum.Outbox);
            if (response.IsSuccessStatusCode)
            {
				getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
				FiletergetCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
				count = getCommunicationList.Count();
				await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task GetCommunicationListByRegisteredCaseId()
        {
            try
            {
                var response = await communicationService.GetCommunicationListByCaseId(Guid.Parse(ReferenceId), (int)CommunicationCorrespondenceTypeEnum.Outbox);
                if (response.IsSuccessStatusCode)
                {

					getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
					FiletergetCommunicationList = (List<CommunicationListVM>)response.ResultData;
					count = getCommunicationList.Count();

				}
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateConslutationFileCommunicationGrid()
        {
            var response = await communicationService.GetConslutationFileCommunication(Guid.Parse(ReferenceId), (int)CommunicationCorrespondenceTypeEnum.Outbox);
            if (response.IsSuccessStatusCode)
            {

				getCommunicationList = (IEnumerable<CommunicationListVM>)response.ResultData;
				FiletergetCommunicationList = (List<CommunicationListVM>)response.ResultData;
				count = getCommunicationList.Count();

			}
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }


        protected void RequestMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ReferenceId + "/" + false);
        }
        protected void FinalDraft(MouseEventArgs args)
        { 
            navigationManager.NavigateTo("/Coms-Request-For-More-Information-Final/" + ReferenceId + "/" + false + "/" + true);
        }

        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                {
                    search = search.ToLower();
                }
				FiletergetCommunicationList = await gridSearchExtension.Filter(getCommunicationList, new Query()
                {
					Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => ( (i.ReferenceNo != null && i.ReferenceNo.ToLower().Contains(@0))
                                                                                          || (i.OutboxNumber != null && i.OutboxNumber.ToLower().Contains(@1)) 
                                                                                          || (i.CorrespondenceTypeEn != null && i.CorrespondenceTypeEn.ToLower().Contains(@2)) 
                                                                                          || (i.SectorNameEn!=null && i.SectorNameEn.ToLower().Contains(@3))
                                                                                          || (i.Activity_En!=null && i.Activity_En.ToLower().Contains(@4)) )" :
					                                                               $@"i => ( (i.ReferenceNo != null && i.ReferenceNo.ToLower().Contains(@0))
                                                                                          || (i.OutboxNumber != null && i.OutboxNumber.ToLower().Contains(@1)) 
                                                                                          || (i.CorrespondenceTypeAr != null && i.CorrespondenceTypeAr.ToLower().Contains(@2)) 
                                                                                          || (i.SectorNameAr!=null && i.SectorNameAr.ToLower().Contains(@3))
                                                                                          || (i.Activity_Ar!=null && i.Activity_Ar.ToLower().Contains(@4)) )" ,
					
					FilterParameters = new object[] { search, search, search,search,search}
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
