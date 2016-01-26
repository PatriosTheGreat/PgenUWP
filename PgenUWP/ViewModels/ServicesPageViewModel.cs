using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.Contracts;
using System.Windows.Input;
using GenerationCore;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace PgenUWP.ViewModels
{
    public sealed class ServicesPageViewModel : ViewModelBase
    {
        public ServicesPageViewModel(
            IServicesManager servicesManager, 
            INavigationService navigationService,
            ISessionStateService sessionStateService)
        {
            Contract.Assert(servicesManager != null);
            Contract.Assert(navigationService != null);
            Contract.Assert(sessionStateService != null);

            _servicesManager = servicesManager;
            _sessionStateService = sessionStateService;
            Services = new ObservableCollection<ServiceInformation>();
            NavigateToService = new LambdaCommand(
                parameter =>
                {
                    Contract.Assert(parameter is ServiceInformation);

                    if (_sessionStateService.SessionState.ContainsKey(nameof(ServiceInformation)))
                    {
                        _sessionStateService.SessionState.Remove(nameof(ServiceInformation));
                    }

                    _sessionStateService.SessionState.Add(nameof(ServiceInformation), parameter);
                    navigationService.Navigate(PageTokens.GeneratePassword, null);
                });

            NavigateToAddService = new LambdaCommand(_ => navigationService.Navigate(PageTokens.AddService, null));

            _servicesManager.ServicesUpdated += ResetServices;

            ResetServices();
        }
        
        public ICommand NavigateToAddService
        {
            get { return _navigateToAddService; }

            set
            {
                _navigateToAddService = value;
                OnPropertyChanged();
            }
        }
        
        public ICommand NavigateToService
        {
            get { return _navigateToService; }

            set
            {
                _navigateToService = value;
                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<ServiceInformation> Services
        {
            get
            {
                return _services;
            }

            set
            {
                _services = value;
                OnPropertyChanged();
            }
        }

        private async void ResetServices()
        {
            Contract.Assert(Services != null);
            Services.CollectionChanged -= OnServicesChanged;
        
            var services = await _servicesManager.LoadServicesAsync();

            Services = new ObservableCollection<ServiceInformation>(services);
            Services.CollectionChanged += OnServicesChanged;
            OnPropertyChanged();
        }

        private void OnServicesChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            OnPropertyChanged();
        }

        private ObservableCollection<ServiceInformation> _services;
        private ICommand _navigateToAddService;
        private ICommand _navigateToService;
        private readonly IServicesManager _servicesManager;
        private readonly ISessionStateService _sessionStateService;
    }
}
