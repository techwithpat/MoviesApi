using MoviesAPI.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<MoviesDatabaseSettings>(builder.Configuration.GetSection("MoviesDatabaseSettings"));
builder.Services.AddSingleton<MoviesService>();
var app = builder.Build();

/// <summary>
/// 
/// </summary>
app.MapGet("/", () => "Movies API!");

/// <summary>
/// Get all movies
/// </summary>
app.MapGet("/api/movies", async(MoviesService moviesService) => await moviesService.Get());

/// <summary>
/// Get a movie by id
/// </summary>
app.MapGet("/api/movies/{id}", async(MoviesService moviesService, string id) => 
{ 
    var movie = await moviesService.Get(id);
    return movie is null ? Results.NotFound() : Results.Ok(movie);
});

/// <summary>
/// Create a new movie
/// </summary>
app.MapPost("/api/movies", async (MoviesService moviesService, Movie movie) => 
{
    await moviesService.Create(movie);
    return Results.Ok();
});

/// <summary>
/// Update a movie
/// </summary>
app.MapPut("/api/movies/{id}", async(MoviesService moviesService, string id, Movie updatedMovie) => 
{
    var movie = await moviesService.Get(id);
    if (movie is null) return Results.NotFound();

    updatedMovie.Id = movie.Id;
    await moviesService.Update(id, updatedMovie);

    return Results.NoContent();
});

/// <summary>
/// Delete a movie
/// </summary>
app.MapDelete("/api/movies/{id}", async (MoviesService moviesService, string id) =>
{
    var movie = await moviesService.Get(id);
    if (movie is null) return Results.NotFound();

    await moviesService.Remove(movie.Id);

    return Results.NoContent();
});

app.Run();
