using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vibe.Models.Clientes;
using Vibe.Services.Interfaces;

namespace Vibe.Services
{
    public class ClienteDataStore : IDataStore<Cliente>
    {
        public List<Cliente> ClientList;

        public ClienteDataStore()
        {
            ClientList = new List<Cliente>();
            var list = new List<Cliente>()
            {
                new Cliente{cpf = "000", especial = true, id = "1", nome = "Nome"}
            };
            ClientList.AddRange(list);
        }

        public async Task<bool> AddItemAsync(Cliente item)
        {
            ClientList.Add(item);
            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Cliente item)
        {
            var oldItem = ClientList.FirstOrDefault(arg => arg.id == item.id);
            ClientList.Remove(oldItem);
            ClientList.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = ClientList.FirstOrDefault(arg => arg.id == id);
            ClientList.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<bool> ReplaceListAsync(List<Cliente> list)
        {
            ClientList.Clear();
            foreach (var cliente in list)
            {
                ClientList.Add(cliente);
            }
            return await Task.FromResult(true);
        }

        public async Task<Cliente> GetItemAsync(string id)
        {
            return await Task.FromResult(ClientList.FirstOrDefault(s => s.id == id));
        }

        public async Task<IEnumerable<Cliente>> GetItemsAsync(bool forceRefresh = false)
        {
            
            return await Task.FromResult(ClientList);
        }

        public async Task<bool> ClearData()
        {
            ClientList.Clear();
            return await Task.FromResult(true);
        }
    }
}
