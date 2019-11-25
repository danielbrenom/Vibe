using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Practices.ServiceLocation;
using Vibe.Interfaces;
using Xamarin.Forms;
using Vibe.Models.Clientes;
using Vibe.Services;
using Vibe.Services.Interfaces;
using Xamarin.Essentials;

namespace Vibe.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public IDataStore<Cliente> DataStore => ServiceLocator.Current.GetInstance<IDataStore<Cliente>>();
        public IApiService ApiServices => ServiceLocator.Current.GetInstance<IApiService>();

        #region Properties

        private bool _authenticated;

        public bool Authenticated
        {
            get => _authenticated;
            set => SetProperty(ref _authenticated, value);
        }

        private bool _isInternetAvailable;

        public bool InternetAvailable
        {
            get => _isInternetAvailable;
            set => SetProperty(ref _isInternetAvailable, value);
        }

        private bool _isBusy;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        string _title = string.Empty;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        #endregion
        public BaseViewModel()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            InternetAvailable = Connectivity.NetworkAccess != NetworkAccess.Internet;
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            InternetAvailable = e.NetworkAccess != NetworkAccess.Internet;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;

            changed?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}