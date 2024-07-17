using ResumeManager.Models;

namespace ResumeManager.Repository
{
    public interface IApplicant
    {
        
       Task<IEnumerable<Applicant>> GetApplicants();
      

    }
}
