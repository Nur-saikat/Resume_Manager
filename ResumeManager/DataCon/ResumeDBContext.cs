using Microsoft.EntityFrameworkCore;
using ResumeManager.Models;

namespace ResumeManager.DataCon
{
    public class ResumeDBContext : DbContext
    {
        public ResumeDBContext(DbContextOptions<ResumeDBContext> options) : base(options)
        {
        }
        public virtual DbSet<Applicant> Applicants { get; set; }
        public virtual DbSet<Experience> Experinces { get; set; }

    }
}
