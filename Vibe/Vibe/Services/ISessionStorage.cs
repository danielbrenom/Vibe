using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Vibe.Services
{
    public interface ISessionStorage : INotifyPropertyChanged
    {
        bool Authenticated { get; set; }
    }
}
