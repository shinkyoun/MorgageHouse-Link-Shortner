using Microsoft.AspNetCore.Mvc;
using MorgageHouse_Link_Shortner.Server.Interfaces;
using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LinkShortnerController : ControllerBase
	{
		private readonly IUrlLinkRepository _linkRepository;
		private readonly IUrlLinkShortnerService _linkShortnerSvc;

		public LinkShortnerController(IUrlLinkRepository linkRepository, IUrlLinkShortnerService linkShortnerSvc)
		{
			this._linkRepository = linkRepository;
			this._linkShortnerSvc = linkShortnerSvc;
		}

		// GET: api/<LinkShortnerController>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			var allList = await this._linkShortnerSvc.GetAllUrlLinks();
			return Ok(allList);
		}

		// GET api/<LinkShortnerController>/5
		[HttpGet("{id}")]
		public async Task<IActionResult> Get(Guid id)
		{
			var foundLink = await this._linkShortnerSvc.GetUrlLink(id);
			if(foundLink != null)
			{
				return Ok(foundLink);
			}
			else
			{
				return NotFound();
			}
		}

		// POST api/<LinkShortnerController>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] UrlShortRegistration request)
		{
			var result = await this._linkRepository.AddUrlLink(request.FullUrl);
			if(result != null)
			{
				return Ok();
			}
			else
			{
				return this.Problem("Failed to create url link");
			}
		}

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await this._linkRepository.DeleteLink(id);
            return Ok(deleted);
        }
    }
}
