using Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebClient.Interfaces;

namespace PacientRegistry.Models
{
    public class ShellModel
    {
        public List<Pacient> Pacients = new List<Pacient>();
        public readonly IWebClient WebClient;

        public ShellModel(IWebClient webClient)
        {
            WebClient = webClient;
        }

        public async Task SavePacientAsync(Pacient pacient)
        {
            byte[] file = null;

            using (var stream = new FileStream(pacient.DocumentPath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            var doc = new Document
            {
                Content = file.ToList(),
                Name = $"{pacient.LastName}{pacient.FirstName}{pacient.PatronymicName}"
            };

            pacient.Document = doc;

            await WebClient.SavePacientAsync(pacient);
        }

        public async Task GetProgrammTokenAsync()
        {
            await WebClient.GetProgrammTokenAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await WebClient.GetPacientsAsync();

                if (p != null)
                {
                    Pacients.Clear();
                    Pacients.AddRange(p);

                    return p.ToList();
                }
                else return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteToken()
        {
            await WebClient.DeleteToken();
        }
    }
}