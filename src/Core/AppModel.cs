using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;
using PubSub;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace XamarinFormsTester
{
    public static class Helpers {
        public static Type updateOn<T>(){
            return typeof(T);
        }
    }

    public class AppModel
    {
        INavigator navigator;

        IServiceAPI api;
        bool pullingListOfDevices;

        List<DeviceInfo> Devices;

        public AppModel (INavigator navigator, IServiceAPI api)
        {
            this.api = api;
            this.navigator = navigator;
            pullingListOfDevices = false;
            this.Subscribe<UpdateDeviceList> (_ => OnUpdateDeviceList ());
        }

        System.Collections.ObjectModel.ObservableCollection<DeviceSummary> DTO2DeviceListPageViewModel (List<DeviceInfo> devices)
        {
            return new System.Collections.ObjectModel.ObservableCollection<DeviceSummary>(devices.Select (d => new DeviceSummary {Name = d.Name, Location = d.Location}));
        }

        public void Start ()
        {
            this.Publish (new UpdateDeviceList ());
            navigator.PushAsync<DeviceListPageViewModel> ((vm) => {
                vm.Pulling = pullingListOfDevices;
                if (!pullingListOfDevices){
                    vm.Devices = DTO2DeviceListPageViewModel(Devices);
                }
            }, new List<Type> {Helpers.updateOn<DeviceListRetrievalStarted>(), Helpers.updateOn<DeviceListUpdated>()});
        }

        public void OnSelectDevice(DeviceSelected cmd)
        {
            navigator.PushAsync<DeviceDetailsPageViewModel> ((vm) => {
                var device = Devices.Single(d => d.Id == cmd.deviceId);
                vm.Title = device.Name;
            }, new List<Type> {Helpers.updateOn<DeviceInfoUpdated>()});
        }

        public async void OnUpdateDeviceList(){
            var token = api.GetDevices ();
            pullingListOfDevices = true;
            Devices = await token;
            pullingListOfDevices = false;
            this.Publish (new DeviceListUpdated (){ Devices = Devices });
        }

    }
}

