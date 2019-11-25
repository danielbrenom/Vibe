using System;
using System.Threading.Tasks;
using Vibe.Models.Clientes;
using Vibe.Models.Usuario;

namespace Vibe.Interfaces
{
    public interface IApiService
    {
        Task<bool> Login(string cpf, string password);
        Task<bool> LoginFromCache();
        Task<bool> RegisterUser(string cpf, string password, DateTime nascimento, string nome);
        Task<Usuario> GetUserInfo(string cpf);
        Task<bool> LoadClientes();
        Task<ClienteComplemento> LoadClienteComplemento(string id);
    }
}
