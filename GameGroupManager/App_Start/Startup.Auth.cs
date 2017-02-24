using System;
using System.Configuration;
using System.Security.Claims;
using System.Threading.Tasks;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using GameGroupManager.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Facebook;

namespace GameGroupManager
{
	public partial class Startup
	{
		// Pour plus d’informations sur la configuration de l’authentification, rendez-vous sur http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{
			// Configurer le contexte de base de données, le gestionnaire des utilisateurs et le gestionnaire des connexions pour utiliser une instance unique par demande
			app.CreatePerOwinContext(ApplicationDbContext.Create);
			app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

			// Autoriser l’application à utiliser un cookie pour stocker des informations pour l’utilisateur connecté
			// et pour utiliser un cookie à des fins de stockage temporaire des informations sur la connexion utilisateur avec un fournisseur de connexion tiers
			// Configurer le cookie de connexion
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				//todo: allow the option to use two-factor authentication with .TwoFactorRememberBrowserCookie
				LoginPath = new PathString("/Account/Login"),
				Provider = new CookieAuthenticationProvider
				{
					// Permet à l'application de valider le timbre de sécurité quand l'utilisateur se connecte.
					// Cette fonction de sécurité est utilisée quand vous changez un mot de passe ou ajoutez une connexion externe à votre compte.  
					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
						validateInterval: TimeSpan.FromMinutes(30),
						regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
				}
			});
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Permet à l'application de stocker temporairement les informations utilisateur lors de la vérification du second facteur dans le processus d'authentification à 2 facteurs.
			app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

			// Permet à l'application de mémoriser le second facteur de vérification de la connexion, un numéro de téléphone ou un e-mail par exemple.
			// Lorsque vous activez cette option, votre seconde étape de vérification pendant le processus de connexion est mémorisée sur le poste à partir duquel vous vous êtes connecté.
			// Ceci est similaire à l'option RememberMe quand vous vous connectez.
			app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

			// Supprimer les commentaires des lignes suivantes pour autoriser la connexion avec des fournisseurs de connexions tiers
			//app.UseMicrosoftAccountAuthentication(
			//    clientId: "",
			//    clientSecret: "");

			//app.UseTwitterAuthentication(
			//   consumerKey: "",
			//   consumerSecret: "");

			var facebookOptions = new FacebookAuthenticationOptions
			{
				AppId = ConfigurationManager.AppSettings["FacebookAppId"],
				AppSecret = ConfigurationManager.AppSettings["FacebookAppSecret"],
				Scope = { "public_profile", "email"},
				Provider = new FacebookAuthenticationProvider()
				{
					OnAuthenticated = context =>
					{
						var client = new FacebookClient(context.AccessToken);
						dynamic info = client.Get("me", new { fields = "name,id,email,first_name,last_name" });

						context.Identity.AddClaim(new Claim(ClaimTypes.Email, info.email));
						context.Identity.AddClaim(new Claim(ClaimTypes.GivenName, info.first_name));
						context.Identity.AddClaim(new Claim(ClaimTypes.Surname, info.last_name));
						return Task.FromResult(0);
					}
				}
			};

			//https://developers.facebook.com/apps/625040680953967/settings/ (GGm page on FB)
			app.UseFacebookAuthentication(facebookOptions);

			//https://console.developers.google.com/apis/dashboard?project=gamegroupmanager&duration=PT1H (GGm page on Google)
			app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			{
				ClientId = ConfigurationManager.AppSettings["GoogleClientId"],
				ClientSecret = ConfigurationManager.AppSettings["GoogleClientSecret"]
			});

			//https://docs.microsoft.com/en-us/aspnet/identity/overview/features-api/best-practices-for-deploying-passwords-and-other-sensitive-data-to-aspnet-and-azure
			//todo: http://stackoverflow.com/questions/13716658/how-to-delete-all-commit-history-in-github
		}
	}
}