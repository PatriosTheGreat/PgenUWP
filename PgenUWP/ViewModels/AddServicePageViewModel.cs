using System.Windows.Input;
using GenerationCore;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace PgenUWP.ViewModels
{
    public sealed class AddServicePageViewModel : ViewModelBase
    {
        public AddServicePageViewModel(
            IServicesManager servicesManager,
            INavigationService navigator)
        {
            _servicesManager = servicesManager;
            _navigator = navigator;
            
            Submit = new LambdaCommand(
                _ =>
                {
                    _servicesManager.AddServiceAsync(
                        new ServiceInformation(
                            string.IsNullOrEmpty(ServiceName) ? DefaultServiceName : ServiceName,
                            new PasswordRestriction(
                                CollectSymbolTypes(),
                                PasswordMinLength,
                                PasswordMaxLength)));

                    _navigator.GoBack();
                });
        
            ServiceName = DefaultServiceName;
            PasswordMinLength = PasswordRestriction.DefaultMinLength;
            PasswordMaxLength = PasswordRestriction.DefaultMaxLength;
            AllowLowLatin = true;
            AllowUpperLatin = true;
        }

        public bool AllowLowLatin
        {
            get { return _allowLowLatin; }

            set
            {
                _allowLowLatin = value;
                OnPropertyChanged();
            }
        }

        public bool AllowUpperLatin
        {
            get { return _allowUpperLatin; }

            set
            {
                _allowUpperLatin = value;
                OnPropertyChanged();
            }
        }

        public string ServiceName
        {
            get { return _serviceName; }

            set
            {
                _serviceName = value;
                OnPropertyChanged();
            }
        }

        public int PasswordMinLength
        {
            get { return _passwordMinLength; }

            set
            {
                _passwordMinLength = value;
                OnPropertyChanged();
            }
        }

        public int PasswordMaxLength
        {
            get { return _passwordMaxLength; }

            set
            {
                _passwordMaxLength = value;
                OnPropertyChanged();
            }
        }

        public ICommand Submit
        {
            get { return _submit; }

            set
            {
                _submit = value;
                OnPropertyChanged();
            }
        }

        public int SymbolTypesCount => (int)CollectSymbolTypes().Count();

        private SymbolsType CollectSymbolTypes()
        {
            var allawedSymbols = SymbolsType.Digital;

            if (AllowLowLatin)
            {
                allawedSymbols |= SymbolsType.LowcaseLatin;
            }

            if (AllowUpperLatin)
            {
                allawedSymbols |= SymbolsType.UpcaseLatin;
            }

            return allawedSymbols;
        }

        private ICommand _submit;
        private readonly IServicesManager _servicesManager;
        private readonly INavigationService _navigator;
        private string _serviceName;
        private int _passwordMinLength;
        private int _passwordMaxLength;
        private bool _allowLowLatin;
        private bool _allowUpperLatin;
        private const string DefaultServiceName = "Default name";
    }
}
