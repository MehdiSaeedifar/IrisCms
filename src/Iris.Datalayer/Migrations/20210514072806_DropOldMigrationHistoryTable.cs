using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Datalayer.Migrations
{
    public partial class DropOldMigrationHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable("__MigrationHistory");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
