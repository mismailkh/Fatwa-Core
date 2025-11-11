using Microsoft.AspNetCore.Components;

namespace FATWA_WEB.Shared
{
    public partial class SessionExpireMessageDialog : ComponentBase, IAsyncDisposable
    {
        #region Paramter
        [Parameter]
        public float TimeRemainingMinutes { get; set; }
        #endregion

        #region variables

        private TimeSpan _countdownTime;
        private System.Threading.Timer? _timer;

        public string FormattedTime => $"{_countdownTime.Minutes:D2}:{_countdownTime.Seconds:D2}";
        #endregion

        #region Functions
        protected override void OnInitialized()
        {
            _countdownTime = TimeSpan.FromMinutes(TimeRemainingMinutes);
            _timer = new System.Threading.Timer(UpdateTimer, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));
        }

        private async void UpdateTimer(object? state)
        {
            if (_countdownTime.TotalSeconds > 0)
            {
                _countdownTime = _countdownTime.Subtract(TimeSpan.FromSeconds(1));
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                _timer?.Dispose();
                await InvokeAsync(StateHasChanged);
                dialogService.Close(false);
            }
        }

        private void OnContinueSessionClick()
        {
            _timer?.Dispose();
            dialogService.Close(true);
        }

        private void OnLogoutClick()
        {
            _timer?.Dispose();
            dialogService.Close(false);
        }

        public async ValueTask DisposeAsync()
        {
            if (_timer != null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
        #endregion
    }
}
