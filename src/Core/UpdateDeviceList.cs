using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;
using PubSub;
using System.Collections.Generic;

namespace XamarinFormsTester
{
	public class UpdateDeviceList
	{
	}

    public class DeviceListRetrievalStarted 
    {     
    }

    public class DeviceListUpdated 
    {
        public List<DeviceInfo> Devices {get;set;}
    }
}

