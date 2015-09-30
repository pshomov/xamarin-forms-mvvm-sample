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

        AppModel app;

        [SetUp]
        public void SetUp ()
        {
            nav = Substitute.For<INavigator> ();
            serviceAPI = Substitute.For<IServiceAPI> ();
            app = new AppModel (nav, serviceAPI);
        }

        [Test]
        public void should_go_to_devicelist_page_on_start ()
        {
            serviceAPI.GetDevices ().Returns (r => {
                return Task.FromResult(new List<DeviceInfo> (){ 
                    new DeviceInfo {
                        Name = "D1",
                        Location = "L1",
                        Online = true
                    }, new DeviceInfo {
                        Name = "D2",
                        Location = "L2",
                        Online = true
                    } 
                });
            });

            AssertModel<DeviceListPageViewModel> (vm => {
                Assert.That(vm.Pulling, Is.False);
                Assert.That(vm.Devices[0].Name, Is.EqualTo("D1"));
                Assert.That(vm.Devices[1].Name, Is.EqualTo("D2"));
            });

            app.Start ();

            nav.Received().PushAsync<DeviceListPageViewModel> (Arg.Any<Action<DeviceListPageViewModel>> (), Arg.Any<List<Type>> ());
        }

        [Test]
        public void should_navigate_to_device_info_when_selected ()
        {
            serviceAPI.GetDevices ().Returns (r => {
                return Task.FromResult(new List<DeviceInfo> (){ 
                    new DeviceInfo {
                        Name = "D1",
                        Id = new DeviceId("id1"),
                        Location = "L1",
                        Online = true
                    }, new DeviceInfo {
                        Name = "D2",
                        Location = "L2",
                        Id = new DeviceId("id2"),
                        Online = true
                    } 
                });
            });

            AssertModel<DeviceDetailsPageViewModel> (vm => {
                Assert.That(vm.Title, Is.EqualTo("D1"));
            });

            app.Start ();
            app.OnSelectDevice (new DeviceSelected{deviceId = new DeviceId("id1")});

            nav.Received().PushAsync<DeviceDetailsPageViewModel> (Arg.Any<Action<DeviceDetailsPageViewModel>> (), Arg.Any<List<Type>> ());
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

