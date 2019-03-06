using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Snap.Server
{
    internal class CorsConfiguration
    {
        public ICollection<Police> Polices { get; } = new List<Police>();
        internal class Police
        {
            public bool WithAnyMethod { get; set; }
            public bool WithAnyHeader { get; set; }
            public bool WithAnyCredential { get; set; }
            public bool WithAnyOrigin { get; set; }
            public bool DisallowCredentials { get; set; }
            public ICollection<string> Methods { get; } = new List<string>();
            public ICollection<string> Headers { get; } = new List<string>();
            public ICollection<string> Origins { get; } = new List<string>();
        }
        internal void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder =>
                {
                    foreach (var corsOptions in Polices)
                    {
                        if (corsOptions.WithAnyCredential)
                            builder.AllowCredentials();
                        else if (corsOptions.DisallowCredentials)
                            builder.DisallowCredentials();

                        if (corsOptions.WithAnyHeader)
                            builder.AllowAnyHeader();
                        else if (corsOptions.Headers.Count > 0)
                            builder.WithHeaders(corsOptions.Headers.ToArray());

                        if (corsOptions.WithAnyMethod)
                            builder.AllowAnyMethod();
                        else if (corsOptions.Methods.Count > 0)
                            builder.WithMethods(corsOptions.Methods.ToArray());

                        if (corsOptions.WithAnyOrigin)
                            builder.AllowAnyOrigin();
                        else if (corsOptions.Origins.Count > 0)
                            builder.WithOrigins(corsOptions.Origins.ToArray());
                    }
                }));
        }
    }
}