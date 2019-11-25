using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Vibe.Interfaces;
using Vibe.Models.Usuario;
using Xamarin.Forms;
using static Xamarin.Forms.Application;

namespace Vibe.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private string cpf, password;
        private bool _loginBusy;
        private Usuario info;
        private ISessionStorage _sessionStorage;
        public Command AuthenticateCommand { get; set; }
        public Command LogoutCommand { get; set; }
        #region Properties

        public string CPF
        {
            get => cpf;
            set => SetProperty(ref cpf, value);
        }

        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        public bool LoginBusy
        {
            get => _loginBusy;
            set => SetProperty(ref _loginBusy, value);
        }

        public Usuario UserInfo
        {
            get => info;
            set => SetProperty(ref info, value);
        }

        #endregion

        public AboutViewModel(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _sessionStorage.PropertyChanged += SessionStorageOnPropertyChanged;
            _sessionStorage.Authenticated = false;
            Title = "Usuario";
            AuthenticateCommand = new Command(LoginCommand);
            LogoutCommand = new Command(Logout);
            LoginBusy = false;
            UserInfo = new Usuario {nome = "Carregando...", cpf = "00000000000", nascimento = DateTime.Now};
        }

        private async void LoginCommand()
        {
            if (LoginBusy) return;
            LoginBusy = true;
            if (string.IsNullOrEmpty(CPF) || string.IsNullOrEmpty(Password))
            {
                await Current.MainPage.DisplayAlert("Atenção", "Os campos não podem estar vazios!", "OK");
                return;
            }

            if (CPF.Length > 14 || CPF.Length < 14)
            {
                await Current.MainPage.DisplayAlert("Atenção", "O campo CPF deve possuir 11 digitos!", "OK");
                return;
            }

            try
            {
                var result = await ApiServices.Login(CPF, Password);
                Authenticated = _sessionStorage.Authenticated = result;

                if (result)
                {
                    UserInfo = await ApiServices.GetUserInfo(CPF);
                }
                else
                {
                    await Current.MainPage.DisplayAlert("Erro", "Login ou senha incorretos", "Ok");
                }
            }
            catch (Exception exc)
            {
                await Current.MainPage.DisplayAlert("Erro", exc.Message, "Ok");
            }
            finally
            {
                LoginBusy = false;
            }
        }

        public async Task<bool> LoginOnStart()
        {
            if (_sessionStorage.StorageData.StorageAuthData == null) return await Task.FromResult(true);
            var result = await ApiServices.LoginFromCache();
            UserInfo = _sessionStorage.StorageData.UserInfo;
            Authenticated = _sessionStorage.Authenticated = result;
            return await Task.FromResult(true);
        }

        public void Logout()
        {
            Authenticated = _sessionStorage.Authenticated = false;
            _sessionStorage.StorageData = null;
        }

        private void SessionStorageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Authenticated))
                Authenticated = _sessionStorage.Authenticated;
        }
    }
}