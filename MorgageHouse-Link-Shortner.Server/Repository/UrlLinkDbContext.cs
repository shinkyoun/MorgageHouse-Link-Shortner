using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.EntityFrameworkCore;

using MorgageHouse_Link_Shortner.Server.Models;

namespace MorgageHouse_Link_Shortner.Server.Repository
{
	public class UrlLinkDbContext : DbContext
	{
		public const string DB_CONN = "UrlLinkDbConnection";

		public DbSet<UrlLink> UrlLinks { get; set; }

		public UrlLinkDbContext(DbContextOptions<UrlLinkDbContext> cxtOption) : base(cxtOption)
		{
		}


		/*
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			IConfigurationRoot configuration = new ConfigurationBuilder()
				.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
				.AddJsonFile("appsettings.json")
				.Build();
			string? dbFromEnv = Environment.GetEnvironmentVariable(UrlLinkDbContext.DB_CONN);
			if (String.IsNullOrEmpty(dbFromEnv))
			{
				optionsBuilder.UseSqlServer(configuration.GetConnectionString(UrlLinkDbContext.DB_CONN));
			}
			else
			{
				optionsBuilder.UseSqlServer(dbFromEnv);
			}
		}
		*/
	}
}
