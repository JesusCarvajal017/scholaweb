namespace Entity.Model
{
    public class UserRol
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public int RolId { get; set; }

        public User User { get; set; }
        public Rol Rol { get; set; }

    }
}
