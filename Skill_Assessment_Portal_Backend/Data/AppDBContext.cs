using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Skill_Assessment_Portal_Backend.Models;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Skill_Assessment_Portal_Backend.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Assessment> Assessments { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<UserAssessment> UserAssessments { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<Result> Results { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // 1. Define the Value Converter (as you already have)
            var stringListConverter = new ValueConverter<List<string>, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null) ?? new List<string>()
            );

            // 2. Define the Value Comparer for List<string>
            var stringListComparer = new ValueComparer<List<string>>(
                (l1, l2) => l1.SequenceEqual(l2), // How to compare two lists for equality
                l => l.Aggregate(0, (hash, s) => HashCode.Combine(hash, s.GetHashCode())), // How to generate a hash code for a list
                l => l.ToList() // How to create a snapshot for change tracking
            );


            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Candidate" },
                new Role { RoleId = 3, RoleName = "Evaluator" }
            );
            modelBuilder.Entity<UserAssessment>()
               .HasIndex(ua => new { ua.UserId, ua.AssessmentId })
               .IsUnique();

            // Configuring Relationships and Delete Behaviors

            // User to Assessment (One-to-Many)
            // A User can create many Assessments.
            // When a User is deleted, we want to prevent the deletion of their created Assessments.
            // A more professional approach is to restrict the deletion and handle it manually.
            modelBuilder.Entity<User>()
               .HasMany(u => u.CreatedAssessments)
               .WithOne(a => a.Creator)
               .HasForeignKey(a => a.CreatedBy)
               .OnDelete(DeleteBehavior.Restrict); // Prevents deleting a User who created Assessments

            // Assessment to Question (One-to-Many)
            // When an Assessment is deleted, all its Questions should be deleted.
            modelBuilder.Entity<Assessment>()
               .HasMany(a => a.Questions)
               .WithOne(q => q.Assessment)
               .HasForeignKey(q => q.AssessmentId)
               .OnDelete(DeleteBehavior.Cascade); // Deletes all Questions when an Assessment is deleted

            // UserAssessment to Submission (One-to-Many)
            // Deleting a test attempt should delete all submissions for it.
            modelBuilder.Entity<UserAssessment>()
               .HasMany(ua => ua.Submissions)
               .WithOne(s => s.UserAssessment)
               .HasForeignKey(s => s.UserAssessmentId)
               .OnDelete(DeleteBehavior.Cascade); // Deletes all Submissions when a UserAssessment is deleted

            // UserAssessment to Result (One-to-One)
            // Deleting a test attempt should delete its result.
            modelBuilder.Entity<UserAssessment>()
               .HasOne(ua => ua.Result)
               .WithOne(r => r.UserAssessment)
               .HasForeignKey<Result>(r => r.UserAssessmentId)
               .OnDelete(DeleteBehavior.Cascade); // Deletes the Result when a UserAssessment is deleted

            //Question to Submission(One-to-Many)
            // When a Question is deleted, we don't want it to automatically cascade delete Submissions
            // because Submissions are already handled by the UserAssessment cascade.
            modelBuilder.Entity<Question>()
                .HasMany(q => q.Submissions)
                .WithOne(s => s.Question)
                .HasForeignKey(s => s.QuestionId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Question>()
                .Property(q => q.Options)
                .HasConversion(stringListConverter, stringListComparer);
        }
    }
}
