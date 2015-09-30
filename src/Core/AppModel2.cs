using System;
using XamarinFormsTester.Services;

namespace XamarinFormsTester
{
    public class AppModel2
    {
        INavigator navigator;
        IServiceAPI api;
        public AppModel2 (INavigator navigator, IServiceAPI api)
        {
            this.api = api;
            this.navigator = navigator;
        }
    }
}

