﻿using LitterManager.Authorization;
using LitterManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

// dotnet aspnet-codegenerator razorpage -m Contact -dc ApplicationDbContext -outDir Pages\Contacts --referenceScriptLibraries
namespace LitterManager.Data
{
    public static class SeedData
    {
        #region snippet_Initialize
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com");
                await EnsureRole(serviceProvider, adminID, Constants.ContactAdministratorsRole);
                await EnsureRole(serviceProvider, adminID, Constants.LitterAdministratorsRole);
                await EnsureRole(serviceProvider, adminID, Constants.InvitationAdministratorsRole);

                // allowed user can create and edit contacts that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
                await EnsureRole(serviceProvider, managerID, Constants.ContactManagersRole);
                await EnsureRole(serviceProvider, managerID, Constants.LitterManagersRole);
                await EnsureRole(serviceProvider, managerID, Constants.InvitationManagersRole);

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                                    string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser { UserName = UserName };
                await userManager.CreateAsync(user, testUserPw);
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                                      string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }
        #endregion
        #region snippet1
        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (context.Contact.Any() || context.Litters.Any() || context.Invitations.Any())
            {
                return;   // DB has been seeded
            }

            var breedDescriptionsJson =
            GetEmbeddedResourceAsString("LitterManager.Data.BreedDescriptions.json");

            try
            {
                List<BreedDescription> data = JsonConvert.DeserializeObject<List<BreedDescription>>(breedDescriptionsJson);

                foreach (dynamic bd in data)
                {
                    context.BreedDescriptions.Add(new BreedDescription
                    {
                        FciId = bd.FciId,
                        BreedName = bd.BreedName,
                        GroupId = bd.GroupId,
                        GroupName = bd.GroupName,
                        SectionId = bd.SectionId,
                        SectionName = bd.SectionName

                    });

                }
                context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            
            

            Litter[] seedLitters = new Litter[]
            {
                new Litter { Description = "lorem ipsum", isActive=true, MotherPresent = true, Prices = new PriceRange(100,300) },
                new Litter { Description = "lorem ipsum quorum", isActive=true, MotherPresent = false, Prices = new PriceRange(500,1000) }
            };

            seedLitters[0].BreedDescriptions = new List<LitterBreedDescriptions>
            {
              new LitterBreedDescriptions {
                Litter = seedLitters[0],
                BreedDescription = context.BreedDescriptions.Find(90)
              },
              new LitterBreedDescriptions {
                Litter = seedLitters[0],
                BreedDescription = context.BreedDescriptions.Find(100)
              }
            };

            seedLitters[1].BreedDescriptions = new List<LitterBreedDescriptions>
            {
              new LitterBreedDescriptions {
                Litter = seedLitters[0],
                BreedDescription = context.BreedDescriptions.Find(120)
              }
            };

            Invitation[] seedInvitations = new Invitation[]
            {
                new Invitation{ SenderId = adminID, OwnerId = "someone", Status=InvitationStatus.Reclaimed, LitterId = seedLitters[0].LitterId},
                new Invitation{ SenderId = "someone", OwnerId = adminID},
            };

            context.Litters.AddRange(seedLitters);

            context.Invitations.AddRange(seedInvitations);

            context.Contact.AddRange(
            #region snippet_Contact
                new Contact
                {
                    Name = "Debra Garcia",
                    Address = "1234 Main St",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "debra@example.com",
                    Status = ContactStatus.Approved,
                    OwnerID = adminID
                },
            #endregion
            #endregion
                new Contact
                {
                    Name = "Thorsten Weinrich",
                    Address = "5678 1st Ave W",
                    City = "Redmond",
                    State = "WA",
                    Zip = "10999",
                    Email = "thorsten@example.com",
                    Status = ContactStatus.Submitted,
                    OwnerID = adminID
                },
             new Contact
             {
                 Name = "Yuhong Li",
                 Address = "9012 State st",
                 City = "Redmond",
                 State = "WA",
                 Zip = "10999",
                 Email = "yuhong@example.com",
                 Status = ContactStatus.Rejected,
                 OwnerID = adminID
             },
             new Contact
             {
                 Name = "Jon Orton",
                 Address = "3456 Maple St",
                 City = "Redmond",
                 State = "WA",
                 Zip = "10999",
                 Email = "jon@example.com",
                 Status = ContactStatus.Submitted,
                 OwnerID = adminID
             },
             new Contact
             {
                 Name = "Diliana Alexieva-Bosseva",
                 Address = "7890 2nd Ave E",
                 City = "Redmond",
                 State = "WA",
                 Zip = "10999",
                 Email = "diliana@example.com",
                 OwnerID = adminID
             }
             );
            context.SaveChanges();
        }

        private static string GetEmbeddedResourceAsString(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var names = assembly.GetManifestResourceNames();

            string result;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                result = reader.ReadToEnd();
            }
            return result;
        }
    }
}