using Core.Types.BaseTypes;

namespace Core.Types
{
    public class Doctor : PersonBase
    {
        public int Id { get; set; }
        public string Position { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
    }
}