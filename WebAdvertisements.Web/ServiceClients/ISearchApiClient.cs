using System.Collections.Generic;
using System.Threading.Tasks;
using WebAdvertisements.Web.DTO;

namespace WebAdvertisement.Web.ServiceClients
{
    public interface ISearchApiClient
    {
        Task<List<AdvertType>> Search(string keyword);
    }
}
