﻿using System;
using XamarinFormsTester.UnitTests.ReduxVVM;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
    public interface Action {
    }
    public delegate State Reducer<State>(State state, Action action);
	public class Store<State> where State : new()
    {
        public void dispatch (Action action)
        {
            this._state = rootReducer.Invoke (this._state, action);
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

