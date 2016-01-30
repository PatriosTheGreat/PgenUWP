using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Windows.ApplicationModel.DataTransfer;
using GenerationCore;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace PgenUWP.ViewModels
{
    public sealed class GeneratePasswordPageViewModel : ViewModelBase
    {
        public GeneratePasswordPageViewModel(
            IServicesManager servicesManager,
            ISessionStateService sessionStateService,
            INavigationService navigationService)
        {
            Contract.Assert(sessionStateService != null);

            _sessionStateService = sessionStateService;
            _service = null;
            _servicePassword = null;

            GenerateServicePassword = new LambdaCommand(
                parameter =>
                {
                    var password = parameter as string;
                    if (password?.Length > 0)
                    {
                        ServicePassword = ServicePasswordGenerator.GeneratePassword(
                            Service,
                            password);
                    }
                },
                _ => Service != null);

            CopyServicePassword = new LambdaCommand(
                _ =>
                {
                    _dataPackage.SetText(ServicePassword);
                    Clipboard.SetContent(_dataPackage);
                },
                _ => ServicePassword?.Length > 0);

            RemoveService = new LambdaCommand(
                _ =>
                {
                    servicesManager.RemoveServiceAsync(Service.UniqueToken);
                    navigationService.GoBack();
                });
        }
        
        public override void OnNavigatedTo(
            NavigatedToEventArgs args, 
            Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(args, viewModelState);

            if (_sessionStateService.SessionState.ContainsKey(nameof(ServiceInformation)))
            {
                Service = _sessionStateService.SessionState[nameof(ServiceInformation)] as ServiceInformation;
            }
        }

        public ServiceInformation Service
        {
            get { return _service; }

            set
            {
                _service = value;
                GenerateServicePassword.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }

        public LambdaCommand CopyServicePassword
        {
            get { return _copyServicePassword; }

            set
            {
                _copyServicePassword = value;
                OnPropertyChanged();
            }
        }

        public LambdaCommand GenerateServicePassword
        {
            get { return _generateServicePassword; }

            set
            {
                _generateServicePassword = value;
                OnPropertyChanged();
            }
        }

        public LambdaCommand RemoveService
        {
            get { return _removeService; }

            set
            {
                _removeService = value;
                OnPropertyChanged();
            }
        }

        public string ServicePassword
        {
            get { return _servicePassword; }

            set
            {
                _servicePassword = value;
                CopyServicePassword.RaiseCanExecuteChanged();
                OnPropertyChanged();
            }
        }
        
        private LambdaCommand _copyServicePassword;
        private string _servicePassword;
        private LambdaCommand _generateServicePassword;
        private LambdaCommand _removeService;
        private ServiceInformation _service;
        private readonly DataPackage _dataPackage = new DataPackage();
        private readonly ISessionStateService _sessionStateService;
    }
}
