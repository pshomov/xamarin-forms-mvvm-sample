using System;
using System.Collections.Generic;
using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester.ViewModels;
using XamarinFormsTester.Services;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using PubSub;

namespace XamarinFormsTester.UnitTests
{
    [TestFixture]
    public class DeviceListPage
    {
        INavigator nav;
        IUnityContainer container;
        IServiceAPI serviceAPI;

        [SetUp]
        public void SetUp ()
        {
            nav = Substitute.For<INavigator> ();
            container = new UnityContainer ();
            container.RegisterInstance (nav);
            serviceAPI = Substitute.For<IServiceAPI> ();
            container.RegisterInstance (serviceAPI);
        }

        [Test]
        public void should_go_to_devicelist_page_on_start ()
        {
            serviceAPI.GetDevices ().Returns (async r => {
                return new List<DeviceInfo> (){ 
                        new DeviceInfo {
                            Name = "D1",
                            Location = "L1",
                            Online = true
                        }, new DeviceInfo {
                            Name = "D2",
                            Location = "L2",
                            Online = true
                        } 
                };
            });

            AssertModel<DeviceListPageViewModel> (vm => {
                Assert.That(vm.Pulling, Is.False);
                Assert.That(vm.Devices[0].Name, Is.EqualTo("D1"));
                Assert.That(vm.Devices[1].Name, Is.EqualTo("D2"));
            });

            var app = container.Resolve<AppModel> ();

            app.Start ();
        }

        void AssertModel<ViewModel> (Action<ViewModel> assertBlock) where ViewModel : class,new()
        {
            var deviceListPageViewModel = new ViewModel();
            nav.PushAsync<ViewModel> (Arg.Do<Action<ViewModel>> (modelUpdate =>  {
                modelUpdate.Invoke (deviceListPageViewModel);
                assertBlock.Invoke (deviceListPageViewModel);
            }), Arg.Any<List<Type>> ());
        }
    }
}

