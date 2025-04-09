namespace Entity.Model
{
    public class Rol
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        //public ICollection<UserRol> UserRol { get; set; }

        //public ICollection<RolFormPermission> RolFormPermissions { get; set; }

    }
}
