using Template.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();
builder.Services.AddRepositories();
builder.Services.AddBuilders();
builder.Services.AddConnectionString();

var app = builder.Build();

app.UseSwagger(builder.Environment);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
