using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.MojRolls
{
    public partial class MojRollsExceptionView : ComponentBase
    {
        #region varibles
        public bool IsBtnView { get; set; } = true;
        public DateTime? SessionDate;
        public int RollsId;
        public int ChamberId;
        public int ChamberTypeCodeId;
        public int CourtId;
        public int MOJRollsCourtsId { get; set; }
        public int MOJRollsChamberId { get; set; }
        public int MOJRollsChamberTypeCodeId { get; set; }

        public int MOJRollsId { get; set; }
        DateTime? MOJRollsSessionDate;
        public bool checkBox1Value;
        public DateTime Min = new DateTime(2001, 1, 1);
        public DateTime Minimum = new DateTime(1950, 1, 1);
        #endregion
        [Parameter]
        public dynamic Id { get; set; }
        public string ExceptionDetails { get; set; }
        #region DropDown list 
        public IEnumerable<MOJRollsCourts> MOJRollsCourtsddl;
        public IEnumerable<MOJRollsChamberVM> MOJRollsChamberddl;
        public IEnumerable<MOJRollsChamberVM> RMSChambersDdl;
        public IEnumerable<MOJRollsChamberTypeCode> MOJRollsChamberTypeCodeddl;
        public IEnumerable<MOJRollsVM> MOJRollsddl;
        #endregion

        protected override async Task OnInitializedAsync()
        {

            await PopulateRMSCourtsLookUP();
            await Load();
        }
        protected async Task Load()
        {
            try
            {
                spinnerService.Show();
                var Result = await mojRollsService.GetRMSRequestsDetailById(Id);
                if (Result.IsSuccessStatusCode)
                {
                    var res = (MOJRollsRequestDetailsList)Result.ResultData;
                    await PopulateChambersLookuUp();
                    await PopulateMojRolleChamberNumber();
                    await PopulateMojRolls();
                    var chamberId = MOJRollsChamberddl.Where(x => x.Id == Convert.ToInt32(res.ChamberType_LookUp)).FirstOrDefault();
                    MOJRollsChamberId = chamberId.Id;
                    var chamernumberId = MOJRollsChamberTypeCodeddl.Where(x => x.Id == Convert.ToInt32(res.ChamberTypeCode_LookUp)).FirstOrDefault();
                    MOJRollsChamberTypeCodeId = chamernumberId.Id;
                    MOJRollsCourtsId = Convert.ToInt32(res.CourtType_LookUp);
                    var mojrolls = MOJRollsddl.Where(x => x.Id == Convert.ToInt32(res.RollId_LookUp)).FirstOrDefault();
                    MOJRollsId = mojrolls.Id;
                    ExceptionDetails = res.ExceptionDetails;
                    MOJRollsSessionDate = res.SessionDate;

                }
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #region Drop Dwon List

        protected async Task PopulateRMSCourtsLookUP()
        {

            var response = await mojRollsService.GetRMSCourtsLookUP(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsCourtsddl = (List<MOJRollsCourts>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateMojRolls()
        {
            var response = await mojRollsService.GetMOjRolls();
            if (response.IsSuccessStatusCode)
            {
                MOJRollsddl = (List<MOJRollsVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateChambersLookuUp()
        {
            var response = await mojRollsService.GetRmsChambersLookuUp(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsChamberddl = (List<MOJRollsChamberVM>)response.ResultData;
                RMSChambersDdl = MOJRollsChamberddl.Distinct(new MOJRollsChamberVMEqualityComparer()).ToList();
                MOJRollsChamberddl = RMSChambersDdl;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'ijaz Ahmad' Date='2023-01-25' Version="1.0" Branch="master"> MOJRollsChamberTypeCode Model use as Chamber Number</History>
        protected async Task PopulateMojRolleChamberNumber()
        {

            var response = await mojRollsService.GetMojRolleChamberNumberByUserId(loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin) ? null : loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsChamberTypeCodeddl = (List<MOJRollsChamberTypeCode>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion
        protected async Task FormSubmit(MouseEventArgs args)
        {

        }


        protected async System.Threading.Tasks.Task save(MouseEventArgs args)
        {
            try
            {

                MOJRollsRequest addDMOJRollsRequest = new MOJRollsRequest();
                if (SessionDate != null)
                {
                    addDMOJRollsRequest.SessionDate = SessionDate;
                }
                else
                {
                    addDMOJRollsRequest.SessionDate = MOJRollsSessionDate;
                }
                if (RollsId != 0)
                {
                    addDMOJRollsRequest.RollId_LookUp = RollsId;
                }
                else
                {
                    addDMOJRollsRequest.RollId_LookUp = MOJRollsId;
                }
                if (ChamberId != 0)
                {

                    addDMOJRollsRequest.ChamberType_LookUp = ChamberId;
                }
                else
                {
                    addDMOJRollsRequest.ChamberType_LookUp = MOJRollsChamberId;
                }
                if (ChamberTypeCodeId != 0)
                {

                    addDMOJRollsRequest.ChamberTypeCode_LookUp = ChamberTypeCodeId;
                }
                else
                {
                    addDMOJRollsRequest.ChamberTypeCode_LookUp = MOJRollsChamberTypeCodeId;
                }
                if (CourtId != 0)
                {

                    addDMOJRollsRequest.CourtType_LookUp = CourtId;
                }
                else
                {
                    addDMOJRollsRequest.CourtType_LookUp = MOJRollsCourtsId;
                }
                var Result = await mojRollsService.GetRMSRequestsDetailById(Id);
                var res = (MOJRollsRequestDetailsList)Result.ResultData;
                if (Result.IsSuccessStatusCode)
                {
                    MOJRollsRequestStatusHistory addMOJRollsRequestStatusHistory = new MOJRollsRequestStatusHistory();
                    addMOJRollsRequestStatusHistory.PreviousStatusId_from = res.RequestStatus_LookUp;
                    addMOJRollsRequestStatusHistory.NewStatusId_To = 4;//Retry Extraction
                    addMOJRollsRequestStatusHistory.Request_Id = res.Id;
                    if (SessionDate != null)
                    {
                        res.SessionDate = SessionDate;
                    }
                    addMOJRollsRequestStatusHistory.CreatedBy = loginState.UserDetail.ActiveDirectoryUserName;
                    addMOJRollsRequestStatusHistory.SessionDate = (MOJRollsSessionDate != null) ? res.SessionDate : res.SessionDate;
                    var response = await mojRollsService.CreateRMSRequestStatusHistory(addMOJRollsRequestStatusHistory);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Request_Success_Massage"),//successfully added
                            Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;"
                        });

                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            dialogService.Close(null);
        }

        public async Task OnChange(bool? value, string name)
        {
            if (value == true)
            {
                IsBtnView = false;
            }
            if (value == false)
            {
                IsBtnView = true;
            }

        }
        public async Task OnChangeMOJRollsCourts(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            var courttype = MOJRollsCourtsddl.Where(x => x.Id == formID).FirstOrDefault();
            var response = await mojRollsService.GetRMSRollsAgainstCourtTypeId((int)courttype.TypeId);
            if (response.IsSuccessStatusCode)
            {
                MOJRollsddl = (List<MOJRollsVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            await PopulateChambersLookuUp();
            RMSChambersDdl = MOJRollsChamberddl.Where(x => x.CourtId == formID).ToList();
            if (RMSChambersDdl != null && RMSChambersDdl.Any())
            {
                RMSChambersDdl = RMSChambersDdl.Distinct(new MOJRollsChamberVMEqualityComparer()).ToList();
                MOJRollsChamberddl = RMSChambersDdl;


            }
            else
            {
                await PopulateChambersLookuUp();
            }
        }
        public void OnChangeMOJRollsChamberTypeCode(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            ChamberTypeCodeId = formID;
        }
        public class MOJRollsChamberVMEqualityComparer : IEqualityComparer<MOJRollsChamberVM>
        {
            public bool Equals(MOJRollsChamberVM x, MOJRollsChamberVM y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.CourtId == y.CourtId;
            }

            public int GetHashCode(MOJRollsChamberVM obj)
            {
                if (obj == null)
                    return 0;

                return HashCode.Combine(obj.Id, obj.CourtId);
            }
        }
        public class MOJRollsChamberTypeCodeEqualityComparer : IEqualityComparer<MOJRollsChamberTypeCode>
        {
            public bool Equals(MOJRollsChamberTypeCode x, MOJRollsChamberTypeCode y)
            {
                if (x == null || y == null)
                    return false;

                return x.Id == y.Id && x.Name == y.Name && x.ChamberId == y.ChamberId;
            }

            public int GetHashCode(MOJRollsChamberTypeCode obj)
            {
                if (obj == null)
                    return 0;

                return HashCode.Combine(obj.Id, obj.Name, obj.ChamberId);
            }
        }
        public async Task OnChangeMOJRollsChamberVM(object value)
        {
            await PopulateMojRolleChamberNumber();
            // Apply distinct filter based on ChamberId
            MOJRollsChamberTypeCodeddl = MOJRollsChamberTypeCodeddl
                    .Where(x => x.ChamberId == (int)value)
                .Select(x => new MOJRollsChamberTypeCode
                {
                    Id = x.Id,
                    Name = x.Name,
                })
                .Distinct(new MOJRollsChamberTypeCodeEqualityComparer()) // Apply distinct using the custom comparer
                .ToList();
            StateHasChanged();
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            ChamberId = formID;
        }
        void GetSessionDAte(DateTime? MOJRollsSessionDate, string name, string format)
        {
            SessionDate = MOJRollsSessionDate;
            if (SessionDate != null && RollsId != 0 && ChamberId != 0 && ChamberTypeCodeId != 0 && CourtId != 0)
            {
                IsBtnView = false;
            }
        }
        public void OnChangeMOJRolls(object value)
        {
            var str = value is IEnumerable<object> ? string.Join(", ", (IEnumerable<object>)value) : value;
            int formID = Convert.ToInt32(str);
            RollsId = formID;
        }
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
