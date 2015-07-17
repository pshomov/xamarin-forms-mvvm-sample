using System;
using System.Collections.ObjectModel;

namespace XamarinFormsTester.ViewModels
{
    public class DeviceListPageViewModel : ViewModel
    {
        public ObservableCollection<DeviceSummary> Devices { get; set;}
        public Boolean Pulling { get; set;}
    }
}

