using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cms.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Entities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Published = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Deleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    PayloadJson = table.Column<string>(type: "TEXT", nullable: true),
                    Version = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    CreatedAt = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "(strftime('%s','now') * 1000)"),
                    UpdatedAt = table.Column<long>(type: "INTEGER", nullable: false, defaultValueSql: "(strftime('%s','now') * 1000)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entities", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entities");
        }
    }
}
