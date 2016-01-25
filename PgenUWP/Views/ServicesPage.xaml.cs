using System;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GenerationCore;
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

        private void FoundServiceSubmitted(
            AutoSuggestBox sender,
            AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var choosenService = (args.ChosenSuggestion as ServiceInformation);
            if (choosenService != null)
            {
                ConcreteDataContext.NavigateToService.Execute(choosenService);
            }
        }

        private void ServiceSearchQUeryChanged(
            AutoSuggestBox sender,
            AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var searchQuery = sender.Text;
                if (string.IsNullOrEmpty(searchQuery))
                {
                    ServicesSearchBox.ItemsSource = null;
                    return;
                }

                var currentServices = ConcreteDataContext.Services;
                var suggestions = currentServices.Where(
                    service =>
                        service.ServiceName.IndexOf(
                            searchQuery,
                            StringComparison.CurrentCultureIgnoreCase) >= 0).ToList();
                
                if (suggestions.Count > 0)
                {
                    SetSearchBoxMemberPath(nameof(ServiceInformation.ServiceName));
                    ServicesSearchBox.ItemsSource =
                        suggestions.OrderByDescending(
                            serviceInformation =>
                                serviceInformation.ServiceName.StartsWith(
                                    searchQuery,
                                    StringComparison.CurrentCultureIgnoreCase))
                            .ThenBy(serviceInformation => serviceInformation.ServiceName)
                            .ToArray();
                }
                else
                {
                    SetSearchBoxMemberPath(string.Empty);
                    ServicesSearchBox.ItemsSource = new[] { "No results found" };
                }
            }
        }
        
        private void SetSearchBoxMemberPath(string path)
        {
            ServicesSearchBox.DisplayMemberPath = path;
            ServicesSearchBox.TextMemberPath = path;
        }
    }
}
