using System;

namespace XamarinFormsTester.Infrastructure.FluxVVM
{
    public struct Action<A> {
        public A type;
    }
    public delegate State Reducer<State, Actions>(State state, Action<Actions> action);
    public class Store<State, Actions>
    {
        public void dispatch (Action<Actions> action)
        {
            this._state = rootReducer.Invoke (this._state, action);
        }

        public State getState ()
        {
            return _state;
        }

        State _state;

        Reducer<State, Actions> rootReducer;

        public Store (Reducer<State, Actions> rootReducer, State initialState)
        {
            this.rootReducer = rootReducer;
            this._state = initialState;
        }
    }
}

