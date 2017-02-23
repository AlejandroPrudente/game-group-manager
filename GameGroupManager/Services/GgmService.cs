using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GameGroupManager.Models;

namespace GameGroupManager.Services
{
	public class GgmService
	{
		private ApplicationDbContext db;

		public GgmService(ApplicationDbContext dbContext)
		{
			db = dbContext;
		}

		public void CreateGgmUser(string email, string firstName, string lastName, string id)
		{
			var ggmUser = new GgmUser
			{
				Email = email,
				FirstName = firstName,
				LastName = lastName,
				ApplicationUserId = id
			};
			db.GgmUsers.Add(ggmUser);
			db.SaveChanges();
		}

		public string GetGgmUserName(string email)
		{
			var user = db.GgmUsers.FirstOrDefault(u => u.Email == email);
			return user?.Name ?? email;
		}
	}
}