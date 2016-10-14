using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace Pyxis.Migrations
{
    public partial class AddCreatedAtToCacheFile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>("CreatedAt",
                                                 "CacheFiles",
                                                 nullable: false,
                                                 defaultValue: DateTime.Now);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("CreatedAt",
                                        "CacheFiles");
        }
    }
}