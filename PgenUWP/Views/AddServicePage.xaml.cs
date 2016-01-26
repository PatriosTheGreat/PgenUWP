using System.ComponentModel;
using Windows.UI.Xaml;
using PgenUWP.ViewModels;

namespace PgenUWP.Views
{
    public sealed partial class AddServicePage : INotifyPropertyChanged
    {
        public AddServicePage()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public AddServicePageViewModel ConcreteDataContext => DataContext as AddServicePageViewModel;

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
