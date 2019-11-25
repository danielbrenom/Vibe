using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PCLStorage;
using Vibe.Annotations;
using Vibe.Interfaces;
using Vibe.Models.Usuario;

namespace Vibe.Services
{
    internal class SessionStorage : ISessionStorage
    {
        public bool Authenticated { get;set; }
        public string AuthToken { get; set; }
        public UsuarioStorageData StorageData { get; set; }
        

        public async Task<bool> SaveInStorage()
        {
            if (StorageData.StorageAuthData == null)
                return await Task.FromResult(false);
            var rootFolder = FileSystem.Current.LocalStorage;
            var folder = await rootFolder.CreateFolderAsync("Storage", CreationCollisionOption.OpenIfExists);
            var file = await folder.CreateFileAsync("cache.dat", CreationCollisionOption.ReplaceExisting);
            var userData = JsonConvert.SerializeObject(StorageData);
            await file.WriteAllTextAsync(userData);
            return await Task.FromResult(true);
        }

        public async Task<bool> LoadFromStorage()
        {
            var rootFolder = FileSystem.Current.LocalStorage;
            var storageFolder = await rootFolder.CreateFolderAsync("Storage", CreationCollisionOption.OpenIfExists);
            var fileExists = await storageFolder.CheckExistsAsync("cache.dat");
            if (fileExists != ExistenceCheckResult.FileExists)
            {
                StorageData = new UsuarioStorageData();
                return await Task.FromResult(false);
            }

            try
            {
                var file = await storageFolder.GetFileAsync("cache.dat");
                var data = await file.ReadAllTextAsync();
                StorageData = JsonConvert.DeserializeObject<UsuarioStorageData>(data);
                return await Task.FromResult(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return await Task.FromResult(false);
            }
        }

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}