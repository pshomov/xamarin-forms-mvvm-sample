using System;
using XamarinFormsTester.UnitTests.ReduxVVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {}

    public delegate State Reducer<State>(State state, Action action);
	public delegate void StateChanged<State>(State state);
	public delegate void Unsubscribe();
    public delegate Action DispatcherDelegate(Action a);

	public interface IStore<State>
	{
		Unsubscribe Subscribe (StateChanged<State> subscription);
		Action StoreDispatch (Action action);
		State GetState ();
	}

	public class Store<State> : IStore<State> where State : new()
    {
        MiddlewareExecutor middlewares;
        public delegate State StoreDelegate();

        public Unsubscribe Subscribe(StateChanged<State> subscription){
			this.subscriptions.Add (subscription);
			return () => {
				subscriptions.Remove(subscription);
			};
		}

        public Action Dispatch (Action action)
        {
            return middlewares (action);
        }

        public Action StoreDispatch (Action action)
        {
            this._state = rootReducer(this._state, action);
            foreach (var s in subscriptions) {
                s (this._state);
            }
            return action;
        }

        public Task<Result> Dispatch<Result> (Func<DispatcherDelegate, StoreDelegate, Task<Result>> actionWithParams)
        {
            return actionWithParams(this.Dispatch, this.GetState);
        }

        public Task Dispatch (Func<DispatcherDelegate, StoreDelegate, Task> actionWithParams)
        {
            return actionWithParams(this.Dispatch, this.GetState);
        }

        public Func<T, Func<DispatcherDelegate, StoreDelegate, Task<Result>>> asyncAction<T, Result> (Func<DispatcherDelegate, StoreDelegate, T, Task<Result>> m){
            return a => (dispatch, getState) => m (dispatch, getState, a);
        }   

        public Func<T, Func<DispatcherDelegate, StoreDelegate, Task>> asyncActionVoid<T> (Func<DispatcherDelegate, StoreDelegate, T, Task> m){
            return a => (dispatch, getState) => m (dispatch, getState, a);
        }   

        public Func<DispatcherDelegate, StoreDelegate, Task> asyncAction (Func<DispatcherDelegate, StoreDelegate, Task> m){
            return (dispatch, getState) => m (dispatch, getState);
        }   
        public Func<DispatcherDelegate, StoreDelegate, Task<Result>> asyncAction<Result> (Func<DispatcherDelegate, StoreDelegate, Task<Result>> m){
            return (dispatch, getState) => m (dispatch, getState);
        }   

        public State GetState ()
        {
            return _state;
        }

        public void Middlewares(params Middleware<State>[] middlewares){
            this.middlewares = middlewares.Select(m => m(this)).Reverse().Aggregate<MiddlewareChainer, MiddlewareExecutor>(StoreDispatch, (acc, middle) => middle(acc));
        }

		public Store (Events<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
		public Store (ComposeReducer<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
        public Store (Reducer<State> rootReducer, State initialState)
        {
            this.rootReducer = rootReducer;
            this._state = initialState;
            this.Middlewares ();
        }

		List<StateChanged<State>> subscriptions = new List<StateChanged<State>>();
		State _state;
		Reducer<State> rootReducer;
    }

	public delegate Action MiddlewareExecutor(Action a);

	public delegate MiddlewareExecutor MiddlewareChainer(MiddlewareExecutor next);

	public delegate MiddlewareChainer Middleware<State>(IStore<State> store);
}

