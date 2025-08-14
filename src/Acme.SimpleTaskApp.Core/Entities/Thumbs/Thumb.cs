using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acme.SimpleTaskApp.Entities.Thumbs
{
	[Table("AppThumbs")]
	public class Thumb : FullAuditedEntity<int>
	{
		
		public int ProductId { get; set; }
		public string Image { get; set; }

		public Thumb(string image)
		{
			Image = image;
		}
	}
}
