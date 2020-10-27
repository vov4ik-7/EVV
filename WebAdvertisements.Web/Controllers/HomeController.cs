﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebAdvertisement.Web.ServiceClients;
using WebAdvertisement.Web.ViewModels.Home;
using WebAdvertisements.Web.ServiceClients;
using WebAdvertisements.Web.ViewModels;
using WebAdvertisements.Web.ViewModels.Home;

namespace WebAdvertisements.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ISearchApiClient _searchApiClient;
        private readonly IMapper _mapper;
        private readonly IAdvertApiClient _advertApiClient;

        public HomeController(ISearchApiClient searchApiClient,
            IMapper mapper,
            IAdvertApiClient advertApiClient)
        {
            _searchApiClient = searchApiClient;
            _mapper = mapper;
            _advertApiClient = advertApiClient;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allAds = await _advertApiClient.GetAllAsync();
            var allViewModels = allAds.Select(x => _mapper.Map<IndexViewModel>(x));

            return View(allViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var viewModel = new List<SearchViewModel>();

            var searchResult = await _searchApiClient.Search(keyword);
            searchResult.ForEach(advertDoc =>
            {
                var viewModelItem = _mapper.Map<SearchViewModel>(advertDoc);
                viewModel.Add(viewModelItem);
            });

            return View("Search", viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
