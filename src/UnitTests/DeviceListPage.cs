using System;
using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester.ViewModels;
using XamarinFormsTester.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

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
                await Task.Delay (100);
                return new List<DeviceInfo> (){ new DeviceInfo {
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
            var app = container.Resolve<AppModel> ();

            app.Start ();
//            serviceAPI.Received ().GetDevices ();
            nav.Received ().PushAsync<DeviceListPageViewModel> (Arg.Any<Action<DeviceListPageViewModel>>());
        }

    }
}

