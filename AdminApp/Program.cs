using Core.Types;
using System;
using System.Threading.Tasks;

namespace AdminApp
{
    internal class Program
    {
        private static async Task AddUser()
        {
            Console.Clear();

            Console.WriteLine("Имя: ");
            var firstName = Console.ReadLine();

            Console.WriteLine("Фамилия: ");
            var lastName = Console.ReadLine();

            Console.WriteLine("Отчество: ");
            var patrName = Console.ReadLine();

            Console.WriteLine("Пароль: ");
            var password = Console.ReadLine();

            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                PatronymicName = patrName
            };

            await AdminWebClient.AddUser(user, password);
        }

        private static void Main(string[] args)
        {
            int x = 0;

            while (x != 5)
            {
                Console.Clear();
                Console.WriteLine("Menu");
                Console.WriteLine("1 - Add User");
                Console.WriteLine("2 - Add Doctor");
                Console.WriteLine("5 - Exit");

                if (int.TryParse(Console.ReadLine(), out x))
                {
                    switch (x)
                    {
                        case 1: { AddUser().Wait(); break; }
                        case 2: { break; }
                    }
                }
                else Console.WriteLine("Wrong input");
            }
        }
    }
}