namespace Entity.Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Identification { get; set; }
        public int Age { get; set; }
        public int Status { get; set; }

        //relación con user 
        public List<User> User { get; set; }

    }
}
