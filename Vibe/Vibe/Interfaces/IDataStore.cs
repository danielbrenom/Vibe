using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vibe.Services.Interfaces
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(string id);
        Task<bool> ReplaceListAsync(List<T> list);
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
        Task<bool> ClearData();
    }
}
