using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Vibe.Interfaces;
using Vibe.Models.Clientes;
using Vibe.Models.Usuario;
using Vibe.Services.Interfaces;
using Xamarin.Essentials;

namespace Vibe.Services
{
    public class ApiServices : IApiService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly MD5 _md5Formatter = MD5.Create();
        private readonly string _baseApiAddress = "https://vibeselecao.azurewebsites.net/api/";
        private ISessionStorage _sessionStorage => ServiceLocator.Current.GetInstance<ISessionStorage>();
        private IDataStore<Cliente> _dataStore => ServiceLocator.Current.GetInstance<IDataStore<Cliente>>();


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
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return false;
                }

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

                if (_sessionStorage.StorageData == null)
                    _sessionStorage.StorageData = new UsuarioStorageData();
                _sessionStorage.StorageData.StorageAuthData = new UsuarioAuthData
                {
                    UserAuthToken = jwtResponse.Value<string>("chave"),
                    UserIdentity = loginParams.senha,
                    UserLogin = loginParams.cpf
                };
                _sessionStorage.AuthToken = jwtResponse.Value<string>("chave");

                await _sessionStorage.SaveInStorage();
                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return false;
            }
        }

        public async Task<bool> LoginFromCache()
        {
            try
            {
                if (_sessionStorage.StorageData.StorageAuthData == null)
                    throw new Exception("Cache vazio");
                _sessionStorage.AuthToken = _sessionStorage.StorageData.StorageAuthData.UserAuthToken;
                _sessionStorage.Authenticated = true;
                if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    return true;
                }

                var loginParams = new UsuarioLogin()
                {
                    cpf = _sessionStorage.StorageData.StorageAuthData.UserLogin,
                    senha = _sessionStorage.StorageData.StorageAuthData.UserIdentity
                };
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

                _sessionStorage.StorageData.StorageAuthData = new UsuarioAuthData
                {
                    UserAuthToken = jwtResponse.Value<string>("chave"),
                    UserIdentity = loginParams.senha,
                    UserLogin = loginParams.cpf
                };
                _sessionStorage.AuthToken = jwtResponse.Value<string>("chave");
                await _sessionStorage.SaveInStorage();
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
            var token = _sessionStorage.AuthToken;
            if (token is null)
            {
                return null;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetStringAsync(_baseApiAddress + "Usuario/" + cpf);
            var userInfo = JsonConvert.DeserializeObject<Usuario>(response);
            _sessionStorage.StorageData.UserInfo = userInfo;
            await _sessionStorage.SaveInStorage();
            return await Task.FromResult(userInfo);
        }

        public async Task<bool> LoadClientes()
        {
            var token = _sessionStorage.AuthToken;
            if (token is null)
            {
                return false;
            }

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetStringAsync(_baseApiAddress + "Cliente");
            var clientList = JsonConvert.DeserializeObject<List<Cliente>>(response);
            await _dataStore.ClearData();
            _sessionStorage.StorageData.ClienteListData = clientList;
            await _sessionStorage.SaveInStorage();
            foreach (var client in clientList)
            {
                await _dataStore.AddItemAsync(client);
            }

            return await Task.FromResult(true);
        }

        public async Task<ClienteComplemento> LoadClienteComplemento(string id)
        {
            try
            {
                var token = _sessionStorage.AuthToken;
                if (token is null)
                {
                    return null;
                }

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.GetStringAsync(_baseApiAddress + "Cliente/" + id);
                var clienteInfo = JsonConvert.DeserializeObject<ClienteComplemento>(response);
                return await Task.FromResult(clienteInfo);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                return await Task.FromResult(new ClienteComplemento());
            }
        }

        public string HashPassword(string password)
        {
            var passHash = _md5Formatter.ComputeHash(Encoding.UTF8.GetBytes(password));
            return passHash.Aggregate("", (current, b) => current + b.ToString());
        }
    }
}