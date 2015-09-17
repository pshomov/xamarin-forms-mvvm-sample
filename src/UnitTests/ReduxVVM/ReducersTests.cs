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
			var topicReducer = new Events<string> ().When<TopicSet> ((s, e) => e.topic).Get ();
			var visibilityReducer = new Events<bool> ().When<FilterVisibility> ((s, e) => e.visible).Get ();
			var reducer = new ComposeReducer<AppStore> ()
				.Part(s => s.redditTopic, topicReducer)
				.Part(s => s.visibility, visibilityReducer)
				.Get();
			var store = new Store<AppStore>(reducer, new AppStore(){redditTopic = "react", visibility = false});
			store.dispatch (new TopicSet{topic = "Redux is awesome"});
			store.dispatch (new FilterVisibility{visible = true});

			Assert.AreEqual(new AppStore{redditTopic = "Redux is awesome", visibility = true}, store.getState());

		}
	}
}

