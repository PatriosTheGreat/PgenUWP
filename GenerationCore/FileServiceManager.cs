using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GenerationCore
{
    public sealed class FileServiceManager : IServicesManager
    {
        public event Action ServicesUpdated = Actions.DoNothing;

        public FileServiceManager()
        {
            _serializer = new DataContractSerializer(typeof(List<ServiceInformation>));
            SetFile("services.config");
        }

        public void SetFile(string filePath)
        {
            _servicesFilePath = filePath;
            ServicesUpdated();
        }

        public async void SaveServiceAsync(ServiceInformation service)
        {
            Contract.Assert(service != null);

            var services = (await LoadServicesAsync()).Concat(new[] { service });
            SaveServicesToFile(services);
        }

        public async void DeleteServiceAsync(string serviceToken)
        {
            Contract.Assert(!string.IsNullOrWhiteSpace(serviceToken));

            var services = (await LoadServicesAsync()).Where(service => service.UniqueToken != serviceToken);
            SaveServicesToFile(services);
        }

        public Task<IEnumerable<ServiceInformation>> LoadServicesAsync()
        {
            return Task.FromResult<IEnumerable<ServiceInformation>>(null);

            /*
            return DoActionWithServicesFile(() =>
            {
                if (!File.Exists(_servicesFilePath))
                {
                    return Enumerable.Empty<ServiceInformation>();
                }

                using (var fileSteam = new FileStream(_servicesFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    return _serializer.ReadObject(fileSteam) as List<ServiceInformation>;
                }
            });*/
        }

        private void SaveServicesToFile(IEnumerable<ServiceInformation> services)
        {
            DoActionWithServicesFile(() =>
            {
                File.WriteAllText(_servicesFilePath, string.Empty);
                using (var fileStream = new FileStream(_servicesFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    _serializer.WriteObject(fileStream, services.ToArray());
                }

                return 0;
            });
        }

        private T DoActionWithServicesFile<T>(Func<T> action)
        {
            var result = default(T);
            lock (_syncRoot)
            {
                bool actionFailed;

                var triesCount = 0;
                do
                {
                    try
                    {
                        result = action();
                        actionFailed = false;
                    }
                    catch (IOException)
                    {
                        actionFailed = true;

                        if (triesCount > 30)
                        {
                            // ToDo: Выкидывать собственное исключение. 
                            // ToDo: Добавить перехват и вывод ошибки с возможностью повторить действие.
                            throw;
                        }

                        Task.Delay(TimeSpan.FromSeconds(1)).GetAwaiter().GetResult();
                    }

                    triesCount++;
                } while (actionFailed);
            }

            return result;
        }

        private readonly DataContractSerializer _serializer;
        private string _servicesFilePath;

        private readonly object _syncRoot = new object();
    }
}
