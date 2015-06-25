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
            if (settings.Exists ("state")) {
                state = settings.Get<AppState> ("state");
            } else
                state = new AppState ();

        }

        public ViewModel GetInitialViewModel() {
            if (state.LoggedIn) {
                return new LoginPageViewModel();
            } else {
                return new LoginPageViewModel();
            }
        }
    }
}
