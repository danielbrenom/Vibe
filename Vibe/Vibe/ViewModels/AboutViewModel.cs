using System;
using System.ComponentModel;
using System.Linq;
using System.Net.Security;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Vibe.Models.Usuario;
using Vibe.Services;
using Vibe.Views;
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

        private bool _authenticated;
        public bool Authenticated
        {
            get => _authenticated;
            set { SetProperty(ref _authenticated, value); }
        }

        #endregion

        public AboutViewModel(ISessionStorage sessionStorage)
        {
            _sessionStorage = sessionStorage;
            _sessionStorage.PropertyChanged+= SessionStorageOnPropertyChanged;
            Authenticated = false;
            Title = "Usuario";
            AuthenticateCommand = new Command(LoginCommand);
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
                var result = await ApiServices.Login(cpf, password);
                if (result)
                {
                    await Current.MainPage.DisplayAlert("Sucesso", "Usuario autenticado", "Ok");
                    UserInfo = await ApiServices.GetUserInfo(CPF);
                }
                else
                {
                    await Current.MainPage.DisplayAlert("Erro", "Login ou senha incorretos", "Ok");
                }

                _sessionStorage.Authenticated = result;
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

        private void SessionStorageOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Authenticated))
                Authenticated = _sessionStorage.Authenticated;
        }
    }
}