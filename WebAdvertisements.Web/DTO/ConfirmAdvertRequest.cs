using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvertisements.Web.DTO
{
    public class ConfirmAdvertRequest
    {
        public string Id { get; set; }
        public string FilePath { get; set; }
        public AdvertisementStatus Status { get; set; }
    }
}
