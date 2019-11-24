using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vibe.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Vibe.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CadastroPage : ContentPage
    {
        private CadastroViewModel cadastroViewModel;
		public CadastroPage ()
		{
			InitializeComponent ();
            BindingContext = this.cadastroViewModel = new CadastroViewModel();
        }
	}
}