using System;

using Xamarin.Forms;

namespace XamarinFormsTester
{
    public class App : Application
    {
        AppModel appModel;

        public App ()
        {
            appModel = new AppModel ();
            MainPage = appModel.GetInitialPage();
        }

        protected override void OnStart ()
        {
            // Handle when your app starts
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
