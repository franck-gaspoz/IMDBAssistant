﻿using System.Diagnostics;

namespace MovieDbAssistant.Dmn.Models.Scrap.Json;

#pragma warning disable CD1606 // The property must have a documentation header.

/// <summary>
/// The movie model.
/// </summary>
[DebuggerDisplay("{Title} | {MinPicAlt}")]
public sealed class MovieModel
{
    /// <summary>
    /// url of scraped page: details
    /// </summary>
    public string Url { get; set; } = string.Empty;

    /// <summary>
    /// provider movie id
    /// </summary>
    public string? Id { get; set; }

    /// <summary>
    /// key (base64)
    /// </summary>
    public string? Key { get; set; }

    public string? Title { get; set; }

    public string? Summary { get; set; }

    public List<string> Interests { get; set; } = [];

    public string? Rating { get; set; }

    public string? RatingCount { get; set; }

    public string? Duration { get; set; }

    public string? ReleaseDate { get; set; }

    public string? Year { get; set; }

    public string? Vote { get; set; }

    public string? Director { get; set; }

    public List<string> Writers { get; set; } = [];

    public List<string> Stars { get; set; } = [];

    public List<ActorModel> Actors { get; set; } = [];

    public string? Anecdotes { get; set; }

    public string? MinPicUrl {  get; set; }

    public string? MinPicWidth { get; set; }

    public string? MinPicAlt { get; set; }

    public List<string> PicsUrls { get; set; } = [];

    public string? PicsFullUrls { get; set; }

    public List<string> PicsSizes { get; set; } = [];

    public string? Filename { get; set; }
}
