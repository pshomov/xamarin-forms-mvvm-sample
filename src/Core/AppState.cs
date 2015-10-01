using System;
using Xamarin.Forms;

namespace XamarinFormsTester
{
    public struct LoginPageStore {
        public bool LoggedIn;
        public string username;
        public string errMsg;
        public bool inProgress;
    }
	public struct AppState
	{
        public LoginPageStore loginPage;
	}

}
