namespace FATWA_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-05-21' Version="1.0" Branch="master"> Added Generic Spinner Service</History>
    public class SpinnerService
    {
        public event Action OnShow;
        public event Action OnNewShow;
        public event Action OnSigningShow;
        public event Action OnHide;

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void NewShow()
        {
            OnNewShow?.Invoke();
        }

        public void SigningShow()
        {
            OnSigningShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }
}
