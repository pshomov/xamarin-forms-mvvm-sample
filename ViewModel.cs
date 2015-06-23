using System;
using Xamarin.Forms;

namespace XamarinFormsTester
{
    public class ViewModel {
        public Page Page { get {return new Page(){BindingContext = this};}}        
    }
    
}
