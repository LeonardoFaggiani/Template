using System.Reflection;

using MediatR;

using Template.Api.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthorization(builder.Configuration);
#if Swagger
builder.Services.AddSwagger();
#endif
builder.Services.AddRepositories();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddDomainEvents();
builder.Services.AddBuilders();
builder.Services.AddConnectionString();
#if HealthChecks
builder.Services.AddHealthChecks();
#endif

var app = builder.Build();

#if Swagger
app.UseSwagger(builder.Environment);
#endif

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#if HealthChecks
app.UseHealthChecks();
#endif

app.Run();