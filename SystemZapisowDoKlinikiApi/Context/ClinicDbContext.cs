﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SystemZapisowDoKlinikiApi.Models;

public partial class ClinicDbContext : DbContext
{
    public ClinicDbContext()
    {
    }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalInformation> AdditionalInformations { get; set; }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<DaySchemeTimeBlock> DaySchemeTimeBlocks { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    public virtual DbSet<DoctorBlock> DoctorBlocks { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Service?> Services { get; set; }

    public virtual DbSet<ServiceDependency> ServiceDependencies { get; set; }

    public virtual DbSet<ServicesTranslation> ServicesTranslations { get; set; }

    public virtual DbSet<TimeBlock> TimeBlocks { get; set; }

    public virtual DbSet<Tooth> Teeth { get; set; }

    public virtual DbSet<ToothStatus> ToothStatuses { get; set; }

    public virtual DbSet<ToothStatusTranslation> ToothStatusTranslations { get; set; }

    public virtual DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Polish_CI_AS");

        modelBuilder.Entity<AdditionalInformation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Additional_information_pk");

            entity.ToTable("Additional_information");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BodyEn)
                .HasMaxLength(1000)
                .HasDefaultValue("")
                .HasColumnName("body_en");
            entity.Property(e => e.BodyPl)
                .HasMaxLength(1000)
                .HasColumnName("body_pl");
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Appointment_pk");

            entity.ToTable("Appointment");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DoctorBlockId).HasColumnName("Doctor_block_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.DoctorBlock).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorBlockId)
                .HasConstraintName("FK_Appointment_DoctorBlock");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Appointment_User");

            entity.HasMany(d => d.AdditionalInformations).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "ApointmentInformation",
                    r => r.HasOne<AdditionalInformation>().WithMany()
                        .HasForeignKey("AdditionalInformationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AppInfo_AdditionalInformation"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("AppointmentId")
                        .HasConstraintName("FK_AppInfo_Appointment"),
                    j =>
                    {
                        j.HasKey("AppointmentId", "AdditionalInformationId").HasName("Apointment_informations_pk");
                        j.ToTable("Apointment_informations");
                        j.IndexerProperty<int>("AppointmentId").HasColumnName("Appointment_id");
                        j.IndexerProperty<int>("AdditionalInformationId").HasColumnName("Additional_information_id");
                    });

            entity.HasMany(d => d.Services).WithMany(p => p.Appointments)
                .UsingEntity<Dictionary<string, object>>(
                    "AppointmentsService",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServicesId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_AppointmentsServices_Services"),
                    l => l.HasOne<Appointment>().WithMany()
                        .HasForeignKey("AppointmentId")
                        .HasConstraintName("FK_AppointmentsServices_Appointment"),
                    j =>
                    {
                        j.HasKey("AppointmentId", "ServicesId").HasName("Appointments_services_pk");
                        j.ToTable("Appointments_services");
                        j.IndexerProperty<int>("AppointmentId").HasColumnName("Appointment_id");
                        j.IndexerProperty<int>("ServicesId").HasColumnName("Services_id");
                    });
        });

        modelBuilder.Entity<DaySchemeTimeBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Day_scheme_time_block_pk");

            entity.ToTable("Day_scheme_time_block");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DoctorUserId).HasColumnName("Doctor_User_id");
            entity.Property(e => e.JobEnd)
                .HasPrecision(0)
                .HasColumnName("job_end");
            entity.Property(e => e.JobStart)
                .HasPrecision(0)
                .HasColumnName("job_start");
            entity.Property(e => e.WeekDay).HasColumnName("week_day");

            entity.HasOne(d => d.DoctorUser).WithMany(p => p.DaySchemeTimeBlocks)
                .HasForeignKey(d => d.DoctorUserId)
                .HasConstraintName("FK_DaySchemeTimeBlock_Doctor");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("Doctor_pk");

            entity.ToTable("Doctor");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("User_id");
            entity.Property(e => e.ImgPath)
                .HasMaxLength(255)
                .HasColumnName("img_path");
            entity.Property(e => e.Specialization)
                .HasMaxLength(100)
                .HasColumnName("specialization");

            entity.HasOne(d => d.User).WithOne(p => p.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .HasConstraintName("FK_Doctor_User");
        });

        modelBuilder.Entity<DoctorBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Doctor_block_pk");

            entity.ToTable("Doctor_block");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DoctorUserId).HasColumnName("Doctor_User_id");
            entity.Property(e => e.TimeBlockId).HasColumnName("Time_block_id");

            entity.HasOne(d => d.DoctorUser).WithMany(p => p.DoctorBlocks)
                .HasForeignKey(d => d.DoctorUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DoctorBlock_Doctor");

            entity.HasOne(d => d.TimeBlock).WithMany(p => p.DoctorBlocks)
                .HasForeignKey(d => d.TimeBlockId)
                .HasConstraintName("FK_DoctorBlock_TimeBlock");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.HasMany(d => d.Services).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "RoleServicePermission",
                    r => r.HasOne<Service>().WithMany()
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Role_Service_Permissions_Service"),
                    l => l.HasOne<Role>().WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_Role_Service_Permissions_Role"),
                    j =>
                    {
                        j.HasKey("RoleId", "ServiceId").HasName("PK__Role_Ser__95E9BE46E4746543");
                        j.ToTable("Role_Service_Permissions");
                        j.IndexerProperty<int>("RoleId").HasColumnName("role_id");
                        j.IndexerProperty<int>("ServiceId").HasColumnName("service_id");
                    });
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Services_pk");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.HighPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("high_price");
            entity.Property(e => e.LowPrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("low_price");
            entity.Property(e => e.MinTime).HasColumnName("min_time");
        });

        modelBuilder.Entity<ServiceDependency>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ServiceD__3213E83F4B64088B");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RequiredServiceId).HasColumnName("required_service_id");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.RequiredService).WithMany(p => p.ServiceDependencyRequiredServices)
                .HasForeignKey(d => d.RequiredServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ServiceDependencies_RequiredService");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceDependencyServices)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_ServiceDependencies_Service");
        });

        modelBuilder.Entity<ServicesTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Services_translation_pk");

            entity.ToTable("Services_translation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .HasColumnName("description");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(2)
                .HasColumnName("language_code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ServiceId).HasColumnName("service_id");

            entity.HasOne(d => d.Service).WithMany(p => p.ServicesTranslations)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_Services_translation_Service");
        });

        modelBuilder.Entity<TimeBlock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Time_block_pk");

            entity.ToTable("Time_block");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.TimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("time_end");
            entity.Property(e => e.TimeStart)
                .HasColumnType("datetime")
                .HasColumnName("time_start");
        });

        modelBuilder.Entity<Tooth>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tooth_pk");

            entity.ToTable("Tooth");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ToothNumber).HasColumnName("tooth_number");
            entity.Property(e => e.ToothStatusId).HasColumnName("Tooth_status_id");
            entity.Property(e => e.UserId).HasColumnName("User_id");

            entity.HasOne(d => d.ToothStatus).WithMany(p => p.Teeth)
                .HasForeignKey(d => d.ToothStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tooth_ToothStatus");

            entity.HasOne(d => d.User).WithMany(p => p.Teeth)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Tooth_User");
        });

        modelBuilder.Entity<ToothStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tooth_status_pk");

            entity.ToTable("Tooth_status");

            entity.Property(e => e.Id).HasColumnName("id");
        });

        modelBuilder.Entity<ToothStatusTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Tooth_status_translation_pk");

            entity.ToTable("Tooth_status_translation");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LanguageCode)
                .HasMaxLength(2)
                .HasColumnName("language_code");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.ToothStatusId).HasColumnName("tooth_status_id");

            entity.HasOne(d => d.ToothStatus).WithMany(p => p.ToothStatusTranslations)
                .HasForeignKey(d => d.ToothStatusId)
                .HasConstraintName("FK_Tooth_status_translation");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .HasColumnName("phone_number");
            entity.Property(e => e.RefreshToken)
                .HasMaxLength(512)
                .HasColumnName("refreshToken");
            entity.Property(e => e.RefreshTokenExpDate).HasColumnName("refreshTokenExpDate");
            entity.Property(e => e.RolesId).HasColumnName("Roles_id");
            entity.Property(e => e.Salt)
                .HasMaxLength(255)
                .HasColumnName("salt");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");

            entity.HasOne(d => d.Roles).WithMany(p => p.Users)
                .HasForeignKey(d => d.RolesId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Roles");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
