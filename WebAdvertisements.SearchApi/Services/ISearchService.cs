using System.Collections.Generic;
using System.Threading.Tasks;
using WebAdvertisements.SearchApi.Models;

namespace WebAdvertisements.SearchApi.Services
{
    public interface ISearchService
    {
        Task<List<AdvertType>> Search(string keyword);
    }
}
