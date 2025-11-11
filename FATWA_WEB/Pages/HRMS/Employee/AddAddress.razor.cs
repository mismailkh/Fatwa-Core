using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_WEB.Pages.CreateUser;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class AddAddress : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic>? Attributes { get; set; }
        UserAdress UserAddress = new UserAdress();
        protected IEnumerable<City> Cities { get; set; }

        City City { get; set; } = new City
        {
            Governorate = new Governorate()
            {
                Country = new Country()
            }
        };
        protected override async void OnInitialized()
        {
            spinnerService.Show();
            LoadAddresses();
            await GetCities();
            await GetCityDetail();
            spinnerService.Hide();
        }

        public void LoadAddresses()
        {

            if (Attributes != null)
            {
                if (Attributes.ContainsKey("userId"))
                {
                    UserAddress.UserId = Attributes["userId"];
                }
                if (Attributes.ContainsKey("AddressId"))
                {
                    UserAddress.AddressId = Attributes["AddressId"];
                }

                if (Attributes.ContainsKey("Address"))
                {
                    UserAddress.Address = Attributes["Address"];

                }
                if (Attributes.ContainsKey("CityId"))
                {
                    UserAddress.CityId = Attributes["CityId"];
                }
            }

        }
        protected async Task GetCities()
        {
            var response = await userService.GetCities();
            if (response.IsSuccessStatusCode)

            {
                Cities = (IEnumerable<City>)response.ResultData;
            }
            else
            {
                /*  await invalidRequestHandlerService.ReturnBadRequestNotification(response);*/
            }
            StateHasChanged();
        }

        public void LoadEditAddress()
        {
            if (Attributes.ContainsKey("AddressId") && Attributes["AddressId"] != null)
            {

                UserAddress.AddressId = Attributes.ContainsKey("AddressId") ? (Guid)Attributes["AddressId"] : Guid.Empty;
                UserAddress.Address = Attributes.ContainsKey("Address") ? (string)Attributes["Address"] : null;
                UserAddress.CityId = Attributes.ContainsKey("CityId") ? (int)Attributes["CityId"] : 0;
            }
        }

        protected async Task GetCityDetail()
        {
            if (UserAddress.CityId == null)
            {
                City = new City()
                {
                    Governorate = new Governorate()
                    {
                        Country = new Country()
                    }
                };
            }
            else
            {
                City = Cities.Where(x => x.CityId == UserAddress.CityId).FirstOrDefault();
            }
            StateHasChanged();
        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        public async Task SubmitChnages()
        {
            if (UserAddress.Address != null && UserAddress.CityId != null)
            {
                spinnerService.Show();
                if (UserAddress.AddressId == Guid.Empty)
                    UserAddress.AddressId = Guid.NewGuid();
                dialogService.Close(UserAddress);
                spinnerService.Hide();
            }
            else
            {
                dialogService.Close();
            }
        }
    }
}
