using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FileProcessingRepository.Migrations
{
  /// <inheritdoc />
  public partial class InitialCreate : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.EnsureSchema(
          name: "files");

      migrationBuilder.CreateTable(
          name: "Files",
          schema: "files",
          columns: table => new
          {
            Id = table.Column<Guid>(type: "uuid", nullable: false),
            FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
            Url = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
            Status = table.Column<int>(type: "integer", maxLength: 32, nullable: false),
            CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Files", x => x.Id);
          });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Files",
          schema: "files");
    }
  }
}
