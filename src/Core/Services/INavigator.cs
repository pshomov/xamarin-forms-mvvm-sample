using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;

namespace XamarinFormsTester.Services
{
    public interface INavigator
    {
        Task PushAsync<Model> () where Model : class;
    }
}

