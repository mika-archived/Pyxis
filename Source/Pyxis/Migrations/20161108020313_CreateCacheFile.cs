using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Pyxis.Migrations
{
    public partial class CreateCacheFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("CacheFiles",
                                         table => new
                                         {
                                             Id = table.Column<int>(nullable: false)
                                                       .Annotation("Autoincrement", true),
                                             Category = table.Column<int>(nullable: false),
                                             CreatedAt = table.Column<DateTime>(nullable: false),
                                             Path = table.Column<string>(nullable: true),
                                             Size = table.Column<long>(nullable: false),
                                             UpdatedAt = table.Column<DateTime>(nullable: false)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_CacheFiles", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CacheFiles");
        }
    }
}