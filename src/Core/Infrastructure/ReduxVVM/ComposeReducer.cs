using System;
using System.Collections.Generic;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Practices.ObjectBuilder2;

namespace XamarinFormsTester.UnitTests.ReduxVVM
{
	public class ComposeReducer<State> where State : new()
	{
		List<Tuple<FieldInfo, Delegate>> fieldReducers = new List<Tuple<FieldInfo, Delegate>>();

		public ComposeReducer<State> Part<T> (Expression<Func<State, T>> composer, Reducer<T> reducer)
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
				var result = new State();
				fieldReducers.ForEach(fieldReducer => {
					var prevState = fieldReducer.Item1.GetValue(state);
					var newState = fieldReducer.Item2.DynamicInvoke(prevState, action);
					object boxer = result; //boxing to allow the next line work for both reference and value objects
					fieldReducer.Item1.SetValue(boxer, newState);
					result = (State)boxer; // unbox, hopefully not too much performance penalty
				});
				return result;
			};
		}

	}


	
}

