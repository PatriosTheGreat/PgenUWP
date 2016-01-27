using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Windows.Storage;

namespace GenerationCore
{
    public sealed class RoamingServicesManager : IServicesManager
    {
        public RoamingServicesManager()
        {
            _serializer = new DataContractSerializer(typeof(List<ServiceInformation>));
            ApplicationData.Current.DataChanged += (sender, args) => ServicesUpdated();
        }

        public event Action ServicesUpdated = Actions.DoNothing;

        public async void SaveServiceAsync(ServiceInformation service)
        {
            Contract.Assert(service != null);

            var services = (await LoadServicesAsync()).Concat(new[] { service });
            SaveServicesAsync(services);
        }

        public async void DeleteServiceAsync(string serviceToken)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceToken));
            
            var services = (await LoadServicesAsync()).Where(service => service.UniqueToken != serviceToken);
            SaveServicesAsync(services);
        }

        private async void SaveServicesAsync(IEnumerable<ServiceInformation> services)
        {
            var servicesFolder = await OpenOrCreateSettingsFolder();
            using (var servicesStream = await servicesFolder.OpenStreamForWriteAsync())
            {
                _serializer.WriteObject(servicesStream, services.ToArray());
            }

            ServicesUpdated();
        }

        public async Task<IEnumerable<ServiceInformation>> LoadServicesAsync()
        {
            var servicesFolder = await OpenOrCreateSettingsFolder();
            using (var servicesStream = await servicesFolder.OpenStreamForReadAsync())
            {
                if (servicesStream.Length == 0)
                {
                    return Enumerable.Empty<ServiceInformation>();
                }

                return _serializer.ReadObject(servicesStream) as List<ServiceInformation>;
            }
        }

        private static async Task<StorageFile> OpenOrCreateSettingsFolder()
        {
            return await RoamingFolder.CreateFileAsync(
                ServicesFileName,
                CreationCollisionOption.OpenIfExists);
        }
        
        private static StorageFolder RoamingFolder => ApplicationData.Current.RoamingFolder;

        private readonly DataContractSerializer _serializer;
        private const string ServicesFileName = "services.config";
    }
}
