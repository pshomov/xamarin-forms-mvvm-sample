using System;
using XamarinFormsTester.UnitTests.ReduxVVM;
using System.Collections.Generic;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {}
    public delegate State Reducer<State>(State state, Action action);
	public delegate void StateChanged<State>(State state);
	public delegate void unsubscribe();
	public interface IStore<State>
	{
		unsubscribe subscribe (StateChanged<State> subscription);
		void dispatch (Action action);
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

        public void dispatch (Action action)
        {
            this._state = rootReducer.Invoke (this._state, action);
			foreach (var s in subscriptions) {
				s.Invoke (this._state);
			}
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
}

