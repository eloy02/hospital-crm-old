using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
        public async Task<bool> AddUserAsync(User user, string password)
        {
            user.UserGuid = Guid.NewGuid();
            user.Password = Security.HashSHA1($"{password}{user.UserGuid}");

            using (var db = new HospitalContext())
            {
                db.User.Add(user);

                await db.SaveChangesAsync();
            }

            return false;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.User.ToListAsync();

                return raw;
            }
        }

        public async Task<bool> CheckUserAsync(int userId, string password)
        {
            using (var db = new HospitalContext())
            {
                var dbuser = await db.User.FindAsync(userId);

                if (dbuser != null)
                {
                    string hashedPassword = Security.HashSHA1($"{password}{dbuser.UserGuid}");

                    if (dbuser.Password == hashedPassword)
                        return true;
                    else return false;
                }
                else return false;
            }
        }
    }
}