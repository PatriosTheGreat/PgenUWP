using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Microsoft.Practices.Unity;
using GenerationCore;
using PgenUWP.ViewModels;
using Prism.Mvvm;

namespace PgenUWP
{
    public sealed partial class App
    {
        public App()
        {
            InitializeComponent();
        }
        
        protected override void OnRegisterKnownTypesForSerialization()
        {
            base.OnRegisterKnownTypesForSerialization();
            SessionStateService.RegisterKnownType(typeof(ServiceInformation));
        }

        protected override Task OnLaunchApplicationAsync(LaunchActivatedEventArgs args)
        {
            if (args.PreviousExecutionState != ApplicationExecutionState.Running)
            {
                InitializeContainer();
            }

            NavigationService.Navigate(PageTokens.Services, null);
            return Task.FromResult(true);
        }

        private void InitializeContainer()
        {
            Container.RegisterInstance(NavigationService);
            Container.RegisterInstance(SessionStateService);
            Container.RegisterInstance<IServicesManager>(new RoamingServicesManager());

            var servicesPageViewModel = Container.Resolve(typeof(ServicesPageViewModel));

            ViewModelLocationProvider.SetDefaultViewModelFactory(
                viewModelType =>
                {
                    if (viewModelType == typeof(ServicesPageViewModel))
                    {
                        return servicesPageViewModel;
                    }

                    return Container.Resolve(viewModelType);
                });
        }
    }
}
