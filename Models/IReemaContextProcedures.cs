﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Reena.MSSQL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace Reena.MSSQL.Models
{
    public partial interface IReemaContextProcedures
    {
        Task<int> DeleteUserProfileAsync(int? UserID, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> InsertUserProfileAsync(int? UserID, string Email, string UserName, string Password, string First_Name, string Last_Name, string About_Me, DateOnly? Birthday, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
        Task<int> UpdateUserProfileAsync(int? UserID, string Email, string UserName, string Password, string First_Name, string Last_Name, string About_Me, DateOnly? Birthday, OutputParameter<int> returnValue = null, CancellationToken cancellationToken = default);
    }
}
