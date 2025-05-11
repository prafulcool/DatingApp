using API;
using API.Extensions;

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

app.Run();
