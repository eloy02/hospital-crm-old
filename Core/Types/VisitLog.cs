using System;

namespace Core.Types
{
    public class VisitLog
    {
        public int Id { get; set; }
        public Doctor Doctor { get; set; }
        public Pacient Pacient { get; set; }
        public DateTime VisitDateTime { get; set; }
    }
}