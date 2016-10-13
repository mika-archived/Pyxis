using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Pyxis.Migrations
{
    public partial class AddReferencedAtToCacheFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("ReferencedAt",
                                                 "CacheFiles",
                                                 nullable: false,
                                                 defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("ReferencedAt",
                                        "CacheFiles");
        }
    }
}