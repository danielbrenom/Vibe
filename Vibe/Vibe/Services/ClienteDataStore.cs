using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibe.Models;
using Vibe.Models.Clientes;

namespace Vibe.Services
{
    class ClienteDataStore : IDataStore<Cliente>
    {
        public List<Cliente> ClientList;

        public ClienteDataStore()
        {

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

        public async Task<Cliente> GetItemAsync(string id)
        {
            return await Task.FromResult(ClientList.FirstOrDefault(s => s.id == id));
        }

        public async Task<IEnumerable<Cliente>> GetItemsAsync(bool forceRefresh = false)
        {
            
            return await Task.FromResult(ClientList);
        }
    }
}
