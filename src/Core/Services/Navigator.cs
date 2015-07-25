using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Practices.Unity;
using PubSub;
using System.Collections.Generic;

namespace XamarinFormsTester.Services
{
    
    public class Navigator : INavigator {
        INavigation nav;
        IUnityContainer container;

        public Navigator (INavigation nav, IUnityContainer resolver)
        {
            this.container = resolver;
            this.nav = nav;
            
        }

        public Task PushAsync<ModelType>() where ModelType : class
        {
            var view = this.DataboundPageFromModelType<ModelType>();
            return nav.PushAsync(view);
        }

        public Task PushAsync<ModelType>(Action<ModelType> configureModel, List<Type> updateOnEvents) where ModelType : class
        {
            var view = this.DataboundPageFromModelType<ModelType>();
            configureModel.Invoke ((ModelType)view.BindingContext);
            foreach (var @event in updateOnEvents) {
                view.Subscribe(@event, e => {
                    configureModel.Invoke ((ModelType)view.BindingContext);
                });
            }
            return nav.PushAsync(view);
        }

        private Page DataboundPageFromModelType<ModelType>() where ModelType : class
        {
            var viewType = ModeViewViewModelConnection.ViewFromViewModel(typeof(ModelType));
            var viewModel = PrepareModel<ModelType> ();
            Page view = (Page)Activator.CreateInstance(viewType);
            view.BindingContext = viewModel;
            return view;
        }

        ModelType PrepareModel<ModelType> () where ModelType : class
        {
            var viewModel = container.Resolve<ModelType> ();
            var navAware = viewModel as INavigationAware;
            if (navAware != null) {
                navAware.OnNavigateTo ();
            }
            return viewModel;
        }
    }
}
