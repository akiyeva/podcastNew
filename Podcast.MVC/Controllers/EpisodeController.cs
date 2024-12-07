using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Podcast.BLL.Services.Contracts;
using Podcast.BLL.AutoMapper;
using AutoMapper;
using Podcast.DAL.DataContext.Entities;
using Podcast.BLL.ViewModels.EpisodeViewModels;

namespace Podcast.MVC.Controllers
{
    public class EpisodeController : Controller
    {
        private readonly IEpisodeService _episodeService;
        private readonly IMapper _mapper;
        public EpisodeController(IEpisodeService episodeService, IMapper mapper)
        {
            _episodeService = episodeService;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("episode/{id}/{title}")]
        public async Task<IActionResult> Details(int id, string title)
        {
            var episode = await _episodeService.GetAsync(id);

            if (episode == null || !episode.Title.Equals(title))
            {
                return NotFound();
            }

            return View(episode);
        }

        [HttpPost]
        [Route("episode/download/{id}")]
        public async Task<IActionResult> Download(int id)
        {
            var existEpisode = await _episodeService.GetAsync(id);

            if (existEpisode == null)
            {
                return NotFound();
            }

            existEpisode.DownloadCount++;
            var episode = _mapper.Map<EpisodeUpdateViewModel>(existEpisode);
            await _episodeService.UpdateAsync(episode);

            var musicFilePath = episode.MusicUrl; 
            if (string.IsNullOrEmpty(musicFilePath) || !System.IO.File.Exists(musicFilePath))
            {
                return NotFound("Music file not found.");
            }

            var musicFileBytes = await System.IO.File.ReadAllBytesAsync(musicFilePath);
            return File(musicFileBytes, "audio", $"{episode.Title}.mp3");
        }


    }
}
