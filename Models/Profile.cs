﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

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

    public DateOnly? Birthday { get; set; }
}