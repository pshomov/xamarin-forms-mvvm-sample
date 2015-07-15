﻿using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester.UnitTests
{
    [TestFixture]
    public class Class1
    {
        ISettings settings;

        [SetUp]
        public void SetUp(){
            settings = Substitute.For<ISettings> ();
        }
            
        [Test]
        public void should_go_to_login_page_if_not_logged_in_already()
        {
            settings.Exists (Arg.Any<string> ()).Returns (false);

            var app = new AppModel (null,settings);
            Assert.That(app.GetInitialViewModel(), Is.TypeOf<LoginPageViewModel>());
        }

        [Test]
        public void should_go_to_devicelist_page_if_logged_in_already()
        {
            settings.Exists (Arg.Any<string> ()).Returns (true);
            settings.Get<AppState> (Arg.Any<string> ()).Returns (new AppState{LoggedIn = true});

            var app = new AppModel (null,settings);
            Assert.That(app.GetInitialViewModel(), Is.TypeOf<DeviceListPageViewModel>());
        }

    }
}