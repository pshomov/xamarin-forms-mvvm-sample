using NUnit.Framework;

namespace XamarinFormsTester.UnitTests
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void PassingTest()
        {
            var app = new AppModel ();
            Assert.That(app.GetInitialPage(), Is.Not.Null);
        }

    }
}