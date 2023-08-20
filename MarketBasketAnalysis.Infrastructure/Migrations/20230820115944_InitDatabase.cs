using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketBasketAnalysis.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MiningSettingsProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    MinSupport = table.Column<double>(type: "REAL", nullable: false),
                    MinConfidence = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiningSettingsProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ItemConversionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Item = table.Column<string>(type: "TEXT", nullable: false),
                    Group = table.Column<string>(type: "TEXT", nullable: false),
                    MiningProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemConversionRules", x => x.Id);
                    table.UniqueConstraint("AK_ItemConversionRules_Item_Group", x => new { x.Item, x.Group });
                    table.ForeignKey(
                        name: "FK_ItemConversionRules_MiningSettingsProfiles_MiningProfileId",
                        column: x => x.MiningProfileId,
                        principalTable: "MiningSettingsProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemExclusionRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemPattern = table.Column<string>(type: "TEXT", nullable: false),
                    ExactMatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    IgnoreCase = table.Column<bool>(type: "INTEGER", nullable: false),
                    MiningProfileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemExclusionRules", x => x.Id);
                    table.UniqueConstraint("AK_ItemExclusionRules_ItemPattern_ExactMatch_IgnoreCase", x => new { x.ItemPattern, x.ExactMatch, x.IgnoreCase });
                    table.ForeignKey(
                        name: "FK_ItemExclusionRules_MiningSettingsProfiles_MiningProfileId",
                        column: x => x.MiningProfileId,
                        principalTable: "MiningSettingsProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ItemConversionRules_MiningProfileId",
                table: "ItemConversionRules",
                column: "MiningProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_ItemExclusionRules_MiningProfileId",
                table: "ItemExclusionRules",
                column: "MiningProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemConversionRules");

            migrationBuilder.DropTable(
                name: "ItemExclusionRules");

            migrationBuilder.DropTable(
                name: "MiningSettingsProfiles");
        }
    }
}
