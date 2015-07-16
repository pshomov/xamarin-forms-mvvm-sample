using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester
{
    public class AppM
    {
        INavigator navigator;

        public AppM (INavigator navigator)
        {
            this.navigator = navigator;
        }

        public void Start ()
        {
            navigator.PushAsync<DeviceListPageViewModel> ();
        }
    }
}

