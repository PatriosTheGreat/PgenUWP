using System.ComponentModel;
using Windows.UI.Xaml;
using PgenUWP.ViewModels;

namespace PgenUWP.Views
{
    public sealed partial class ServicesPage : INotifyPropertyChanged
    {
        public ServicesPage()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public ServicesPageViewModel ConcreteDataContext => DataContext as ServicesPageViewModel;

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
