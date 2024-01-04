﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Reena.MSSQL.Models;

public partial class ReemaContext : DbContext
{
    public ReemaContext(DbContextOptions<ReemaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<FavoriteActivity> FavoriteActivities { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<SpokenLanguage> SpokenLanguages { get; set; }

    public virtual DbSet<UserProfileView> UserProfileViews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__AuditLog__5E5499A807BB5D8A");

            entity.ToTable("AuditLog", "CW1");

            entity.Property(e => e.LogId).HasColumnName("LogID");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OperationDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.OperationType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.TableName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<FavoriteActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId).HasName("PK__Favorite__393F5BA55F06C570");

            entity.ToTable("FavoriteActivity", "CW1");

            entity.Property(e => e.ActivityId)
                .ValueGeneratedNever()
                .HasColumnName("Activity_ID");
            entity.Property(e => e.ActivityName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Activity_Name");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Profile__1788CCAC34A7880D");

            entity.ToTable("Profile", "CW1", tb => tb.HasTrigger("ProfileAuditTrigger"));

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.AboutMe)
                .HasColumnType("text")
                .HasColumnName("About_Me");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<SpokenLanguage>(entity =>
        {
            entity.HasKey(e => e.SpokenLanguageId).HasName("PK__Spoken_L__52DC6802D4CF70EB");

            entity.ToTable("Spoken_Language", "CW1");

            entity.Property(e => e.SpokenLanguageId)
                .ValueGeneratedNever()
                .HasColumnName("Spoken_LanguageID");
            entity.Property(e => e.SpokenLanguageName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("SpokenLanguage_Name");
        });

        modelBuilder.Entity<UserProfileView>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UserProfileView", "CW1");

            entity.Property(e => e.AboutMe)
                .HasColumnType("text")
                .HasColumnName("About_Me");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FavoriteActivity)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("First_Name");
            entity.Property(e => e.LastName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("Last_Name");
            entity.Property(e => e.SpokenLanguage)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingGeneratedProcedures(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}