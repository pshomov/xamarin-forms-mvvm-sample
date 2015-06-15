using System;
using Xamarin.Forms;

namespace XamarinFormsTester
{
    public class AppModel
    {
        public AppModel ()
        {
        }

        public Page GetInitialPage() {
            return new ContentPage();
        }
    }
}
