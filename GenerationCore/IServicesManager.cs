using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GenerationCore
{
    public interface IServicesManager
    {
        void SaveServiceAsync(ServiceInformation service);

        void DeleteServiceAsync(string serviceToken);

        Task<IEnumerable<ServiceInformation>> LoadServicesAsync();

        event Action ServicesUpdated;
    }
}
