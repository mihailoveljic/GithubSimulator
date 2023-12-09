using GitHubSimulator.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services
    .AddAuthentication(builder.Configuration)
    .AddAuthorization();
builder.Services.AddControllers();
builder.Services
    .AddLogging()
    .AddFactories()
    .AddInfrastructure(builder.Configuration)
    .AddRepositories()
    .AddServices();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Cross-Origin 
builder.Services
    .AddCors(options =>
    {
        options.AddPolicy("AllowOrigin",
            builder => builder.WithOrigins("*")
                              .AllowAnyHeader()
                              .AllowAnyMethod());
    });
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
