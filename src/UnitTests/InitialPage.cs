using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester.ViewModels;

namespace XamarinFormsTester.UnitTests
{
    [TestFixture]
    public class InitialPage
    {
        ISettings settings;

        [SetUp]
        public void SetUp(){
            settings = Substitute.For<ISettings> ();
        }
            
        [Test]
        public void should_show_initializing_page()
        {
            settings.Get<AppState> (Arg.Any<string> ()).Returns (new AppState{LoggedIn = false});

            var app = new AppModel (settings);
            Assert.That(app.GetInitialViewModel(), Is.TypeOf<InitializingAppPageViewModel>());
        }
            
    }
}