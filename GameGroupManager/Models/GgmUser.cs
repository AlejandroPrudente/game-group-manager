using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GameGroupManager.Models
{
	public class GgmUser
	{
		public int Id { get; set; }

		//use FluentValidation for everything? http://stackoverflow.com/questions/16678625/asp-net-mvc-4-ef5-unique-property-in-model-best-practice
		[Required]
		[StringLength(256)]
		[Display(Name = "Email address")]
		public string Email { get; set; }

		[Required(AllowEmptyStrings = true)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		[Display(Name = "First name")]
		public string FirstName { get; set; }

		[Required(AllowEmptyStrings = true)]
		[DisplayFormat(ConvertEmptyStringToNull = false)]
		[Display(Name = "Last name")]
		public string LastName { get; set; }

		public string Name => $"{FirstName} {LastName}";

		public virtual ApplicationUser User { get; set; }

		[Required]
		public string ApplicationUserId { get; set; }
	}
}