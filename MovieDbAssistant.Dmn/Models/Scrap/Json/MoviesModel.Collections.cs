using MovieDbAssistant.Lib.Extensions;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

/// <summary>
/// The movies model.
/// </summary>
public sealed partial class MoviesModel
{
    /// <summary>
    /// apply distinct on movie list
    /// </summary>
    public MoviesModel Distinct()
    {
        var movies = Movies.GroupBy(x => x.Id)
            .Select(x => x.First())
            .ToList();
        Movies.Clear();
        Movies.AddRange(movies);
        return this;
    }

    /// <summary>
    /// sort by title
    /// </summary>
    public MoviesModel Sort()
    {
        Movies.Sort(new Comparison<MovieModel>(
            (x, y) => x.Title == null ?
                -1 : x.Title.CompareTo(y.Title)
            ));
        return this;
    }

    /// <summary>
    /// index movies in the list (from 0)
    /// </summary>
    /// <returns>A <see cref="MoviesModel"/></returns>
    public MoviesModel Index()
    {
        var idx = 0;
        foreach (var item in Movies)
            item.ListIndex = idx++;
        return this;
    }

    /// <summary>
    /// remove unacceptable models
    /// </summary>
    public MoviesModel Filter()
    {
        var movies = Movies
            .Where(x => !string.IsNullOrWhiteSpace(x.Title))
            .ToList();
        Movies.Clear();
        Movies.AddRange(movies);
        return this;
    }

    /// <summary>
    /// merge another model into this one (if not null)
    /// </summary>
    /// <param name="moviesModel">The movies model.</param>
    /// <returns>A <see cref="MoviesModel? "/>this model</returns>
    public MoviesModel? Merge(MoviesModel? moviesModel)
    {
        if (moviesModel == null) return moviesModel;
        QueryCacheFiles = QueryCacheFiles.AddRangeDistinct(
            moviesModel.QueryCacheFiles)!;
        Movies.AddRange(moviesModel.Movies);
        return this;
    }
}
