using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XamarinFormsTester
{
	public interface IServiceAPI
	{
        Task<List<DeviceInfo>> GetDevices ();
	}

}

