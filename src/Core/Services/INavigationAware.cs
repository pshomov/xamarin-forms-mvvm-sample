using System;
using System.Threading.Tasks;

namespace XamarinFormsTester.Services
{
    public interface INavigationAware
    {
        Task OnNavigateTo();
    }    
    
}
