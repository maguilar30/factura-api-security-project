using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace factura_api_security_project.Data.Entities
{
    [Table("User",Schema = "sec")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string NationalId{ get; set; }
        public int CountryId { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string Address { get; set; }
        public bool IsActive {  get; set; }
        public DateTime Datetime { get; set;  }
    }
}
