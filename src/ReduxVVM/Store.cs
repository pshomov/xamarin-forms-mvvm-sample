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
		Action Dispatch (Action action);
		State GetState ();
	}
        
	public class Store<State> where State : new()
    {
        class SyncStore<State> : IStore<State> where State : new() {
            public SyncStore (Reducer<State> rootReducer, State initialState)
            {
                this.rootReducer = rootReducer;
                this._state = initialState;
            }

            public Unsubscribe Subscribe(StateChanged<State> subscription){
                this.subscriptions.Add (subscription);
                return () => {
                    subscriptions.Remove(subscription);
                };
            }
            public Action Dispatch (Action action)
            {
                this._state = rootReducer(this._state, action);
                foreach (var s in subscriptions) {
                    s (this._state);
                }
                return action;
            }
            public State GetState ()
            {
                return _state;
            }
            List<StateChanged<State>> subscriptions = new List<StateChanged<State>>();
            State _state;
            Reducer<State> rootReducer;
        }
        MiddlewareExecutor middlewares;
        public delegate State StoreDelegate();

        public Unsubscribe Subscribe(StateChanged<State> subscription) { return store.Subscribe (subscription); }

        public Action Dispatch (Action action)
        {
            return middlewares (action);
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

        public Func<DispatcherDelegate, StoreDelegate, Task<Result>> asyncAction<Result> (Func<DispatcherDelegate, StoreDelegate, Task<Result>> m){
            return (dispatch, getState) => m (dispatch, getState);
        }   

        public State GetState (){ return store.GetState ();}

        public void Middlewares(params Middleware<State>[] middlewares){
            this.middlewares = middlewares.Select(m => m(store)).Reverse().Aggregate<MiddlewareChainer, MiddlewareExecutor>(store.Dispatch, (acc, middle) => middle(acc));
        }

		public Store (SimpleReducer<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
		public Store (CompositeReducer<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState){}
        public Store (Reducer<State> rootReducer, State initialState)
        {
            store = new SyncStore<State> (rootReducer, initialState);
            this.Middlewares ();
        }

        SyncStore<State> store;
    }

	public delegate Action MiddlewareExecutor(Action a);

	public delegate MiddlewareExecutor MiddlewareChainer(MiddlewareExecutor next);

	public delegate MiddlewareChainer Middleware<State>(IStore<State> store);
}

