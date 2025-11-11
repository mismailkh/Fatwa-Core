using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.ContactManagment
{
    public partial class AddDesignation : ComponentBase
	{
		#region Constructor
		public AddDesignation()
		{
            Designations = new Designation();
		}
		#endregion

		#region Variables Declaration
		public Designation Designations { get; set; }
		#endregion

		#region Component Events

		protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();

                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

		#region Form submit
		protected async Task Form0Submit(Designation args)
        {
			try
			{
				if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
				{
					var response = await lookupService.AddNewDesignation(args);

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
