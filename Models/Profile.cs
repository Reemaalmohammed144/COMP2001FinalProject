﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Reena.MSSQL.Models;

public partial class Profile
{
    public int UserId { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string AboutMe { get; set; }

    public string Birthday { get; set; }

    [JsonIgnore]
    public string? AccessToken { get; set; }
}