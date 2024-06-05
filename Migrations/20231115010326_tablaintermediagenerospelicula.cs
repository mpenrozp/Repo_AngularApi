using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiProducto.Migrations
{
    /// <inheritdoc />
    public partial class tablaintermediagenerospelicula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GeneroPeliculas_Generos_GeneroId",
                table: "GeneroPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_GeneroPeliculas_Peliculas_PeliculaId",
                table: "GeneroPeliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GeneroPeliculas",
                table: "GeneroPeliculas");

            migrationBuilder.RenameTable(
                name: "GeneroPeliculas",
                newName: "GenerosPeliculas");

            migrationBuilder.RenameIndex(
                name: "IX_GeneroPeliculas_PeliculaId",
                table: "GenerosPeliculas",
                newName: "IX_GenerosPeliculas_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Generos_GeneroId",
                table: "GenerosPeliculas");

            migrationBuilder.DropForeignKey(
                name: "FK_GenerosPeliculas_Peliculas_PeliculaId",
                table: "GenerosPeliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GenerosPeliculas",
                table: "GenerosPeliculas");

            migrationBuilder.RenameTable(
                name: "GenerosPeliculas",
                newName: "GeneroPeliculas");

            migrationBuilder.RenameIndex(
                name: "IX_GenerosPeliculas_PeliculaId",
                table: "GeneroPeliculas",
                newName: "IX_GeneroPeliculas_PeliculaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GeneroPeliculas",
                table: "GeneroPeliculas",
                columns: new[] { "GeneroId", "PeliculaId" });

            migrationBuilder.AddForeignKey(
                name: "FK_GeneroPeliculas_Generos_GeneroId",
                table: "GeneroPeliculas",
                column: "GeneroId",
                principalTable: "Generos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GeneroPeliculas_Peliculas_PeliculaId",
                table: "GeneroPeliculas",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
