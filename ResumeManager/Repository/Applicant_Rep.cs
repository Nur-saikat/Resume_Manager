using ResumeManager.DataCon;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ResumeManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace ResumeManager.Repository
{
    public class Applicant_Rep : IApplicant
    {
        private readonly ResumeDBContext _dbContext;

        public Applicant_Rep(ResumeDBContext dbContext)
        {
            _dbContext = dbContext;
        }
     

       public async Task<IEnumerable<Applicant>> GetApplicants()
        {
            var applicants = await _dbContext.Applicants.ToListAsync();
            var experiences = await _dbContext.Experinces.ToListAsync();

            foreach (var applicant in applicants)
            {
                applicant.Experinces = experiences.Where(e => e.Applicant.Id == applicant.Id).ToList();
            }

            return applicants;
        }
      
    }
}
