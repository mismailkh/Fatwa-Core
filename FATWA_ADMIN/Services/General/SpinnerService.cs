namespace FATWA_ADMIN.Services.General
{
    //<History Author = 'Hassan Abbas' Date='2022-05-21' Version="1.0" Branch="master"> Added Generic Spinner Service</History>
    public class SpinnerService
    {
        public event Action OnShow;
        public event Action OnHide;

        public void Show()
        {
            OnShow?.Invoke();
        }

        public void Hide()
        {
            OnHide?.Invoke();
        }
    }
}
