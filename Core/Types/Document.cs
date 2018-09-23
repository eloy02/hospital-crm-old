using System.Collections.Generic;

namespace Core.Types
{
    public class Document
    {
        public int Id { get; set; }
        public List<byte> Content { get; set; }
        public string Name { get; set; }
    }
}