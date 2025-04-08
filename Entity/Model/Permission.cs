namespace Entity.Model
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }

        public ICollection<RolFormPermission> RolFormPermissions { get; set; }
    }
}
