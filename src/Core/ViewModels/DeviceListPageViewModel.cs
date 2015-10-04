using System;
using System.Collections.ObjectModel;
using XamarinFormsTester.Infrastructure.ReduxVVM;
using System.Linq;

namespace XamarinFormsTester.ViewModels
{
    public class DeviceListPageViewModel : ViewModel
    {
        public ObservableCollection<DeviceSummary> Devices { get; set;}
        public Boolean Pulling { get; set;}

        public DeviceListPageViewModel (Store<AppState> store)
        {
            store.Subscribe ((s) => {
                Pulling = s.DevicePage.inProgress;
                Devices = new ObservableCollection<DeviceSummary>(s.DevicePage.Devices.Select(d => new DeviceSummary{Name = d.Name, Location = d.Location}));
            });
        }
    }
}

