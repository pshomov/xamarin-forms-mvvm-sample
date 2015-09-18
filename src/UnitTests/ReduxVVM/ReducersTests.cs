using System;
using NUnit.Framework;
using System.Collections.Generic;
using XamarinFormsTester.Infrastructure.ReduxVVM;

namespace XamarinFormsTester.UnitTests.ReduxVVM
{
	public class TopicSet : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
		public string topic;
	}
	public class FilterVisibility : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
		public bool visible;
	}

	public struct AppStore {
		public String redditTopic;
		public bool visibility;

		public override	String ToString(){
			return String.Format("topic:{0}, visibility {1}", redditTopic, visibility);
		}
	}

	[TestFixture]
	public class ReducersTests
	{
		[Test]
		public void should_prvide_way_to_combine_reducers(){
			var topicReducer = new Events<string> ().When<TopicSet> ((s, e) => e.topic);
			var visibilityReducer = new Events<bool> ().When<FilterVisibility> ((s, e) => e.visible);
			var reducer = new ComposeReducer<AppStore> ()
				.Part(s => s.redditTopic, topicReducer)
				.Part(s => s.visibility, visibilityReducer)
				.Get();
			var store = new Store<AppStore>(reducer, new AppStore(){redditTopic = "react", visibility = false});
			store.dispatch (new TopicSet{topic = "Redux is awesome"});
			store.dispatch (new FilterVisibility{visible = true});

			Assert.AreEqual(new AppStore{redditTopic = "Redux is awesome", visibility = true}, store.getState());

		}

		struct Address
		{
			public string streetNr;
			public string city;
		}

		enum DeliveryMethod
		{
			REGULAR,
			GUARANTEED
		}

		struct Destination
		{
			public Address addr;
			public DeliveryMethod deliver;
		}
		struct Order
		{
			public string name;
			public Address origin;
			public Destination destination;
		}
		struct SetOrigin : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
			public Address newAddress;
		}
		struct SetDestination : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
			public Address newAddress;
		}
		struct BehindSchedule : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
		}
		struct SetDelivery : XamarinFormsTester.Infrastructure.ReduxVVM.Action{
			public DeliveryMethod method;
		}


		[Test]
		public void should_prvide_way_to_create_deep_hierarchy_of_reducers(){
			var originReducer = new Events<Address> ().When<SetOrigin> ((s, e) => e.newAddress);
			var destinationReducer = new ComposeReducer<Destination> ()
				.Part (s => s.deliver, new Events<DeliveryMethod> ().When<BehindSchedule>((s, a) => DeliveryMethod.REGULAR).When<SetDelivery>((_, a) => a.method))
				.Part (s => s.addr, new Events<Address> ().When<SetDestination>((s,a) => a.newAddress));
			var orderReducer = new ComposeReducer<Order> ()
				.Part(s => s.origin, originReducer)
				.Part(s => s.destination, destinationReducer);
			var store = new Store<Order>(orderReducer, new Order(){});
			store.dispatch (new SetOrigin{newAddress = new Address{streetNr = "Laugavegur 26", city="Reykjavík"}});
			store.dispatch (new SetDestination{newAddress = new Address{streetNr = "5th Street", city="New York"}});
			store.dispatch (new SetDelivery{method = DeliveryMethod.GUARANTEED});

			store.dispatch (new BehindSchedule());

			Assert.AreEqual(new Order{
				origin = new Address{streetNr = "Laugavegur 26", city = "Reykjavík"}, 
				destination = new Destination{addr = new Address{streetNr = "5th Street", city = "New York"}, deliver = DeliveryMethod.REGULAR}
			}, store.getState());

		}
	}

}

