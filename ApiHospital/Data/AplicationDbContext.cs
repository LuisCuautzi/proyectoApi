using System.Collections.Generic;
using System;
using Microsoft.EntityFrameworkCore;
using ApiHospital.Models;

namespace ApiHospital.Data
{
    public class AplicationDbContext : DbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Historial> hitorial { get; set; }
        public DbSet<Cita> citas { get; set; }
        public DbSet<Hospital> hospital { get; set; }
        public DbSet<Medico> medicos { get; set; }
        public DbSet<Paciente> pacientes { get; set; }

    }
}

