using Microsoft.EntityFrameworkCore;
using TaskAssignWebApi.Domain;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<TaskAssignDbContext>(options => options.UseInMemoryDatabase("TaskAssignDatabase"));

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
