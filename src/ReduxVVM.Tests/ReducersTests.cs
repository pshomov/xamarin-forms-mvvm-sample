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
			var topicReducer = new SimpleReducer<string> ().When<TopicSet> ((s, e) => e.topic);
			var visibilityReducer = new SimpleReducer<bool> ().When<FilterVisibility> ((s, e) => e.visible);
            var reducer = new CompositeReducer<AppStore> (() => new AppStore(){redditTopic = "react", visibility = false})
				.Part(s => s.redditTopic, topicReducer)
				.Part(s => s.visibility, visibilityReducer);
			var store = new Store<AppStore>(reducer);
			store.Dispatch (new TopicSet{topic = "Redux is awesome"});
			store.Dispatch (new FilterVisibility{visible = true});

			Assert.AreEqual(new AppStore{redditTopic = "Redux is awesome", visibility = true}, store.GetState());

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
			var originReducer = new SimpleReducer<Address> ().When<SetOrigin> ((s, e) => e.newAddress);
			var destinationReducer = new CompositeReducer<Destination> ()
				.Part (s => s.deliver, new SimpleReducer<DeliveryMethod> ().When<BehindSchedule>((s, a) => DeliveryMethod.REGULAR).When<SetDelivery>((_, a) => a.method))
				.Part (s => s.addr, new SimpleReducer<Address> ().When<SetDestination>((s,a) => a.newAddress));
            var orderReducer = new CompositeReducer<Order> (() => new Order(){})
				.Part(s => s.origin, originReducer)
				.Part(s => s.destination, destinationReducer);
			var store = new Store<Order>(orderReducer);
			store.Dispatch (new SetOrigin{newAddress = new Address{streetNr = "Laugavegur 26", city="Reykjavík"}});
			store.Dispatch (new SetDestination{newAddress = new Address{streetNr = "5th Street", city="New York"}});
			store.Dispatch (new SetDelivery{method = DeliveryMethod.GUARANTEED});

			store.Dispatch (new BehindSchedule());

			Assert.AreEqual(new Order{
				origin = new Address{streetNr = "Laugavegur 26", city = "Reykjavík"}, 
				destination = new Destination{addr = new Address{streetNr = "5th Street", city = "New York"}, deliver = DeliveryMethod.REGULAR}
			}, store.GetState());

		}
	}

}

