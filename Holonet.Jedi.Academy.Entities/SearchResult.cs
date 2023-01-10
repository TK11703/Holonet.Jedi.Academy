using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Holonet.Jedi.Academy.Entities
{
	public class SearchResult
	{
		public int Id { get; set; } = 0;
		public string Name { get; set; } = string.Empty;
		public string Type { get; set; } = string.Empty;

		public string URL
		{
			get
			{
				return string.Format("/{0}/Details?id={1}", Type, Id);
			}
		}
	}
}
