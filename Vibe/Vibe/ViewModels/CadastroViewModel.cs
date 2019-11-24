using System;
using System.Collections.Generic;
using System.Text;
using Vibe.Services;
using Xamarin.Forms;
using static Xamarin.Forms.Application;

namespace Vibe.ViewModels
{
    public class CadastroViewModel : BaseViewModel
    {
        private string _nome, _cpf, _senha;
        private DateTime _nascimento;
        private ApiServices _apiServices = new ApiServices();

        #region Properties

        public string Nome
        {
            get => _nome;
            set
            {
                _nome = value;
                SetProperty(ref _nome, value);
            }
        }

        public string Cpf
        {
            get => _cpf;
            set
            {
                _cpf = value;
                SetProperty(ref _cpf, value);
            }
        }

        public string Senha
        {
            get => _senha;
            set
            {
                _senha = value;
                SetProperty(ref _senha, value);
            }
        }

        public DateTime Nascimento
        {
            get => _nascimento;
            set
            {
                _nascimento = value;
                SetProperty(ref _nascimento, value);
            }
        }

        #endregion

        public Command CadastroCommand { get; set; }

        public CadastroViewModel()
        {
            Title = "Cadastro";
            Nascimento = DateTime.Now;
            CadastroCommand = new Command(CadastrarCommand);
            
        }

        private async void CadastrarCommand()
        {
            var registerSuccess = await _apiServices.RegisterUser(_cpf, _senha, _nascimento, _nome);
            if (registerSuccess)
            {
               await Current.MainPage.DisplayAlert("Sucesso", "Usuário cadastrado", "Ok");
            }
            else
            {
               await Current.MainPage.DisplayAlert("Erro", "Ocorreu um erro ao cadastrar o usuário", "Ok");
            }
        }
    }
}