using System.ComponentModel;
using Windows.UI.Xaml;
using PgenUWP.ViewModels;

namespace PgenUWP.Views
{
    public sealed partial class GeneratePasswordPage : INotifyPropertyChanged
    {
        public GeneratePasswordPage()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public GeneratePasswordPageViewModel ConcreteDataContext => DataContext as GeneratePasswordPageViewModel;

        private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            var propertyChanged = PropertyChanged;
            propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ConcreteDataContext)));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
