using System;
using NUnit.Framework;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using XamarinFormsTester.UnitTests;
using System.Collections.Generic;

namespace ReduxVVM.Tests
{
	public class AsyncAction : XamarinFormsTester.Infrastructure.ReduxVVM.Action {
		Action<Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action>> dispatch;

		public AsyncAction(Action<Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action>> dispatch){
			this.dispatch = dispatch;
		}

		public void get(Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action> dispatcher){
			dispatch (dispatcher);
		}
	}

	class DoneAction : XamarinFormsTester.Infrastructure.ReduxVVM.Action {
	}

	[TestFixture]
	public class AsyncActions
	{
		public void should_dispatch_events_to_store(){
			var storeReducerReached = 0;
			var reducer = new Events<List<string>>().When<SomeAction>((s,e) => {storeReducerReached += 1;return s;});
			var store = new Store<List<String>> (reducer, new List<string>{});
			var middlewareCounter = 0;
			var middleware = new Middlewares<List<String>> (store, 
				                 delegate (IStore<List<String>> s) {
					return delegate (MiddlewareExecutor next) {
						return delegate (XamarinFormsTester.Infrastructure.ReduxVVM.Action action) {
							//					if (action as AsyncAction){
							//					}
//							((AsyncAction)action).get ();
							//					action.get();
							return new DoneAction ();
						};
					};
				}
			                 );

//			middleware.dispatch (new AsyncAction(dis => {}));
//			Assert.That(storeReducerReached, Is.EqualTo(1));

		}
	}
}

