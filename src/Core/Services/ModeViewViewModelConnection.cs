using System;
using XLabs.Ioc;
using Xamarin.Forms;
using System.Linq;

namespace XamarinFormsTester
{
    public static class ModeViewViewModelConnection
    {
        public static Page ResolvePage<T>(this IResolver container)
        {
            return (Page)container.Resolve(ViewFromViewModel(typeof(T)));
        }

        public static Page ResolvePage(this IResolver container, Type viewModel)
        {
            var type = ViewFromViewModel(viewModel);
            return (Page)container.Resolve(type);
        }

        public static Type ViewFromViewModel(Type model)
        {
            var fullName = model.FullName;
            var nameParts = fullName.Split('.');
            var modelName = nameParts[nameParts.Count() - 1];
            var viewName = String.Join(".", nameParts.Take(nameParts.Count() - 2)) + ".Views." + modelName.Replace("ViewModel", string.Empty);
            var viewType = Type.GetType(viewName);
            if (viewType == null)
            {
                throw new Exception("Cannot locate view " + viewName);
            }

            return viewType;
        }    
    }
}

