using TestAPI.Models;

namespace TestAPI.ViewModels
{
    public class UpdateVM
    {
        public string NIK { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string? DepId { get; set; }
        public Status Status { get; set; }
    }
}
