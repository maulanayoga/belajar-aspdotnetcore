using TestAPI.Models;

namespace TestAPI.ViewModels
{
    public class EmpVM
    {
        public string NIK { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? DepId { get; set; }
        public string? DepName { get; set; }
        public Status Status { get; set; }
    }
}
