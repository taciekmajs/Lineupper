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
                        PasswordHash = "hashedpassword",
                        UserType = SharedKernel.Enums.UserType.Organizer
                    },
                    new Organizer
                    {
                        Id = Guid.NewGuid(),
                        Username = "organizer2",
                        Email = "org2@example.com",
                        PasswordHash = "hashedpassword2",
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
                        PasswordHash = "securepass",
                        UserType = SharedKernel.Enums.UserType.Participant
                    },
                    new Participant
                    {
                        Id = Guid.NewGuid(),
                        Username = "participant2",
                        Email = "part2@example.com",
                        PasswordHash = "securepass2",
                        UserType = SharedKernel.Enums.UserType.Participant
                    }
                };

                context.AddRange(participants);
                context.SaveChanges();
            }

            // SPRAWDZANIE I DODAWANIE FESTIWALI
            if (!context.Festivals.Any())
            {
                var organizer = context.Organizers.FirstOrDefault(o => o.Username == "organizer1");
                if (organizer != null)
                {
                    var festival = new Festival
                    {
                        Id = Guid.NewGuid(),
                        Name = "Metal Feast",
                        Location = "Kraków",
                        StartDate = DateTime.UtcNow.AddDays(30),
                        EndDate = DateTime.UtcNow.AddDays(33),
                        Status = SharedKernel.Enums.FestivalStatus.BeforeVoting,
                        Organizer = organizer
                    };

                    context.Festivals.Add(festival);
                    context.SaveChanges();

                    // DODANIE ZESPOŁÓW
                    var bands = new[]
                    {
                        new Band
                        {
                            Id = Guid.NewGuid(),
                            Name = "Iron Noise",
                            Genre = "Metal",
                            Festival = festival
                        },
                        new Band
                        {
                            Id = Guid.NewGuid(),
                            Name = "Electric Fury",
                            Genre = "Hard Rock",
                            Festival = festival
                        },
                        new Band
                        {
                            Id = Guid.NewGuid(),
                            Name = "Rage Storm",
                            Genre = "Thrash Metal",
                            Festival = festival
                        }
                    };

                    context.Bands.AddRange(bands);
                    context.SaveChanges();

                    // DODANIE PLANU
                    var scheduleItems = new[]
                    {
                        new ScheduleItem
                        {
                            Id = Guid.NewGuid(),
                            Festival = festival,
                            Band = bands[0],
                            StartTime = festival.StartDate.AddHours(17),
                            EndTime = festival.StartDate.AddHours(18)
                        },
                        new ScheduleItem
                        {
                            Id = Guid.NewGuid(),
                            Festival = festival,
                            Band = bands[1],
                            StartTime = festival.StartDate.AddHours(19),
                            EndTime = festival.StartDate.AddHours(20)
                        },
                        new ScheduleItem
                        {
                            Id = Guid.NewGuid(),
                            Festival = festival,
                            Band = bands[2],
                            StartTime = festival.StartDate.AddHours(21),
                            EndTime = festival.StartDate.AddHours(22)
                        }
                    };

                    context.ScheduleItems.AddRange(scheduleItems);
                    context.SaveChanges();

                    // DODANIE GŁOSÓW
                    var participant = context.Participants.FirstOrDefault(p => p.Username == "participant1");
                    if (participant != null)
                    {
                        var votes = new[]
                        {
                            new Vote
                            {
                                Id = Guid.NewGuid(),
                                Participant = participant,
                                Band = bands[0],
                                CreatedAt = DateTime.UtcNow
                            },
                            new Vote
                            {
                                Id = Guid.NewGuid(),
                                Participant = participant,
                                Band = bands[1],
                                CreatedAt = DateTime.UtcNow
                            }
                        };

                        context.Votes.AddRange(votes);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
