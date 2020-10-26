using System;
using System.Collections.Generic;
using System.Text;

namespace WebAdvertisements.AdvertApi.Models
{
    public class ConfirmAdvertisement
    {
        public string Id { get; set; }

        public string FilePath { get; set; }

        public AdvertisementStatus Status { get; set; }
    }
}
