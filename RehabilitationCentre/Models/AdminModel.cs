using Core.Types;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace RehabilitationCentre.Models
{
    public class AdminModel
    {
        private readonly IWebClient WebClient;

        public AdminModel(IWebClient webClient)
        {
            WebClient = webClient;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            var r = await WebClient.GetUsersAsync();

            if (r is null)
                return new List<User>();

            return r;
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var r = await WebClient.GetDoctorsAsync();

            if (r is null)
                return new List<Doctor>();

            return r;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            return await WebClient.UpdateUserAsync(user);
        }

        public async Task<Doctor> UpdateDoctorAsync(Doctor doctor)
        {
            return await WebClient.UpdateDoctorAsync(doctor);
        }
    }
}