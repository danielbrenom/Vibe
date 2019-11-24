using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Vibe.Models.Usuario
{
    public class UsuarioAuthData
    {
        public async Task<string> GetAuthToken()
        {
            try
            {
                return await SecureStorage.GetAsync("auth_token");
            }
            catch
            {
                return null;
            }
        }

        public async Task SetAuthToken(string token)
        {
            try
            {
                await SecureStorage.SetAsync("auth_token", token);
            }
            catch
            {
                return;
            }
        }
    }
}