using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Practices.Unity;

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

        public Task PushAsync<ModelType>(Action<ModelType> configureModel) where ModelType : class
        {
            var view = this.DataboundPageFromModelType<ModelType>();
            configureModel.Invoke ((ModelType)view.BindingContext);
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
