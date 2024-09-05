using MorgageHouse_Link_Shortner.Server.Interfaces;
using MorgageHouse_Link_Shortner.Server.Models;
using MorgageHouse_Link_Shortner.Server.Repository;
using System.Collections.Generic;
using System.Diagnostics;

namespace MorgageHouse_Link_Shortner.Server.Services
{
	public class UrlLinkShortnerService : IUrlLinkShortnerService
	{
		private readonly IUrlLinkRepository _repository;
		private readonly ILogger _logger;
		private readonly IHttpContextAccessor _httpCxtAccessor;

        private readonly IObfuscator<Guid> _obfuscator;

        public UrlLinkShortnerService(ILoggerFactory logger, IHttpContextAccessor httpCxtAccessor, IUrlLinkRepository repository)
		{
			this._repository = repository;
			this._logger = logger.CreateLogger("UrlLinkShortnerService");
			this._httpCxtAccessor = httpCxtAccessor;
            this._obfuscator = new GuidObfuscator();
        }

		private string GetFullPath(string shortUrl)
		{
			var host = this._httpCxtAccessor.HttpContext?.Request.Host;

			// The below is temporary fix for debugging. Without this, angular will try to resolve route itself still (like inside localhost:4100 ...)
            if (Debugger.IsAttached && host.ToString() == "localhost:4200")
			{
                return $"http://localhost:5100/{shortUrl}";
            }
			return $"{this._httpCxtAccessor.HttpContext?.Request.Scheme}://{host}/{shortUrl}";
		}

        public async Task<List<UrlShortLink>> GetAllUrlLinks()
		{
			List<UrlShortLink> rtnList = new List<UrlShortLink>();

			List<UrlLink> links = await this._repository.GetAllUrlLinks();
			foreach(UrlLink itm in links)
			{
				var idObfuscated = this._obfuscator.Encode(itm.Id);

				UrlShortLink converted = new UrlShortLink()
				{
					Id = itm.Id,
					FullUrl = itm.FullUrl,
					ShortLink = this.GetFullPath(idObfuscated)
                };
				rtnList.Add(converted);
			}

			return rtnList;
		}

		public async Task<UrlShortLink?> GetUrlLink(Guid id)
		{
            

            var link = await this._repository.GetUrlLink(id);
			if(link == null)
			{
				return null;
			}

            var idObfuscated = this._obfuscator.Encode(link.Id);
            UrlShortLink rtn = new UrlShortLink()
			{
				Id = link.Id,
				FullUrl = link.FullUrl,
				ShortLink = this.GetFullPath(idObfuscated)
            };

			return rtn;
		}

		public async Task<UrlShortLink?> GetUrlLink(string shortUrl)
		{
			try
			{
				Guid guid;
				var isOK = this._obfuscator.TryDecode(shortUrl, out guid);
				if (!isOK)
				{
                    this._logger.LogWarning($"GetUrlLink failed, shortUrl, ${shortUrl}, is invalid");
                    return null;
                }

                UrlLink? link = await this._repository.GetUrlLink(guid);
				if (link == null)
				{
                    this._logger.LogWarning($"GetUrlLink failed, shortUrl, ${shortUrl}, is invalid format but does not exist");
                    return null;
                }

                UrlShortLink rtn = new UrlShortLink()
				{
					Id = link.Id,
					FullUrl = link.FullUrl,
					ShortLink = this.GetFullPath(shortUrl)
                };

				return rtn;
			}
            catch (Exception err)
			{
				this._logger.LogError($"GetUrlLink failed, {err}");
				return null;
			}
		}
	}
}
