using System;
using NUnit.Framework;
using XamarinFormsTester.Services;
using NSubstitute;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using XamarinFormsTester.UnitTests.ReduxVVM;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

#pragma warning disable 4014 1998
namespace XamarinFormsTester.UnitTests
{
    public class AppStart : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}

    public struct LoginInfo {
        public string Username;
        public string Password;
    }

    public struct LoggedIn : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
        public string Username;
        public string City;
    }
    public struct LoggingIn : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
        public string Username;
    }


    [TestFixture]
    public class LoginTest
    {
        INavigator nav;
        IServiceAPI serviceAPI;


        Store<AppState> store;

        ComposeReducer<AppState> reducer;

        List<Tuple<XamarinFormsTester.Infrastructure.ReduxVVM.Action, AppState>> history;

        [SetUp]
        public void SetUp ()
        {
            nav = Substitute.For<INavigator> ();
            nav.PushAsync<Object> ().Returns (Task.Delay(0));
            serviceAPI = Substitute.For<IServiceAPI> ();
            serviceAPI.AuthUser ("john", "secret").Returns(Task.FromResult(new UserInfo{Username = "John", HomeCity="Reykjavik"}));

            var loginReducer = new Events<LoginPageStore> ()
            .When<LoggingIn> ((s, a) => {
                s.inProgress = true;
                return s;
            })
            .When<LoggedIn> ((s, a) => {
                s.inProgress = false;
                return s;
            });
            reducer = new ComposeReducer<AppState> ()
                .Part (s => s.loginPage, loginReducer);

            history = new List<Tuple<XamarinFormsTester.Infrastructure.ReduxVVM.Action, AppState>> ();
            store = new Store<AppState> (reducer, new AppState());
            store.Middlewares (s => next => action => {
                var before = s.GetState ();
                var res = next (action);
                var after = s.GetState ();
                history.Add (Tuple.Create (action, after));
                return res;
            });
        }

        [Test]
        public void should_not_modify_original_state(){
            var state = new AppState ();
            var store = new Store<AppState> (reducer, state);
            store.Dispatch (new LoggingIn ());
            Assert.That (store.GetState (), Is.Not.EqualTo (state));
        }

        [Test]
        public async void should_navigate_to_login_viewmode_when_not_logged_in(){
            await store.Dispatch (store.asyncAction((disp, getState) => {
                if (!getState().loginPage.LoggedIn) 
                    return nav.PushAsync<LoginPageViewModel>();
                else 
                    return nav.PushAsync<DeviceListPageViewModel>();
            }));

            nav.Received().PushAsync<LoginPageViewModel> ();
        }

        [Test]
        public async void should_start_login_process_when_provided_username_password(){
            var LoginAction = store.asyncActionVoid<LoginInfo> (async (disp, getState, userinfo) =>  {
                disp(new LoggingIn{Username = userinfo.Username});
                var loggedIn = await serviceAPI.AuthUser (userinfo.Username, userinfo.Password);
                disp(new LoggedIn{Username = userinfo.Username, City = loggedIn.HomeCity});
                nav.PushAsync<DeviceListPageViewModel>();
            });
            await store.Dispatch (LoginAction(new LoginInfo{Username = "john", Password = "secret"}));

            nav.Received().PushAsync<DeviceListPageViewModel> ();
            Assert.That (history.Find(a => a.Item1.GetType() == typeof(LoggingIn)).Item2.loginPage, Is.EqualTo (new LoginPageStore{ inProgress = true }));
            Assert.That (history.Find(a => a.Item1.GetType() == typeof(LoggedIn)).Item2.loginPage, Is.EqualTo (new LoginPageStore{ inProgress = false }));
        }

    }
}

