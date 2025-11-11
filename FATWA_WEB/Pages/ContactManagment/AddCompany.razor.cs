using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class AddCompany : ComponentBase
	{
		#region Constructor
		public AddCompany()
		{
            Companys = new Company();
            CityList = new List<City>();


		}
		#endregion

		#region Variables Declaration
		public Company Companys { get; set; }
        public List<City> CityList { get; set; }
		#endregion

		#region Component Events

		protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();

                await PopulateDropdowns();

                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Populate Dropdowns records
        protected async Task PopulateDropdowns()
        {
            try
            {
                await GetCityList();
            }
            catch (Exception)
            {
                throw;
            }

        }

		private async Task GetCityList()
		{
            try
            {
				var response = await lookupService.GetCityList();
				if (response.IsSuccessStatusCode)
				{
					CityList = (List<City>)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
			}
            catch (Exception)
            {
				throw new NotImplementedException();
			}	
		}

		#endregion

		#region Form submit
		protected async Task Form0Submit(Company args)
        {
			try
			{
				if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
				{
					var response = await lookupService.AddNewCompany(args);

					if (response.IsSuccessStatusCode)
					{
						dialogService.Close(args);
					}
                    else
                    {
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Error,
							Detail = translationState.Translate("Something_Went_Wrong"),
							//Summary = translationState.Translate("Error"),
							Style = "position: fixed !important; left: 0; margin: auto; "
						});
						return;
                    }
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

        #endregion

        #region Form closed
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

    }
}
