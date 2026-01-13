using FootLab.Bussines.Mapping;
using FootLab.Bussines.Services;
using FootLab.Bussines.Services.LetsLearnEnglish.Bussines.Services.BaseService;
using FootLab.DataAccess;
using FootLab.DataAccess.Repositories;
using FootLab.Entities.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//mapper tanimi
builder.Services.AddAutoMapper(config => {
    config.AddProfile<MapProfile>();
});


// AddTransient --> her seferinde yeriden build eder
//builder.Services.AddTransient<IAuthService, AuthService>();
//builder.Services.AddTransient<ITokenService, TokenService>();

// AddScoped --> bir kere buileder her istendiðinde buildleri daðýtýr
//builder.Services.AddScoped<NotFoundFilter>();
builder.Services.AddScoped<IBaseRepository<Team>, BaseRepository<Team>>();
builder.Services.AddScoped<IBaseRepository<Player>, BaseRepository<Player>>();
builder.Services.AddScoped<IBaseRepository<Match>, BaseRepository<Match>>();
builder.Services.AddScoped<IBaseRepository<Goal>, BaseRepository<Goal>>();
builder.Services.AddScoped<IBaseRepository<League>, BaseRepository<League>>();
builder.Services.AddScoped<IBaseRepository<Group>, BaseRepository<Group>>();
builder.Services.AddScoped<IBaseRepository<Standing>, BaseRepository<Standing>>();
builder.Services.AddScoped<IBaseRepository<TeamSeasonDetail>, BaseRepository<TeamSeasonDetail>>();



builder.Services.AddScoped<IBaseService<Team>, BaseService<Team>>();
builder.Services.AddScoped<IBaseService<Player>, BaseService<Player>>();
builder.Services.AddScoped<IBaseService<Match>, BaseService<Match>>();
builder.Services.AddScoped<IBaseService<Goal>, BaseService<Goal>>();
builder.Services.AddScoped<IBaseService<League>, BaseService<League>>();
builder.Services.AddScoped<IBaseService<Group>, BaseService<Group>>();
builder.Services.AddScoped<IBaseService<Standing>, BaseService<Standing>>();
builder.Services.AddScoped<IBaseService<TeamSeasonDetail>, BaseService<TeamSeasonDetail>>();

//Web Scrapping
builder.Services.AddScoped<TffScraper>();

builder.Services.AddDbContext<DataContext>(x =>
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        option =>
        {
            option.MigrationsAssembly(Assembly.GetAssembly(typeof(DataContext))?.GetName().Name);
        }
    ));

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
