using System;

using Xamarin.Forms;
using XamarinFormsTester.Services;
using Microsoft.Practices.Unity;

namespace XamarinFormsTester
{
    public class App : Application
    {
        AppModel appM;

        public App ()
        {
            var navigationPage = new NavigationPage (new ContentPage());
            var container = new UnityContainer ();
            appM = new AppModel (new Navigator(navigationPage.Navigation, container), null);
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
