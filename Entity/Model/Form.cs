namespace Entity.Model
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }    
        public string Description { get; set; }

        public int Status { get; set; }

        //public ICollection<RolFormPermission> RolFormPermissions { get; set; }

        //public ICollection<ModuleForm> ModuleForm { get; set; }
    }
}
