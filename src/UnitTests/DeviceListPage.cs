using System;
using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester.ViewModels;
using XamarinFormsTester.Services;

namespace XamarinFormsTester.UnitTests
{
    [TestFixture]
    public class DeviceListPage
    {
        ISettings settings;
        INavigator nav;

        [SetUp]
        public void SetUp(){
            settings = Substitute.For<ISettings> ();
            nav = Substitute.For<INavigator> ();
        }

        [Test]
        public void should_go_to_devicelist_page_on_start()
        {
            var app = new AppModel (nav);

            app.Start ();

            nav.Received().PushAsync<DeviceListPageViewModel>();
        }

    }
}

