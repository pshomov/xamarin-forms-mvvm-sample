using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Collections.Generic;
using System;

namespace XamarinFormsTester.Infrastructure.ReduxVVM
{
	public class Events<State>
	{
        Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate> ();
        public Events<State> When<Event>(Func<State, Event, State> handler) where Event : XamarinFormsTester.Infrastructure.ReduxVVM.Action, new() {
            handlers.Add (typeof(Event), handler);
            return this;
        }
        public XamarinFormsTester.Infrastructure.ReduxVVM.Reducer<State> Get(){
            return delegate(State state, XamarinFormsTester.Infrastructure.ReduxVVM.Action action) {
                if (handlers.ContainsKey(action.GetType())){
                    var handler = handlers [action.GetType ()];
                    return (State)handler.DynamicInvoke(state, action);
                } else {
                    return state;
                }
            };
        }
	}

}

