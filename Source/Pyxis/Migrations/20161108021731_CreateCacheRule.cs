using Microsoft.EntityFrameworkCore.Migrations;

namespace Pyxis.Migrations
{
    public partial class CreateCacheRule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable("CacheRules",
                                         table => new
                                         {
                                             Id = table.Column<int>(nullable: false)
                                                       .Annotation("Autoincrement", true),
                                             Key = table.Column<string>(nullable: true),
                                             Value = table.Column<string>(nullable: true)
                                         },
                                         constraints: table => { table.PrimaryKey("PK_CacheRules", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("CacheRules");
        }
    }
}