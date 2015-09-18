using System;
using XamarinFormsTester.UnitTests.ReduxVVM;
using System.Collections.Generic;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {
    }
    public delegate State Reducer<State>(State state, Action action);
	public delegate void StateChanged<State>(State state);
	public class Store<State> where State : new()
    {
		List<StateChanged<State>> subscribtions = new List<StateChanged<State>>();
		public delegate void unsubscribe ();
		public unsubscribe subscribe(StateChanged<State> subscribtion){
			this.subscribtions.Add (subscribtion);
			return () => {
				subscribtions.Remove(subscribtion);
			};
		}

        public void dispatch (Action action)
        {
            this._state = rootReducer.Invoke (this._state, action);
			foreach (var s in subscribtions) {
				s.Invoke (this._state);
			}
        }

        public State getState ()
        {
            return _state;
        }

        State _state;

        Reducer<State> rootReducer;

		public Store (Events<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState)
		{
		}
		public Store (ComposeReducer<State> rootReducer, State initialState) : this(rootReducer.Get(), initialState)
		{
		}
        public Store (Reducer<State> rootReducer, State initialState)
        {
            this.rootReducer = rootReducer;
            this._state = initialState;
        }
    }
}

