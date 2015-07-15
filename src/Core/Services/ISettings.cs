using System;
using Xamarin.Forms;

namespace XamarinFormsTester
{
	public interface ISettings
	{
        T Get<T> (string state);
	}

}
