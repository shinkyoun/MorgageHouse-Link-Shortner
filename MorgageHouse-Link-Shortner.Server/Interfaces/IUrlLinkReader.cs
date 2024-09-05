using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Interfaces
{
	public interface IUrlLinkReader<T> where T : UrlLink
	{
		Task<List<T>> GetAllUrlLinks();
		Task<T?> GetUrlLink(Guid id);
	}
}
