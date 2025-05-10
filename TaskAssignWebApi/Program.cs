using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TaskAssignWebApi.Domain;
using TaskAssignWebApi.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(TaskAssignMappingProfile));

builder.Services.AddDbContext<TaskAssignDbContext>(options => options.UseInMemoryDatabase("TaskAssignDatabase"));

builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAllOrigins", policy => policy.AllowAnyOrigin().AllowAnyMethod().WithHeaders("Content-Type", "Authorization"));
});

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
	options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
	options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<TaskAssignDbContext>();
	dbContext.SeedData();
}

app.UseHttpsRedirection();

app.UseCors("AllowAllOrigins");

app.UseAuthorization();

app.MapControllers();

app.Run();
