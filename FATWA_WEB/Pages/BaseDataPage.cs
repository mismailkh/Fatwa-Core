using FATWA_DOMAIN.Interfaces;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Data;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages
{
    public abstract class BaseDataPage<TEntity, TKey> : ComponentBase
        where TEntity : class, IEntity, new()
    {
        protected TEntity Model { get; set; } = new();

        protected RadzenDataGrid<TEntity> DataGrid { get; set; }

        protected IEnumerable<TEntity> Records { get; set; }

        protected int RecordCount { get; set; }

        protected bool IsLoading { get; set; }

        protected bool ShowEditMode { get; set; }

        #region Dependencies







        [Inject]
        protected SpinnerService spinnerService { get; set; }

        [Inject]
        protected DialogService dialogService { get; set; }

        [Inject]
        protected NotificationService notificationService { get; set; }

        [Inject]
        protected TranslationState translationState { get; set; }

        [Inject]
        protected IGenericODataService<TEntity, TKey> ODataService { get; set; }

        #endregion Dependencies

        #region CRUD Operations


        //this is the new implementation

        //protected override async Task OnInitializedAsync()
        //{ 
        //}

        //protected virtual async Task LoadGridAsync(LoadDataArgs args)
        //{
        //    IsLoading = true;
        //    string odataFilter = GetODataFilter(args);
        //    var result = await ODataService.FindAsync(filter: odataFilter, top: args.Top, skip: args.Skip, orderby: args.OrderBy, count: true);
        //    Records = result.Value.AsODataEnumerable();
        //    RecordCount = Records.Count();
        //    IsLoading = false; 
        //}

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        protected virtual async Task Load()
        {
            spinnerService.Show();

            //string odataFilter = GetODataFilter();
            var result = await ODataService.FindAsync();
            Records = result.Value.AsODataEnumerable();
            RecordCount = Records.Count();

            spinnerService.Hide();
        }

        protected virtual string GetODataFilter(LoadDataArgs args) => args.Filter;

        protected virtual void Create()
        {
            Model = new TEntity();
            ShowEditMode = true;
        }

        protected virtual async Task EditAsync(TKey id)
        {
            Model = await ODataService.FindOneAsync(id);
            ShowEditMode = true;
        }

        protected virtual Task OnValidSumbitAsync()
        {
            return OnValidSumbitAsync(dialogService);
        }

        protected virtual async Task OnValidSumbitAsync(DialogService? dialogService)
        {
            try
            {
                TEntity? result = null;
                var key = (TKey)Model.KeyValues.First();
                if (key.IsDefault())
                {
                    result = await ODataService.InsertAsync(Model);
                    if (result != null)
                        notificationService.Notify(NotificationSeverity.Info, "Info", "Record created!");
                }
                else
                {
                    result = await ODataService.UpdateAsync(key, Model);
                    if (result != null)
                        notificationService.Notify(NotificationSeverity.Info, "Info", "Record updated!");
                }

                dialogService.Close(result);

                ShowEditMode = false;
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Sure_Cancel"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected virtual async Task DeleteAsync(TKey id)
        {
            try
            {
                //if (await dialogService.Confirm("Are you sure you want to delete this record?") == true)
                //{
                await ODataService.DeleteAsync(id);
                await DataGrid.Reload();
                //    notificationService.Notify(NotificationSeverity.Info, "Info", "Record deleted!");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //notificationService.Notify(NotificationSeverity.Error, "Error", "Unable to delete record!");
                // TODO: Log Error
            }
        }

        protected virtual void Cancel()
        {
            Model = new TEntity();
            ShowEditMode = false;

            dialogService.Close(null);
        }

        #endregion CRUD Operations
    }
}
