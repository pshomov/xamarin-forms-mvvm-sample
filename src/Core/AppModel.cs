using System;
using Xamarin.Forms;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester
{
    public class AppModel
    {
        AppState state;
        AppM app;

        public AppModel (ISettings settings)
        {
            this.app = app;
            state = settings.Get<AppState> ("state");
        }

        public ViewModel GetInitialViewModel() {
            return new InitializingAppPageViewModel ();
        }
    }
}
