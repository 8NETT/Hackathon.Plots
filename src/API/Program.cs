using API.Configurations;
using API.Endpoints;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogConfiguration();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddValidation();
builder.Services.AddSwagger();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

app.UseApiRequestLogging();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = string.Empty;
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapPropriedadeEndpoints();
app.MapTalhaoEndpoints();

app.Run();
