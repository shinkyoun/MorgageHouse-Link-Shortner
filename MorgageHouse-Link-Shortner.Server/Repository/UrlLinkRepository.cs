using MorgageHouse_Link_Shortner.Server.Interfaces;
using MorgageHouse_Link_Shortner.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace MorgageHouse_Link_Shortner.Server.Repository
{
    public class UrlLinkRepository : IUrlLinkRepository
	{
		private readonly UrlLinkDbContext _dbCxt;
		public UrlLinkRepository(UrlLinkDbContext dbContext) {
			this._dbCxt = dbContext;
		}

		public async Task<List<UrlLink>> GetAllUrlLinks()
		{
			return await this._dbCxt.UrlLinks.ToListAsync();
		}

		public async Task<UrlLink?> GetUrlLink(Guid id)
		{
			return await this._dbCxt.UrlLinks.FirstOrDefaultAsync(link => link.Id == id);
		}

		public async Task<UrlLink?> AddUrlLink(string fullUrl)
		{
			var urlLinkToAdd = new UrlLink()
			{
				Id = Guid.NewGuid(),
				FullUrl = fullUrl
			};

			this._dbCxt.Add(urlLinkToAdd);
			var result = await _dbCxt.SaveChangesAsync();
			return result >= 0 ? urlLinkToAdd : null;
		}

		public async Task<bool> DeleteLink(Guid id)
		{
            var found = await this._dbCxt.UrlLinks.FirstOrDefaultAsync(link => link.Id == id);
			if(found != null) 
			{
				this._dbCxt.Remove(found);
                var result = await _dbCxt.SaveChangesAsync();
				return true;
            }
			else
			{
				return false;
			}
        }
    }
}
