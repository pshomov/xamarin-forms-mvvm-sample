using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester
{
    public class AppModel
    {
        INavigator navigator;

        IServiceAPI api;

        public AppModel (INavigator navigator, IServiceAPI api)
        {
            this.api = api;
            this.navigator = navigator;
        }

        public void Start ()
        {
            navigator.PushAsync<DeviceListPageViewModel> ((vm) => {
                vm.Pulling = true;
            });
        }

    }
}

