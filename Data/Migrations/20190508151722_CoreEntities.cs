using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LitterManager.Data.Migrations
{
    public partial class CoreEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BreedDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FciId = table.Column<int>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    BreedName = table.Column<string>(nullable: true),
                    SectionName = table.Column<string>(nullable: true),
                    GroupName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BreedDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                columns: table => new
                {
                    InvitationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    SenderId = table.Column<string>(nullable: true),
                    OwnerId = table.Column<string>(nullable: true),
                    LitterId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationId);
                });

            migrationBuilder.CreateTable(
                name: "Litters",
                columns: table => new
                {
                    LitterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    isActive = table.Column<bool>(nullable: false),
                    MotherPresent = table.Column<bool>(nullable: false),
                    Prices_PriceFrom = table.Column<int>(nullable: false),
                    Prices_PriceTo = table.Column<int>(nullable: false),
                    Prices_PriceAvg = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Litters", x => x.LitterId);
                });

            migrationBuilder.CreateTable(
                name: "LitterBreedDescriptions",
                columns: table => new
                {
                    LitterId = table.Column<int>(nullable: false),
                    BreedDescriptionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitterBreedDescriptions", x => new { x.LitterId, x.BreedDescriptionId });
                    table.ForeignKey(
                        name: "FK_LitterBreedDescriptions_BreedDescriptions_BreedDescriptionId",
                        column: x => x.BreedDescriptionId,
                        principalTable: "BreedDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LitterBreedDescriptions_Litters_LitterId",
                        column: x => x.LitterId,
                        principalTable: "Litters",
                        principalColumn: "LitterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LitterBreedDescriptions_BreedDescriptionId",
                table: "LitterBreedDescriptions",
                column: "BreedDescriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations");

            migrationBuilder.DropTable(
                name: "LitterBreedDescriptions");

            migrationBuilder.DropTable(
                name: "BreedDescriptions");

            migrationBuilder.DropTable(
                name: "Litters");
        }
    }
}
