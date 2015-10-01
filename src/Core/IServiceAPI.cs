using System;
using XamarinFormsTester.Services;
using XamarinFormsTester.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XamarinFormsTester
{
    public class UserInfo {
        public string Username;
        public string HomeCity;
    }
	public interface IServiceAPI
	{
        Task<List<DeviceInfo>> GetDevices ();
        Task<UserInfo> AuthUser (string username, string password);
	}

}

