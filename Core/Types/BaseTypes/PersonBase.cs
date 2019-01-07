namespace Core.Types.BaseTypes
{
    public class PersonBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }

        public string FIO
        {
            get
            {
                var result = string.Empty;

                if (!string.IsNullOrEmpty(LastName))
                    result = $"{result} {LastName}";

                if (!string.IsNullOrEmpty(FirstName))
                    result = $"{result} {FirstName}";

                if (!string.IsNullOrEmpty(PatronymicName))
                    result = $"{result} {PatronymicName}";

                return result;
            }
        }
    }
}