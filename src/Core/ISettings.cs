using System;
using Xamarin.Forms;

namespace XamarinFormsTester
{
	public interface ISettings
	{
        bool Exists (string state);

        T Get<T> (string state);
	}

}
