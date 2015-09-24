using System;
using XamarinFormsTester.UnitTests.ReduxVVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {}
    public class AsyncAction<Result> : XamarinFormsTester.Infrastructure.ReduxVVM.Action {
        Func<Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action>, Task<Result>> dispatch;

        public AsyncAction(Func<Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action>, Task<Result>> dispatch){
            this.dispatch = dispatch;
        }

        public Task<Result> get(Func<XamarinFormsTester.Infrastructure.ReduxVVM.Action, XamarinFormsTester.Infrastructure.ReduxVVM.Action> dispatcher){
            return dispatch (dispatcher);
        }
    }

    public delegate State Reducer<State>(State state, Action action);
	public delegate void StateChanged<State>(State state);
	public delegate void unsubscribe();
	public interface IStore<State>
	{
		unsubscribe subscribe (StateChanged<State> subscription);
		Action dispatch (Action action);
		State getState ();
	}

	public class Store<State> : IStore<State> where State : new()
    {
		public unsubscribe subscribe(StateChanged<State> subscription){
			this.subscriptions.Add (subscription);
			return () => {
				subscriptions.Remove(subscription);
			};
		}

        public Action dispatch (Action action)
        {
            this._state = rootReducer.Invoke (this._state, action);
			foreach (var s in subscriptions) {
				s.Invoke (this._state);
			}
			return action;
        }

        public Task<T> dispatch<T> (AsyncAction<T> action)
        {
            return action.get(this.dispatch);
        }

        public State getState ()
        {
            return _state;
        }
			
		public Store (Events<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
		public Store (ComposeReducer<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
        public Store (Reducer<State> rootReducer, State initialState)
        {
            this.rootReducer = rootReducer;
            this._state = initialState;
        }

		List<StateChanged<State>> subscriptions = new List<StateChanged<State>>();
		State _state;
		Reducer<State> rootReducer;
    }

	public delegate Action MiddlewareExecutor(Action a);

	public delegate MiddlewareExecutor MiddlewareChainer(MiddlewareExecutor next);

	public delegate MiddlewareChainer Middleware<State>(IStore<State> store);

	public class Middlewares<State> : IStore<State> where State : new(){
		IStore<State> next;
		MiddlewareExecutor middlewares;

		public Middlewares(IStore<State> next, params Middleware<State>[] middlewares){
			this.middlewares = middlewares.Select(m => m(next)).Reverse().Aggregate<MiddlewareChainer, MiddlewareExecutor>(next.dispatch, (acc, middle) => middle(acc));
			this.next = next;
		}
		public unsubscribe subscribe (StateChanged<State> subscription)
		{
			return next.subscribe (subscription);
		}
		public Action dispatch (Action action)
		{
			return middlewares (action);
		}
		public State getState ()
		{
			return next.getState();
		}
	}
}

