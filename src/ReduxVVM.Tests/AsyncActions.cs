using System;
using NUnit.Framework;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using XamarinFormsTester.UnitTests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReduxVVM.Tests
{
	[TestFixture]
	public class AsyncActions
	{
        [Test]
		public async void should_allow_for_async_execution_of_code(){
			var storeReducerReached = 0;
			var reducer = new Events<List<string>>().When<SomeAction>((s,e) => {storeReducerReached += 1;return s;});
			var store = new Store<List<String>> (reducer, new List<string>{ "a"});

            var result = await store.Dispatch (async (dispatcher, store2) => {
                await Task.Delay(300);
                Assert.That(store2.Invoke()[0], Is.EqualTo("a"));
                dispatcher.Invoke(new SomeAction());
                return 112;
            });

            Assert.That(storeReducerReached, Is.EqualTo(1));
            Assert.That(result, Is.EqualTo(112));
		}

	}
}

