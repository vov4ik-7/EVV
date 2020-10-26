using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using WebAdvertisements.AdvertApi.DTO;
using WebAdvertisements.AdvertApi.Models;

namespace WebAdvertisements.AdvertApi.Profiles
{
    public class AdvertProfile : Profile
    {
        public AdvertProfile()
        {
            CreateMap<Advertisement, AdvertisementDb>()
                .ReverseMap();
        }
    }
}
