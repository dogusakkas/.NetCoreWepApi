using Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Config
{
    public class BookConfig : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasData(
                new Book { Id = 1, Title = "C# Programming", Price = 154 },
                new Book { Id = 2, Title = "ASP.NET Core", Price = 200 },
                new Book { Id = 3, Title = "Entity Framework Core", Price = 180 },
                new Book { Id = 4, Title = "Blazor Development", Price = 220 },
                new Book { Id = 5, Title = "Microservices Architecture", Price = 300 },
                new Book { Id = 6, Title = "Design Patterns in C#", Price = 250 },
                new Book { Id = 7, Title = "Azure for Developers", Price = 275 },
                new Book { Id = 8, Title = "Docker and Kubernetes", Price = 320 },
                new Book { Id = 9, Title = "Unit Testing in .NET", Price = 190 },
                new Book { Id = 10, Title = "Web API Development", Price = 210 },
                new Book { Id = 11, Title = "LINQ Fundamentals", Price = 160 },
                new Book { Id = 12, Title = "C# Advanced Topics", Price = 230 },
                new Book { Id = 13, Title = "Performance Optimization in .NET", Price = 280 },
                new Book { Id = 14, Title = "Security Best Practices", Price = 240 },
                new Book { Id = 15, Title = "DevOps with .NET", Price = 260 },
                new Book { Id = 16, Title = "Mobile App Development with Xamarin", Price = 290 },
                new Book { Id = 17, Title = "WPF for Desktop Applications", Price = 195 },
                new Book { Id = 18, Title = "SignalR for Real-time Applications", Price = 225 },
                new Book { Id = 19, Title = "C# for Beginners", Price = 150 },
                new Book { Id = 20, Title = "Refactoring Legacy Code", Price = 310 }
            );
        }
    }
}
