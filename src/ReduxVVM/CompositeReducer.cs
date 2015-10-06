using System;
using System.Collections.Generic;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Linq.Expressions;
using System.Reflection;

namespace XamarinFormsTester.UnitTests.ReduxVVM
{
	public class CompositeReducer<State> where State : new()
	{
		List<Tuple<FieldInfo, Delegate>> fieldReducers = new List<Tuple<FieldInfo, Delegate>>();

        Func<State> initializer;

        public CompositeReducer ()
        {
            initializer = () => default(State);            
        }

        public CompositeReducer (Func<State> initializer)
        {
            this.initializer = initializer;
            
        }
		public CompositeReducer<State> Part<T> (Expression<Func<State, T>> composer, SimpleReducer<T> reducer)
		{
			return Part<T> (composer, reducer.Get ());
		}
		public CompositeReducer<State> Part<T> (Expression<Func<State, T>> composer, CompositeReducer<T> reducer) where T : new()
		{
			return Part<T> (composer, reducer.Get ());
		}
		public CompositeReducer<State> Part<T> (Expression<Func<State, T>> composer, Reducer<T> reducer)
		{
			var memberExpr = composer.Body as System.Linq.Expressions.MemberExpression;
			var member = (FieldInfo)memberExpr.Member;

			if (memberExpr == null)
				throw new ArgumentException(string.Format(
					"Expression '{0}' should be a field.",
					composer.ToString()));
			if (member == null)
				throw new ArgumentException(string.Format(
					"Expression '{0}' should be a constant expression",
					composer.ToString()));

			fieldReducers.Add (new Tuple<FieldInfo, Delegate> (member, reducer));
			return this;
		}
		public XamarinFormsTester.Infrastructure.ReduxVVM.Reducer<State> Get(){
			return delegate(State state, XamarinFormsTester.Infrastructure.ReduxVVM.Action action) {
                var result = action.GetType() == typeof(InitStoreAction) ? initializer() : state;
				foreach (var fieldReducer in fieldReducers) {
                    var prevState = action.GetType() == typeof(InitStoreAction) ? null : fieldReducer.Item1.GetValue(state);
					var newState = fieldReducer.Item2.DynamicInvoke(prevState, action);
					object boxer = result; //boxing to allow the next line work for both reference and value objects
					fieldReducer.Item1.SetValue(boxer, newState);
					result = (State)boxer; // unbox, hopefully not too much performance penalty
				}
				return result;
			};
		}

	}
	
}

