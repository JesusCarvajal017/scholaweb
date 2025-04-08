
namespace Entity.Model
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; } // Por seguridad
        public int Status { get; set; }
        public int PersonId { get; set; } 
        public Person Person { get; set; }
        public ICollection<UserRol> UserRols { get; set; }

    }
}
