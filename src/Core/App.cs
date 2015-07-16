using System;

using Xamarin.Forms;
using XamarinFormsTester.Services;
using XLabs.Ioc;

namespace XamarinFormsTester
{
    public class App : Application
    {
        AppModel appModel;

        AppM appM;

        public App ()
        {
            appModel = new AppModel (null);
            var navigationPage = new NavigationPage (appModel.GetInitialViewModel ().Page);
            var container = new SimpleContainer ();
            appM = new AppM (new Navigator(navigationPage.Navigation, container.GetResolver()));
            MainPage = navigationPage;
        }

        protected override void OnStart ()
        {
            appM.Start();
        }

        protected override void OnSleep ()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume ()
        {
            // Handle when your app resumes
        }
    }
}
