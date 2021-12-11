using System;

namespace Blogpost.Application.Profiles.Queries.Models;

public class MyProfileDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }
}