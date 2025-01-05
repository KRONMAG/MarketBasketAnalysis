using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketBasketAnalysis.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameIsLoadedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsLoaded",
                table: "AssociationRuleSets",
                newName: "IsAvailable");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "AssociationRuleSets",
                newName: "IsLoaded");
        }
    }
}
