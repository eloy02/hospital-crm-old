using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.DB
{
    public partial class DBServise
    {
        public async Task<User> AddUserAsync(User user, string password)
        {
            user.UserGuid = Guid.NewGuid();
            user.Password = Security.HashSHA1($"{password}{user.UserGuid}");

            using (var db = new HospitalContext())
            {
                var r = db.User.Add(user);

                await db.SaveChangesAsync();

                return r.Entity;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.User.ToListAsync();

                return raw;
            }
        }

        public async Task<User> GetUsersAsync(int userId)
        {
            using (var db = new HospitalContext())
            {
                var raw = await db.User.SingleOrDefaultAsync(u => u.Id == userId);

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

        public async Task<User> UpdateUserAsync(User user)
        {
            using (var db = new HospitalContext())
            {
                var userdb = await db.User.SingleOrDefaultAsync(u => u.Id == user.Id);

                userdb.FirstName = user.FirstName;
                userdb.LastName = user.LastName;
                userdb.PatronymicName = user.PatronymicName;
                userdb.AccessGroup = user.AccessGroup;

                await db.SaveChangesAsync();

                return userdb;
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            using (var db = new HospitalContext())
            {
                var userdb = await db.User.SingleOrDefaultAsync(u => u.Id == id);

                db.User.Remove(userdb);

                await db.SaveChangesAsync();
            }
        }
    }
}