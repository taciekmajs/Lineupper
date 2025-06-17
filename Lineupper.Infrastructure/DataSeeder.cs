using Lineupper.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Lineupper.Infrastructure
{
    public static class DataSeeder
    {
        public static void SeedDatabase(LineupperDbContext context)
        {
            context.Database.Migrate();

            // SPRAWDZANIE I DODAWANIE ORGANIZATORÓW
            if (!context.Organizers.Any())
            {
                var organizers = new[]
                {
                    new Organizer
                    {
                        Id = Guid.NewGuid(),
                        Username = "organizer1",
                        Email = "org1@example.com",
                        PasswordHash = "org1haslo",
                        UserType = SharedKernel.Enums.UserType.Organizer
                    },
                    new Organizer
                    {
                        Id = Guid.NewGuid(),
                        Username = "organizer2",
                        Email = "org2@example.com",
                        PasswordHash = "org2haslo",
                        UserType = SharedKernel.Enums.UserType.Organizer
                    }
                };

                context.AddRange(organizers);
                context.SaveChanges();
            }

            // SPRAWDZANIE I DODAWANIE UCZESTNIKÓW
            if (!context.Participants.Any())
            {
                var participants = new[]
                {
                    new Participant
                    {
                        Id = Guid.NewGuid(),
                        Username = "participant1",
                        Email = "part1@example.com",
                        PasswordHash = "part1haslo",
                        UserType = SharedKernel.Enums.UserType.Participant
                    },
                    new Participant
                    {
                        Id = Guid.NewGuid(),
                        Username = "participant2",
                        Email = "part2@example.com",
                        PasswordHash = "part2haslo",
                        UserType = SharedKernel.Enums.UserType.Participant
                    }
                };

                context.AddRange(participants);
                context.SaveChanges();
            }
        }
    }
}
