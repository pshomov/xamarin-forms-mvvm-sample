using NUnit.Framework;
using NSubstitute;
using XamarinFormsTester;

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
        public void PassingTest()
        {
            settings.Exists (Arg.Any<string> ()).Returns (false);

            var app = new AppModel (null,settings);
            Assert.That(app.GetInitialViewModel(), Is.Not.Null);
        }

    }
}