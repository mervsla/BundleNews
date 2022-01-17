using BundleNews.Core.Services.RSSService.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BundleNews.UI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RssController : Controller
    {
        IRssService _rssGeneratorService;
        IHostEnvironment _environment;
        public RssController(IRssService service, IHostEnvironment environment)
        {
            _rssGeneratorService = service;
            _environment = environment;
        }
        public IActionResult AddRss()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult RunRssGenerator(string url, string name)
        {
            bool result = _rssGeneratorService.RunRssGenerator(url, name, _environment);
            if (result)
                return Ok(Json("Ekleme başarılı"));
            else
                return Ok(Json("İçerikler kaydedilemedi"));
        }
    }
}
