using NUnit.Framework;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Collections.Generic;
using System;

namespace XamarinFormsTester.UnitTests.ReduxVVM
{

    [TestFixture]
    public class StoreTests
    {

        [Test]
        public void should_register_root_reducer(){
            
            Reducer<List<string>> reducer = (List<string>state, XamarinFormsTester.Infrastructure.ReduxVVM.Action action) =>  {                
                var newState = new List<string> (state);

                switch(action.GetType().Name){
                    case "ItemAdded":
                        ItemAdded concreteEv = (ItemAdded)action;
                        newState.Add(concreteEv.item);
                        break;
                    default:
                        break;
                }
                return newState;
            };
            var store = new Store<List<String>> (reducer, new List<string>{ "Use ReduxVVM" });
            store.dispatch (new ItemAdded{item = "Read the Redux docs"});

            CollectionAssert.AreEqual(store.getState(), new List<string>{"Use ReduxVVM", "Read the Redux docs"});
        }

        [Test]
        public void should_register_root_reducer_with_builder(){

            var reducer = new Events<List<string>>()
                .When<ItemAdded>((state, action) => {
                    var newSatte = new List<String> (state);
                    newSatte.Add(action.item);
                    return newSatte;
                })
                .Get();
            var store = new Store<List<String>> (reducer, new List<string>{ "Use ReduxVVM" });
            store.dispatch (new ItemAdded{item = "Read the Redux docs"});

            CollectionAssert.AreEqual(store.getState(), new List<string>{"Use ReduxVVM", "Read the Redux docs"});
        }

        [Test]
        public void should_return_same_state_when_command_not_for_that_reducer(){

            var reducer = new Events<List<string>>()
                .Get();
            var store = new Store<List<String>> (reducer, new List<string>{ "Use ReduxVVM" });
            store.dispatch (new ItemAdded{item = "Read the Redux docs"});

            CollectionAssert.AreEqual(store.getState(), new List<string>{"Use ReduxVVM"});
        }
    }
}
