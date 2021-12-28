
using System.Threading.Tasks;
using API.Entities;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace API.Data
{
    public static class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {

            if (await context.Users.AnyAsync()) return;

            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");

            var users = JsonConvert.DeserializeObject<List<AppUser>>(userData);


            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();

                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("123"));
                user.PasswordSalt = hmac.Key;
                
                context.Users.Add(user);
            }

            await context.SaveChangesAsync()
;        }
    }
}