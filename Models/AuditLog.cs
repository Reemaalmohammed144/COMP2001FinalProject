﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Reena.MSSQL.Models;

public partial class AuditLog
{
    public int LogId { get; set; }

    public string TableName { get; set; }

    public string OperationType { get; set; }

    public int? UserId { get; set; }

    public string Email { get; set; }

    public string UserName { get; set; }

    public DateTime? OperationDate { get; set; }
}