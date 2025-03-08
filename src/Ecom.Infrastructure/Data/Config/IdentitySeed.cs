using Ecom.Core.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Data.Config
{
    public class IdentitySeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName = "Ibrahim",
                    Email = "ibrahim@gmail.com",
                    UserName = "ibrahim@gmail.com",
                    Address = new Address
                    {
                        FirstName = "ibrahim",
                        LastName = "abbas",
                        City = "azaz",
                        State = "haram",
                        Street = "test street",
                        ZipCode = "123"
                    }
                };
                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}
