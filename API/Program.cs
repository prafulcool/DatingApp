using API;
using API.Data;
using API.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseDeveloperExceptionPage(); // handled by microsoft in development mode
app.UseMiddleware<ExceptionMiddleware>();
app.UseCors(x => x
.WithOrigins("http://localhost:4200","https://localhost:4200")
.AllowAnyHeader()
.AllowAnyMethod()

//.AllowAnyOrigin() // need to check without this
);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

//apply migrations
using var scope = app.Services.CreateScope(); //we need to access a service.

//And in our add application services we've registered several services for dependency injection.

//Now at this stage, when we're inside our program class, we are not injecting anything into this.

//So we need to use a pattern called the service locator to get hold of a service that we wish to use

//outside of dependency injection.

//And that's what we're doing here.

//So we need to create a scope specifically.

//And we want this scope to be disposed of.

//Because this is not coming via an HTTP request that we're doing something with our database.

//So we have to use a different approach here.

//And that's the idea of this.
var services = scope.ServiceProvider;
try
{
    var context = services.GetService<DataContext>();
    await context.Database.MigrateAsync();
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex,"An error occured during migration");
}

app.Run();
