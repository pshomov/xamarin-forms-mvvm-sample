using XamarinFormsTester.Infrastructure.FluxVVM;
using System.Collections.Generic;
using System;

namespace XamarinFormsTester.Infrastructure.FluxVVM
{
	public class Events<State>
	{
        Dictionary<Type, Delegate> handlers = new Dictionary<Type, Delegate> ();
        public Events<State> When<Event>(Func<State, Event, State> handler) where Event : Infrastructure.FluxVVM.Action, new() {
            handlers.Add (typeof(Event), handler);
            return this;
        }
        public Infrastructure.FluxVVM.Reducer<State> Get(){
            return delegate(State state, XamarinFormsTester.Infrastructure.FluxVVM.Action action) {
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

