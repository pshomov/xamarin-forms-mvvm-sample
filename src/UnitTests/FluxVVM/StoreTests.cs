using NUnit.Framework;
using XamarinFormsTester.Infrastructure.FluxVVM;
using System.Collections.Generic;
using System;

namespace XamarinFormsTester.UnitTests.FluxVVM
{
    [TestFixture]
    public class StoreTests
    {
        enum Actions {
            ADD_ITEM,
            REMOVE_ITEM
        }

        [Test]
        public void should_register_root_reducer(){
            
            Reducer<List<string>, Actions> reducer = (List<string>state, XamarinFormsTester.Infrastructure.FluxVVM.Action<Actions> action) =>  {                
                var newState = new List<string> (state);
                newState.Add("Read the Redux docs");
                return newState;
            };
            var store = new Store<List<String>, Actions> (reducer, new List<string>{ "Use FluxVVM" });
            store.dispatch (new XamarinFormsTester.Infrastructure.FluxVVM.Action<Actions>{type = Actions.ADD_ITEM});

            CollectionAssert.AreEqual(store.getState(), new List<string>{"Use FluxVVM", "Read the Redux docs"});
        }
    }
}

