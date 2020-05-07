using Microsoft.EntityFrameworkCore.Migrations;

namespace Wallppr.Migrations
{
    public partial class First : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SettingName = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true),
                    IsEditable = table.Column<int>(nullable: false),
                    DefaultValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallpapers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UId = table.Column<string>(nullable: true),
                    AddedDate = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    Thumbnail = table.Column<string>(nullable: true),
                    WallpaperUrl = table.Column<string>(nullable: true),
                    WallpaperThumbUrl = table.Column<string>(nullable: true),
                    DimensionX = table.Column<int>(nullable: false),
                    DimensionY = table.Column<int>(nullable: false),
                    IsFavorite = table.Column<int>(nullable: false),
                    WallpaperType = table.Column<string>(unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallpapers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WallpaperId = table.Column<long>(nullable: false),
                    ColorCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Colors_Wallpapers_WallpaperId",
                        column: x => x.WallpaperId,
                        principalTable: "Wallpapers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Colors_WallpaperId",
                table: "Colors",
                column: "WallpaperId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Wallpapers");
        }
    }
}
