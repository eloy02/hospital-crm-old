namespace DB.NetCore.Model
{
    public class Document
    {
        public int Id { get; set; }
        public string FilePath { get; set; }

        public Pacient Pacient { get; set; }
    }
}