using System;
using Xamarin.Forms;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester
{
    public class AppModel
    {
        AppState state;

        public AppModel (ISecureStorage storage, ISettings settings)
        {
                state = settings.Get<AppState> ("state");
        }

        public ViewModel GetInitialViewModel() {
            if (state.LoggedIn) {
                return new DeviceListPageViewModel();
            } else {
                return new LoginPageViewModel();
            }
        }
    }
}
