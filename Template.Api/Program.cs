using System.Reflection;

using MediatR;

using Template.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(builder.Configuration);
builder.Services.AddSwagger();
builder.Services.AddRepositories();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDomainEvents();
builder.Services.AddBuilders();
builder.Services.AddConnectionString();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger(builder.Environment);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseHealthChecks();

app.Run();
