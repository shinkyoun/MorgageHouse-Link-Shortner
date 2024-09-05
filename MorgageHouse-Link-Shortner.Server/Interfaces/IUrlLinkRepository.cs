using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Interfaces
{
    public interface IUrlLinkRepository : IUrlLinkReader<UrlLink>
	{
		Task<UrlLink?> AddUrlLink(string fullUrl);
        Task<bool> DeleteLink(Guid id);
	}
}
