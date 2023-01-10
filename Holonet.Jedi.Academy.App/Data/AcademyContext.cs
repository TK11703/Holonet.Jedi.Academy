using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Holonet.Jedi.Academy.Entities.App;
using Holonet.Jedi.Academy.Entities;

namespace Holonet.Jedi.Academy.App.Data
{
    public class AcademyContext : DbContext
    {
        public AcademyContext(DbContextOptions<AcademyContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Quest> Quests { get; set; }
		
		public DbSet<QuestObjective> QuestObjectives { get; set; }
		public DbSet<Objective> Objectives { get; set; }
		public DbSet<ObjectiveDestination> ObjectiveDestinations { get; set; }

		public DbSet<QuestXP> QuestParticipation { get; set; }
		//public DbSet<QuestDestination> QuestDestinations { get; set; }
		
		public DbSet<CompletedObjective> CompletedObjectives { get; set; }

		public DbSet<Rank> Ranks { get; set; }

        public DbSet<ForcePower> ForcePowers { get; set; }
		public DbSet<ForcePowerXP> ForcePowersLearned { get; set; }

		public DbSet<TerminationReason> TerminationReasons { get; set; }

		public DbSet<Planet> Planets { get; set; }

		public DbSet<Species> AlienRaces { get; set; }

		public DbSet<Knowledge> KnowledgeOpportunities { get; set; }
		public DbSet<KnowledgeXP> KnowledgeLearned { get; set; }

		public DbSet<UserProfile> UserProfiles { get; set; }

		public DbSet<Notification> Notifications { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("App");
            //modelBuilder.UseCollation("SQL_Latin1_General_CP1_CS_AS");
            modelBuilder.Entity<Student>().ToTable("Students");
			modelBuilder.Entity<Rank>().ToTable("Ranks");
			modelBuilder.Entity<Species>().ToTable("Species");
			modelBuilder.Entity<Planet>().ToTable("Planets");
			modelBuilder.Entity<Knowledge>().ToTable("KnowledgeOpportunities");
			modelBuilder.Entity<KnowledgeXP>().ToTable("KnowledgeXP");
			modelBuilder.Entity<Quest>().ToTable("Quests");
			modelBuilder.Entity<QuestObjective>().ToTable("QuestObjectives");
			modelBuilder.Entity<Objective>().ToTable("Objectives");
			modelBuilder.Entity<ObjectiveDestination>().ToTable("ObjectiveDestinations");
			modelBuilder.Entity<CompletedObjective>().ToTable("CompletedObjectives");
			modelBuilder.Entity<QuestXP>().ToTable("QuestXP");
			//modelBuilder.Entity<QuestDestination>().ToTable("QuestDestinations");
			modelBuilder.Entity<ForcePower>().ToTable("ForcePowers");
			modelBuilder.Entity<ForcePowerXP>().ToTable("ForcePowerXP");
			modelBuilder.Entity<TerminationReason>().ToTable("TerminationReasons");
			modelBuilder.Entity<UserProfile>().ToTable("UserProfiles");
			modelBuilder.Entity<Notification>().ToTable("Notifications");
		}
    }
}
