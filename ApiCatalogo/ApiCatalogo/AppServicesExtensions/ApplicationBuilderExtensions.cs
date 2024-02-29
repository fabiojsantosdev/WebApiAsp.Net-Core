namespace ApiCatalogo.AppServicesExtensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder app, 
            IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            return app;
        }

        public static IApplicationBuilder UseAppCors(this IApplicationBuilder app)
        {

            string[] origens =
          {
               "http://192.168.0.157",
                "http://192.168.0.95:5501",
            };

            app.UseCors(p =>
            {
                p.WithOrigins(origens);
                p.WithMethods("GET","POST","PUT","DELETE","OPTIONS");
                p.AllowAnyHeader();
            });
            return app;
        }
        public static IApplicationBuilder UseSwaggerMiddleware(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { });
            return app;
        }
    }
}
