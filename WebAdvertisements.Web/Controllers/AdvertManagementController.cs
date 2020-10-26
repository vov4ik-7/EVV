using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdvertisements.Web.DTO;
using WebAdvertisements.Web.ServiceClients;
using WebAdvertisements.Web.Services;
using WebAdvertisements.Web.ViewModels.Account;

namespace WebAdvertisements.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IMapper _mapper;
        private readonly IAdvertApiClient _advertApiClient;

        public AdvertManagementController(IFileUploader fileUploader,
            IMapper mapper,
            IAdvertApiClient advertApiClient)
        {
            _fileUploader = fileUploader;
            _mapper = mapper;
            _advertApiClient = advertApiClient;
        }

        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            return View(new CreateAdvertisementViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertisementViewModel model, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                var createAdvertModel = _mapper.Map<CreateAdvertisement>(model);
                createAdvertModel.UserName = User.Identity.Name;

                var apiCallResponse = await _advertApiClient.CreateAsync(createAdvertModel);
                var id = apiCallResponse.Id;

                bool isOkToConfirmAd = true;
                string filePath = string.Empty;
                if (imageFile != null)
                {
                    var fileName = !string.IsNullOrEmpty(imageFile.FileName) ? Path.GetFileName(imageFile.FileName) : id;
                    filePath = $"{id}/{fileName}";

                    try
                    {
                        using var readStream = imageFile.OpenReadStream();
                        var result = await _fileUploader.UploadFileAsync(filePath, readStream);
                        if (!result)
                            throw new Exception(
                                "Could not upload the image to file repository. Please see the logs for details.");
                    }
                    catch (Exception)
                    {
                        isOkToConfirmAd = false;
                        var confirmModel = new ConfirmAdvertRequest()
                        {
                            Id = id,
                            FilePath = filePath,
                            Status = AdvertisementStatus.Pending
                        };
                        await _advertApiClient.ConfirmAsync(confirmModel);
                    }
                }

                if (isOkToConfirmAd)
                {
                    var confirmModel = new ConfirmAdvertRequest()
                    {
                        Id = id,
                        FilePath = filePath,
                        Status = AdvertisementStatus.Active
                    };
                    await _advertApiClient.ConfirmAsync(confirmModel);
                }

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }
    }
}
