﻿using Core.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using WebClient.Interfaces;

namespace RehabilitationCentre.Models
{
    public class PacientsListModel
    {
        public readonly IWebClient WebClient;
        private DispatcherTimer Timer;

        public List<Pacient> PacientsList = new List<Pacient>();

        public PacientsListModel(IWebClient webClient)
        {
            WebClient = webClient;

            Timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(10)
            };

            Timer.Tick += Timer_Tick;

            Timer.Start();
        }

        public async Task GetProgrammTokenAsync()
        {
            await WebClient.GetProgrammTokenAsync();
        }

        private async void Timer_Tick(object sender, EventArgs e)
        {
            await GetPacientsAsync();
        }

        public async Task<List<Pacient>> GetPacientsAsync()
        {
            try
            {
                var p = await WebClient.GetPacientsAsync();

                if (p != null)
                {
                    PacientsList.Clear();
                    PacientsList.AddRange(p);

                    return p.ToList();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task OpenPacientDocument(Pacient pacient)
        {
            var doc = await WebClient.GetPacientDocumentAsync(pacient);

            var path = Directory.GetCurrentDirectory() + @"\Temp";
            Directory.CreateDirectory(path);
            var file = $"{path}\\{Guid.NewGuid().ToString()}.pdf";
            File.WriteAllBytes(file, doc.Content.ToArray());
            var pdfProc = System.Diagnostics.Process.Start(file);
        }

        public async Task<IEnumerable<Doctor>> GetDoctorsAsync()
        {
            var d = await WebClient.GetDoctorsAsync();

            if (d != null)
                return d;
            else return null;
        }

        public async Task SetPacientVisitAsync(Pacient selectedPacient, Doctor selectedDoctor)
        {
            await WebClient.SavePacientVisitAsync(selectedPacient, selectedDoctor);
        }

        public async Task DeleteToken()
        {
            await WebClient.DeleteToken();
        }
    }
}