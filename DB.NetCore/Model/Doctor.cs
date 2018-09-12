namespace DB.NetCore.Model
{
    public class Doctor
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PatronymicName { get; set; }
        public string Position { get; set; }
    }
}