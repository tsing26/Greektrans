using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using static GreekTransWeb.Controllers.ApiController;

namespace GreekTransWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                // https://stackoverflow.com/questions/36452468/swagger-ui-web-api-documentation-present-enums-as-strings
                .AddJsonOptions(x =>
                {
                    // 禁止返回的属性名使用 camel 形态
                    // https://stackoverflow.com/questions/58476681/asp-net-core-3-0-system-text-json-camel-case-serialization
                    x.JsonSerializerOptions.PropertyNamingPolicy = null;
                    x.JsonSerializerOptions.WriteIndented = true;
                    // x.JsonSerializerOptions.Converters.Add(new Controllers.v3Controller.ByteArrayConverter());
                });

            {
                builder.Services.AddSwaggerGen(c =>
                {
                    // https://stackoverflow.com/questions/58834430/c-sharp-net-core-swagger-trying-to-use-multiple-api-versions-but-all-end-point
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "GreekTrans API V1",
                        Description = "GreekTrans API 希腊文罗马化",
                        Version = "v1",
                        License = new OpenApiLicense
                        {
                            Name = "Apache-2.0",
                            Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0.html")
                        },
                        Contact = new OpenApiContact
                        {
                            Name = "xietao",
                            Email = "xietao@dp2003.com",
                            Url = new Uri("https://github.com/tsing26/Greektrans")
                        }
                    });

                    // c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                    // version < 3.0 like this: c.OperationFilter<ExamplesOperationFilter>(); 
                    // version 3.0 like this: c.AddSwaggerExamples(services.BuildServiceProvider());
                    // version > 4.0 like this:
                    c.ExampleFilters();
                    // c.SchemaFilter<EnumSchemaFilter>();

                });

                builder.Services.AddSwaggerExamplesFromAssemblyOf<TransliterResponseExamples>();
            }

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", "GreekTrans API V1");
                c.SwaggerEndpoint($"/swagger/swagger.json", "GreekTrans API V1");
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Trans}/{greek?}");

            app.Run();
        }
    }
}