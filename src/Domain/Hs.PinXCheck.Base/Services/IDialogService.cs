namespace Hs.PinXCheck.Base.Services
{
    public interface IDialogService
    {
        void ShowDialog(string dialogName);

        bool CanShowDialog(string dialogName);
    }
}
