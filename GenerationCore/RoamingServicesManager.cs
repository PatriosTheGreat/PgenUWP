﻿using System;
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

        public async void AddServiceAsync(ServiceInformation service)
        {
            Contract.Assert(service != null);

            var services = (await LoadServicesAsync()).Concat(new[] { service });
            SaveServicesAsync(services);
        }

        public async void RemoveServiceAsync(string serviceToken)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceToken));
            
            var services = (await LoadServicesAsync()).Where(service => service.UniqueToken != serviceToken);
            SaveServicesAsync(services);
        }

        private async void SaveServicesAsync(IEnumerable<ServiceInformation> services)
        {
            var serviceFile = await RoamingFolder.CreateFileAsync(
                ServicesFileName,
                CreationCollisionOption.ReplaceExisting);

            using (var servicesStream = await serviceFile.OpenStreamForWriteAsync())
            {
                _serializer.WriteObject(servicesStream, services.ToList());
            }

            ServicesUpdated();
        }

        public async Task<IEnumerable<ServiceInformation>> LoadServicesAsync()
        {
            var serviceFile = await RoamingFolder.CreateFileAsync(
                ServicesFileName,
                CreationCollisionOption.OpenIfExists);
            
            using (var servicesStream = await serviceFile.OpenStreamForReadAsync())
            {
                if (servicesStream.Length == 0)
                {
                    return Enumerable.Empty<ServiceInformation>();
                }

                return _serializer.ReadObject(servicesStream) as List<ServiceInformation>;
            }
        }
        
        private static StorageFolder RoamingFolder => ApplicationData.Current.RoamingFolder;

        private readonly DataContractSerializer _serializer;
        private const string ServicesFileName = "services.config";
    }
}
