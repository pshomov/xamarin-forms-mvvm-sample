﻿using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinFormsTester.Services
{
    public interface INavigator
    {
        Task PushAsync<Model> () where Model : class;
        Task PushAsync<Model> (Action<Model> configureModel) where Model : class;
    }
}

