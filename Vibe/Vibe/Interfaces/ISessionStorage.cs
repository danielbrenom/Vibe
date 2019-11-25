using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Vibe.Models.Usuario;

namespace Vibe.Interfaces
{
    public interface ISessionStorage : INotifyPropertyChanged
    {
        bool Authenticated { get; set; }
        string AuthToken { get; set; }
        UsuarioStorageData StorageData { get; set; }
        Task<bool> SaveInStorage();
        Task<bool> LoadFromStorage();
    }
}
