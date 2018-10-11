using Core.Types.BaseTypes;

namespace Core.Types
{
    public class User : PersonBase
    {
        public int Id { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName} {PatronymicName}";
        }
    }
}