using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;

namespace XamarinFormsTester.Services
{
    
    public class Navigator : INavigator {
        INavigation nav;
        IResolver container;

        public Navigator (INavigation nav, IResolver resolver)
        {
            this.container = resolver;
            this.nav = nav;
            
        }

        public Task PushAsync<ModelType>() where ModelType : class
        {
            var view = this.DataboundPageFromModelType<ModelType>();
            return nav.PushAsync(view);
        }

        private Page DataboundPageFromModelType<ModelType>() where ModelType : class
        {
            var viewType = ModeViewViewModelConnection.ViewFromViewModel(typeof(ModelType));
            var viewModel = container.Resolve<ModelType>();
            Page view = (Page)Activator.CreateInstance(viewType);
            view.BindingContext = viewModel;
            return view;
        }

    }
}
