using Microsoft.EntityFrameworkCore.Migrations;

namespace Iris.Datalayer.Migrations
{
    public partial class AddFullTextIndexToPostAndBook : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON Posts(Title Statistical_Semantics, Body Statistical_Semantics) KEY INDEX [PK_dbo.Posts];",
                suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON Books(Name Statistical_Semantics, Description Statistical_Semantics, Author Statistical_Semantics, ISBN Statistical_Semantics, Publisher Statistical_Semantics) KEY INDEX [PK_dbo.Books];",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
