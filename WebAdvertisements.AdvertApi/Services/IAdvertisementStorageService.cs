using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvertisements.AdvertApi.Models;

namespace WebAdvertisements.AdvertApi.Services
{
    public interface IAdvertisementStorageService
    {
        Task<string> Add(Advertisement advertisement);

        Task Confirm(ConfirmAdvertisement confirm);

        Task<Advertisement> GetByIdAsync(string id);

        Task<List<Advertisement>> GetAllAsync();
    }
}
