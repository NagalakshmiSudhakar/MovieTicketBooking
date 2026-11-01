
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.Repositories;
using MovieTicketBooking.Repositories.Interfaces;
using MovieTicketBooking.Services;
using MovieTicketBooking.Services.Interfaces;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IShowSeatRepository, ShowSeatRepository>();
builder.Services.AddScoped<IShowSeatService, ShowSeatService>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
