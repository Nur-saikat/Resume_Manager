using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ResumeManager.Models
{
    public class Experience
    {
        public Experience() 
        { 
        }
        [Key]
        public int ExperinceId { get; set; }
        [ForeignKey("Applicant")]
        public int AppliApplicant { get; set; } 
        public virtual Applicant Applicant { get; private set; }

        public string CompanyName { get; set; } = "";
        public string Designation {  get; set; } = "";

        [Range(1,25,ErrorMessage ="Years Must Be Between 1 and 12")]
        [Required]
        public int YearsWorkrd { get; set; }

    }


}
