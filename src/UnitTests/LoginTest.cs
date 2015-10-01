using System;
using NUnit.Framework;
using XamarinFormsTester.Services;
using NSubstitute;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using XamarinFormsTester.UnitTests.ReduxVVM;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;

#pragma warning disable 4014 1998
namespace XamarinFormsTester.UnitTests
{
    public class AppStart : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}
    public class Login : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}
    public class LoginStarted : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}
    public class ViewMainPage : XamarinFormsTester.Infrastructure.ReduxVVM.Action {}

    [TestFixture]
    public class LoginTest
    {
        INavigator nav;
        IServiceAPI serviceAPI;


        Store<AppState> store;

        ComposeReducer<AppState> reducer;

        [SetUp]
        public void SetUp ()
        {
            nav = Substitute.For<INavigator> ();
            nav.PushAsync<Object> ().Returns (Task.Delay(0));
            serviceAPI = Substitute.For<IServiceAPI> ();

            var loginReducer = new Events<LoginPageStore> ().When<LoginStarted> ((s,a) => {
                var newState = s;
                newState.inProgress = true;
                return newState;
            });
            reducer = new ComposeReducer<AppState> ()
                .Part (s => s.loginPage, loginReducer);

            store = new Store<AppState> (reducer, new AppState());
        }

        [Test]
        public void should_not_modify_original_state(){
            var state = new AppState ();
            var store = new Store<AppState> (reducer, state);
            store.Dispatch (new LoginStarted ());
            Assert.That (store.GetState (), Is.Not.EqualTo (state));
        }

        [Test]
        public async void should_initiate_to_login(){
            var done = await store.Dispatch (async (disp, getState) => {
                if (!getState().loginPage.LoggedIn) 
                    nav.PushAsync<LoginPageViewModel>();
                else 
                    nav.PushAsync<DeviceListPageViewModel>();
                return true;
            });

            Assert.That (done, Is.True);
            nav.Received().PushAsync<LoginPageViewModel> ();

        }

        [Test]
        public async void should_start_login_process_when_provided_username_password(){
            var done = await store.Dispatch (async (disp, getState) => {
                disp(new LoginStarted());
                if (!getState().loginPage.LoggedIn) 
                    return nav.PushAsync<LoginPageViewModel>();
                else 
                    return nav.PushAsync<DeviceListPageViewModel>();
            });

            Assert.That (done, Is.TypeOf<Task>());
            nav.Received().PushAsync<LoginPageViewModel> ();

        }

    }
}

