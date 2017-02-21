using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameGroupManager.Models
{
	public class Preferences
	{
		//composite key
		public int GgmUserId { get; set; }
		public string Key { get; set; }

		public string Value { get; set; }
	}
}