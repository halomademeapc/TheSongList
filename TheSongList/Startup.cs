using IF.Lastfm.Core.Api;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Security.Claims;
using TheSongList.Authorization;
using TheSongList.Services;

namespace TheSongList
{
    public class Startup
    {
        public IHostingEnvironment env;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<SongContext>(options =>
            {
                options.UseSqlite("Data Source=songs.db");
            });

            services.AddScoped<SearchService>();
            services.AddScoped(provider => new LastfmClient(
                Configuration["Lastfm:ClientId"],
                Configuration["Lastfm:ClientSecret"]
                ));
            services.AddScoped<MetadataService>();

            services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddAuthentication(v =>
                {
                    v.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
                    v.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddGoogle(o =>
                {
                    o.ClientId = Configuration["Authentication:Google:ClientId"];
                    o.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                    o.UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
                    o.ClaimActions.Clear();
                    o.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
                    o.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
                    o.ClaimActions.MapJsonKey("urn:google:profile", "link");
                    o.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
                });

            services.AddMvc(options =>
            {
                if (env.IsDevelopment())
                {
                    options.Filters.Add(new AllowAnonymousFilter());
                }
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "The Song List",
                    Version = "v1",
                    Description = "All the songs in Supernatural",
                    Contact = new Contact
                    {
                        Name = "Alex Griffith",
                        Email = "support@halomademeapc.com",
                        Url = "https://www.halomademeapc.com"
                    }
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CanEdit", policy =>
                    policy.Requirements.Add(new HasEmailRequirement(GetPermittedEmails())));
            });

            services.AddSingleton<IAuthorizationHandler, HasEmailHandler>();
        }

        private List<string> GetPermittedEmails()
        {
            try
            {
                return Configuration.GetSection("PermittedEmails").Get<List<string>>();
            }
            catch
            {
                return new List<string>();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, SongContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            db.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "The Song List v1");
            });

            app.UseAuthentication().UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
