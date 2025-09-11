using Epers.Models;
using Epers.Models.Evaluare;
using Epers.Models.Nomenclatoare;
using Epers.Models.Obiectiv;
using Epers.Models.PIP;
using Epers.Models.Salesforce;
using Epers.Models.Sincron;
using Epers.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace Epers.DataAccess
{
    public class EpersContext : DbContext
    {
        public EpersContext(DbContextOptions<EpersContext> options) : base(options)
        {
        }

        // Nomenclatoare
        public DbSet<NLocatii> NLocatii { get; set; }
        public DbSet<NCompartimente> NCompartimente { get; set; }
        public DbSet<NDivizii> NDivizii { get; set; }
        public DbSet<NPosturi> NPosturi { get; set; }
        public DbSet<NSkills> NSkills { get; set; }
        public DbSet<NObiective> NObiective { get; set; }
        public DbSet<NCursuri> NCursuri { get; set; }
        public DbSet<NStariPIP> NStariPIP { get; set; }
        public DbSet<NFirme> NFirme { get; set; }



        // SelectionBox
        public DbSet<SelectionBox> SelectionBox { get; set; }

        // SetareProfil
        public DbSet<SetareProfil> SetareProfil { get; set; }


        // Evaluare
        public DbSet<Evaluare_competente> Evaluare_competente { get; set; }
        public DbSet<Notite> Notite { get; set; }

        // Obiective
        public DbSet<Obiective> Obiective { get; set; }

        // Sincron
        public DbSet<EmployeesSincronDbModel> EmployeesSincronDbModel { get; set; }

        // PIP
        public DbSet<PlanInbunatatirePerformante> PlanInbunatatirePerformante { get; set; }

        // User
        public DbSet<User> User { get; set; }

        public DbSet<Rol> Rol { get; set; }

        // Integreare Salesforce
        public DbSet<AgentMetrics> AgentMetrics { get; set; }

        // Proceduri Stocate

    }
}

