using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvertisements.Web.DTO;

namespace WebAdvertisements.Web.ServiceClients
{
    public interface IAdvertApiClient
    {
        Task<CreateAdvertResponse> CreateAsync(CreateAdvertisement model);
        Task<bool> ConfirmAsync(ConfirmAdvertRequest model);
        Task<List<Advertisement>> GetAllAsync();
        Task<Advertisement> GetAsync(string advertId);
    }
}
