using Microsoft.Practices.ServiceLocation;

namespace Vibe.ViewModels
{
    public class ViewModelLocator
    {
        public ItemsViewModel ItemsViewModel
        {
            get => ServiceLocator.Current.GetInstance<ItemsViewModel>();
        }

        public AboutViewModel AboutViewModel
        {
            get => ServiceLocator.Current.GetInstance<AboutViewModel>();
        }
    }
}