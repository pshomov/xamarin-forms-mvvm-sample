using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Collections.Generic;
using System;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
	public class SimpleReducer<State>
	{
        Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate> ();

        Func<State> initializer;

        public SimpleReducer ()
        {            
            initializer = () => default(State);
        }
        public SimpleReducer (Func<State> initializer)
        {
            this.initializer = initializer;            
        }
        public SimpleReducer<State> When<Event>(Func<State, Event, State> handler) where Event : XamarinFormsTester.Infrastructure.ReduxVVM.Action, new() {
            handlers.Add (typeof(Event), handler);
            return this;
        }
        public XamarinFormsTester.Infrastructure.ReduxVVM.Reducer<State> Get(){
            return delegate(State state, XamarinFormsTester.Infrastructure.ReduxVVM.Action action) {
                var prevState = action.GetType() == typeof(InitStoreAction) ? initializer() : state;
                if (handlers.ContainsKey(action.GetType())){
                    var handler = handlers [action.GetType ()];
                    return (State)handler.DynamicInvoke(prevState, action);
                } else {
                    return prevState;
                }
            };
        }
	}

}

