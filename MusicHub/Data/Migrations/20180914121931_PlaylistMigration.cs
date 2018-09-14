using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MusicHub.Data.Migrations
{
    public partial class PlaylistMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Locations",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(maxLength: 40, nullable: false),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Playlists_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlaylistSongsConnections",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlaylistID = table.Column<int>(nullable: true),
                    SongID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaylistSongsConnections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PlaylistSongsConnections_Playlists_PlaylistID",
                        column: x => x.PlaylistID,
                        principalTable: "Playlists",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlaylistSongsConnections_Songs_SongID",
                        column: x => x.SongID,
                        principalTable: "Songs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Playlists_UserId",
                table: "Playlists",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongsConnections_PlaylistID",
                table: "PlaylistSongsConnections",
                column: "PlaylistID");

            migrationBuilder.CreateIndex(
                name: "IX_PlaylistSongsConnections_SongID",
                table: "PlaylistSongsConnections",
                column: "SongID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlaylistSongsConnections");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Locations",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
