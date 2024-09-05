using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Interfaces
{
    public interface IUrlLinkShortnerService : IUrlLinkReader<UrlShortLink>
	{

		Task<UrlShortLink?> GetUrlLink(string shortUrl);
	}
}
