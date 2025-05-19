using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Service_Image.api.Extensions
{
    public static class ServicesExtension
    {
        //cette methode d'extension nous permet d'appeler builder.services.AddJwtAuthentication()
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            //pour recuperer le cle secret dans appsettings.json
            var key = Encoding.ASCII.GetBytes(config["JwtConfig:Secret"]);

            services.AddAuthentication(options =>
            {
                //pour montrer qu'on utilise JWT ici
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //utilise quand une resource a besoin necessite l'authentification
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    //validation de cle signature de tocken
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    //pour pouvoir verifier que le token vient bien de notre serveur(verification de l'emeteur)
                    ValidateIssuer = false,
                    //pour accepter tous les clients ou non
                    ValidateAudience = false
                };
            });
        }
    }
}
