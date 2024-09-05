using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MorgageHouse_Link_Shortner.Server.Models
{
	[Table("url_link", Schema = "dbo")]
	public class UrlLink
	{
		/// <summary>
		/// Will be more shorter if this is integer
		/// However using guid to have litle bit more obfuscated effect
		/// </summary>
		[Key()]
        [Column("link_id")]
		public Guid Id { get; set; }

		[Column("full_url")]
		[Required(AllowEmptyStrings = false)]
		[MaxLength(2048)]
		public string FullUrl { get; set; }
	}

}
