using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;
using GameGroupManager.Models;
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

			//http://stackoverflow.com/questions/25966530/asp-net-mvc-5-1-c-sharp-owin-facebook-authentication-or-login-ask-for-birthday
			//var facebookOptions = new FacebookAuthenticationOptions
			//{
			//	Scope = { "email", "first_name", "last_name" },
			//	AppId = "625040680953967",
			//	AppSecret = "93252fd81a19777f24048f5181d24eff",
			//	Provider = new FacebookAuthenticationProvider()
			//	{
			//		OnAuthenticated = context =>
			//		{
			//			context.Identity.AddClaim(new System.Security.Claims.Claim("FacebookAccessToken", context.AccessToken));
			//			return Task.FromResult(true);
			//		}
			//	}
			//};
			//facebookOptions.SignInAsAuthenticationType = DefaultAuthenticationTypes.ExternalCookie;

			//http://www.codemeworld.com/c-facebookgoogle-login-how-to-get-user-first-name-and-last-name-separately/
			//var facebookOptions = new FacebookAuthenticationOptions
			//{
			//	AppId = "625040680953967",
			//	AppSecret = "93252fd81a19777f24048f5181d24eff",
			//	Scope = { "email", "first_name", "last_name"},
			//	//Note that you need to explicitly specify what you need.
			//	//UserInformationEndpoint = IdentityEnvProperties.FacebookUserInformationEndpoint,
			//	//BackchannelHttpHandler = new FacebookBackChannelHandler(),
			//	Provider = new FacebookAuthenticationProvider
			//	{
			//		OnAuthenticated = context =>
			//		{
			//			context.Identity.AddClaim(new Claim("FacebookAccessToken", context.AccessToken));
			//			return Task.FromResult(true);
			//		}
			//	}
			//};
			//This line is required besides the above configuration.
			//facebookOptions.Scope.Add("email");

			var facebookOptions = new FacebookAuthenticationOptions
			{
				AppId = "625040680953967",
				AppSecret = "93252fd81a19777f24048f5181d24eff"
			};

			//https://developers.facebook.com/apps/625040680953967/settings/ (GGm page on FB)
			app.UseFacebookAuthentication(facebookOptions);

			//app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			//{
			//    ClientId = "",
			//    ClientSecret = ""
			//});
		}
	}
}