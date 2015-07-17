using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester
{
    public class AppModel
    {
        INavigator navigator;

        public AppModel (INavigator navigator)
        {
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

