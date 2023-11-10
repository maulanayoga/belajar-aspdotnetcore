using System.ComponentModel.DataAnnotations;

namespace TestAPI.Models
{
    public class Departement
    {
        [Key]
        public string Id { get; set; }
        public string? Name { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
    }
}
