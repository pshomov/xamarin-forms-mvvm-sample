using System;
using NUnit.Framework;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Collections.Generic;

namespace XamarinFormsTester.UnitTests
{
	public class SomeAction : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
		public string topic;
	}

	[TestFixture]
	public class MiddlewareTests
	{
		[Test]
		public void should_allow_middleware_to_hook_into_dispatching(){
			var storeReducerReached = 0;
			var reducer = new SimpleReducer<List<string>>().When<SomeAction>((s,e) => {storeReducerReached += 1;return s;});
			var store = new Store<List<String>> (reducer, new List<string>{});
			var middlewareCounter = 0;
            store.Middlewares( 
				s => next => action => {
					middlewareCounter += 3;
					Assert.That(middlewareCounter, Is.EqualTo(3));
					var res = next(action);
					middlewareCounter += 3000;
					Assert.That(middlewareCounter, Is.EqualTo(3333));
					return res;
				}, 
				s => next => action => {
					middlewareCounter += 30;
					Assert.That(middlewareCounter, Is.EqualTo(33));
                    Assert.That(storeReducerReached, Is.EqualTo(0));
					var res = next(action);
					Assert.That(storeReducerReached, Is.EqualTo(1));
					middlewareCounter += 300;
					Assert.That(middlewareCounter, Is.EqualTo(333));
					return res;
				}
			);

			store.Dispatch (new SomeAction ());
			Assert.That (middlewareCounter, Is.EqualTo (3333));
			Assert.That(storeReducerReached, Is.EqualTo(1));
		}

		[Test]
		public void should_allow_middleware_to_shortcut_lower_middleware(){
			var storeReducerReached = 0;
			var reducer = new SimpleReducer<List<string>>().When<SomeAction>((s,e) => {storeReducerReached += 1;return s;});
			var store = new Store<List<String>> (reducer, new List<string>{});
			var middlewareCounter = 0;
            store.Middlewares(
				s => next => action => {
					middlewareCounter += 3;
					Assert.That(middlewareCounter, Is.EqualTo(3));
					middlewareCounter += 3000;
					Assert.That(middlewareCounter, Is.EqualTo(3003));
					return action;
				}, 
				s => next => action => {
					middlewareCounter += 30;
					Assert.That(middlewareCounter, Is.EqualTo(33));
					Assert.That(storeReducerReached, Is.EqualTo(0));
					var res = next(action);
					Assert.That(storeReducerReached, Is.EqualTo(1));
					middlewareCounter += 300;
					Assert.That(middlewareCounter, Is.EqualTo(333));
					return res;
				}
			);

            store.Dispatch (new SomeAction ());
			Assert.That (middlewareCounter, Is.EqualTo (3003));
			Assert.That(storeReducerReached, Is.EqualTo(0));
		}

		[Test]
		public void should_allow_middleware_shortcut_the_store_dispatcher(){
			var storeReducerReached = 0;
			var reducer = new SimpleReducer<List<string>>().When<SomeAction>((s,e) => {storeReducerReached += 1;return s;});
			var store = new Store<List<String>> (reducer, new List<string>{});
			var middlewareCounter = 0;
            store.Middlewares(
				s => next => action => {
					middlewareCounter += 3;
					Assert.That(middlewareCounter, Is.EqualTo(3));
					var res = next(action);
					middlewareCounter += 3000;
					Assert.That(middlewareCounter, Is.EqualTo(3333));
					return res;
				}, 
				s => next => action => {
					middlewareCounter += 30;
					Assert.That(middlewareCounter, Is.EqualTo(33));
					middlewareCounter += 300;
					Assert.That(middlewareCounter, Is.EqualTo(333));
					return action;
				}
			);

            store.Dispatch (new SomeAction ());
			Assert.That (middlewareCounter, Is.EqualTo (3333));
			Assert.That(storeReducerReached, Is.EqualTo(0));
		}	
	}
}

