using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vibe.Models.Clientes;
using Vibe.Models.Usuario;

namespace Vibe.Services
{
    public class ApiServices
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly MD5 _md5Formatter = MD5.Create();
        private readonly UsuarioAuthData _userAuthData = new UsuarioAuthData();
        private readonly string _baseApiAddress = "https://vibeselecao.azurewebsites.net/api/";

        public async Task<bool> RegisterUser(string cpf, string password, DateTime nascimento, string nome)
        {
            var newUser = new Usuario {cpf = cpf, nascimento = nascimento, nome = nome, senha = HashPassword(password)};

            var jsonContent = JsonConvert.SerializeObject(newUser);
            HttpContent httpContent = new StringContent(jsonContent);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var httpPost = await _httpClient.PostAsync(
                _baseApiAddress + "Usuario", httpContent);
            return httpPost.IsSuccessStatusCode;
        }

        public async Task<bool> Login(string cpf, string password)
        {
            try
            {
                var loginParams = new UsuarioLogin() {cpf = cpf, senha = HashPassword(password)};
                var jsonContent = JsonConvert.SerializeObject(loginParams);
                HttpContent httpContent = new StringContent(jsonContent);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var httpPost = await _httpClient.PostAsync(
                    _baseApiAddress + "Autenticacao", httpContent);
                var response = await httpPost.Content.ReadAsStringAsync();
                JObject jwtResponse = JsonConvert.DeserializeObject<dynamic>(response);
                if (string.IsNullOrEmpty(jwtResponse.Value<string>("chave")))
                {
                    return false;
                }

                await _userAuthData.SetAuthToken(jwtResponse.Value<string>("chave"));
                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return false;
            }
        }

        public async Task<Usuario> GetUserInfo(string cpf)
        {
            var token = await _userAuthData.GetAuthToken();
            if (token is null)
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetStringAsync(_baseApiAddress + "Usuario/" + cpf);
            var userInfo = JsonConvert.DeserializeObject<Usuario>(response);
            return await Task.FromResult(userInfo);
        }

        public async Task<ClienteList> LoadClientes()
        {
            var token = await _userAuthData.GetAuthToken();
            if (token is null)
            {
                return null;
            }
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetStringAsync(_baseApiAddress + "Cliente");
            var clientList = JsonConvert.DeserializeObject<ClienteList>(response);
            return await Task.FromResult(clientList);
        }

        public string HashPassword(string password)
        {
            var passHash = _md5Formatter.ComputeHash(Encoding.UTF8.GetBytes(password));
            return passHash.Aggregate("", (current, b) => current + b.ToString());
        }
    }
}