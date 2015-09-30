using System;
using NUnit.Framework;
using XamarinFormsTester.Services;
using NSubstitute;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using XamarinFormsTester.UnitTests.ReduxVVM;

namespace XamarinFormsTester.UnitTests
{
    public class AppStart : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}
    [TestFixture]
    public class LoginTest
    {
        INavigator nav;
        IServiceAPI serviceAPI;

        AppModel2 app;

        Store<AppState> store;

        [SetUp]
        public void SetUp ()
        {
            nav = Substitute.For<INavigator> ();
            serviceAPI = Substitute.For<IServiceAPI> ();
            var reducer = new ComposeReducer<AppState> ()
                .Part (s => s.LoggedIn, (s, _) => { return s;});

            store = new Store<AppState> (reducer, new AppState());
            app = new AppModel2 (nav, serviceAPI);
        }

        [Test]
        public void should_initiate_login(){
            store.Dispatch (new AppStart ());
        }
    }
}

