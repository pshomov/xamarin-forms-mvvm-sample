using System;
using XamarinFormsTester.UnitTests.ReduxVVM;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {}
    public class InitStoreAction : Action {}

    public delegate State Reducer<State>(State state, Action action);
	public delegate void StateChangedSubscriber<State>(State state);
	public delegate void Unsubscribe();
    public delegate void DispatcherDelegate(Action a);

	public interface IStore<State>
	{
		Unsubscribe Subscribe (StateChangedSubscriber<State> subscription);
		void Dispatch (Action action);
		State GetState ();
	}
        
	public class Store<State> where State : new()
    {
        class SyncStore<State> : IStore<State> where State : new() {
            public SyncStore (Reducer<State> rootReducer)
            {
                this.rootReducer = rootReducer;
                this._state = rootReducer(this._state, new InitStoreAction());
            }

            public Unsubscribe Subscribe(StateChangedSubscriber<State> subscription){
                this.subscriptions.Add (subscription);
                return () => {
                    subscriptions.Remove(subscription);
                };
            }
            public void Dispatch (Action action)
            {
                this._state = rootReducer(this._state, action);
                foreach (var s in subscriptions) {
                    s (this._state);
                }
            }
            public State GetState ()
            {
                return _state;
            }
            List<StateChangedSubscriber<State>> subscriptions = new List<StateChangedSubscriber<State>>();
            State _state;
            Reducer<State> rootReducer;
        }

        public delegate State GetStateDelegate();
        public Unsubscribe Subscribe(StateChangedSubscriber<State> subscription) { return store.Subscribe (subscription); }

        public void Dispatch (Action action)
        {
            middlewares (action);
        }
            
        public Task<Result> Dispatch<Result> (Func<DispatcherDelegate, GetStateDelegate, Task<Result>> actionWithParams)
        {
            return actionWithParams(this.Dispatch, this.GetState);
        }

        public Task Dispatch (Func<DispatcherDelegate, GetStateDelegate, Task> actionWithParams)
        {
            return actionWithParams(this.Dispatch, this.GetState);
        }

        public Func<T, Func<DispatcherDelegate, GetStateDelegate, Task<Result>>> asyncAction<T, Result> (Func<DispatcherDelegate, GetStateDelegate, T, Task<Result>> m){
            return a => (dispatch, getState) => m (dispatch, getState, a);
        }   

        public Func<T, Func<DispatcherDelegate, GetStateDelegate, Task>> asyncActionVoid<T> (Func<DispatcherDelegate, GetStateDelegate, T, Task> m){
            return a => (dispatch, getState) => m (dispatch, getState, a);
        }   

        public Func<DispatcherDelegate, GetStateDelegate, Task<Result>> asyncAction<Result> (Func<DispatcherDelegate, GetStateDelegate, Task<Result>> m){
            return (dispatch, getState) => m (dispatch, getState);
        }   

        public State GetState (){ return store.GetState ();}

        public void Middleware(params Middleware<State>[] middlewares){
            this.middlewares = middlewares.Select(m => m(store)).Reverse().Aggregate<MiddlewareChainer, MiddlewareExecutor>(store.Dispatch, (acc, middle) => middle(acc));
        }

		public Store (SimpleReducer<State> rootReducer) : this(rootReducer.Get()){}
		public Store (CompositeReducer<State> rootReducer) : this(rootReducer.Get()){}
        public Store (Reducer<State> rootReducer)
        {
            store = new SyncStore<State> (rootReducer);
            this.Middleware ();
        }

        SyncStore<State> store;
        MiddlewareExecutor middlewares;
    }

	public delegate void MiddlewareExecutor(Action a);

	public delegate MiddlewareExecutor MiddlewareChainer(MiddlewareExecutor next);

	public delegate MiddlewareChainer Middleware<State>(IStore<State> store);
}

