using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Datalayer.Migrations
{
    public partial class ChangeDatabaseCompatibilityLevelTo120 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"ALTER DATABASE EbooksWorldDb
SET COMPATIBILITY_LEVEL = 120", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
