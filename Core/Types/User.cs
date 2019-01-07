using Core.Types.BaseTypes;
using Core.Types.Enumerations;

namespace Core.Types
{
    public class User : PersonBase
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public EAccessGroup Access { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {PatronymicName}";
        }
    }
}