using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace bsStoreBook.Migrations
{
    /// <inheritdoc />
    public partial class CreateBookClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Price", "Title" },
                values: new object[,]
                {
                    { 1, 154m, "C# Programming" },
                    { 2, 200m, "ASP.NET Core" },
                    { 3, 180m, "Entity Framework Core" },
                    { 4, 220m, "Blazor Development" },
                    { 5, 300m, "Microservices Architecture" },
                    { 6, 250m, "Design Patterns in C#" },
                    { 7, 275m, "Azure for Developers" },
                    { 8, 320m, "Docker and Kubernetes" },
                    { 9, 190m, "Unit Testing in .NET" },
                    { 10, 210m, "Web API Development" },
                    { 11, 160m, "LINQ Fundamentals" },
                    { 12, 230m, "C# Advanced Topics" },
                    { 13, 280m, "Performance Optimization in .NET" },
                    { 14, 240m, "Security Best Practices" },
                    { 15, 260m, "DevOps with .NET" },
                    { 16, 290m, "Mobile App Development with Xamarin" },
                    { 17, 195m, "WPF for Desktop Applications" },
                    { 18, 225m, "SignalR for Real-time Applications" },
                    { 19, 150m, "C# for Beginners" },
                    { 20, 310m, "Refactoring Legacy Code" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
