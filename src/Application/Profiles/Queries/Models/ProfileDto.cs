using System;

namespace Blogpost.Application.Profiles.Queries.Models;

public record ProfileDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}