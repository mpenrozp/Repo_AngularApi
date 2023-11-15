using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiProducto.Migrations
{
    /// <inheritdoc />
    public partial class relacionpeliculasygeneros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Pelicula_PeliculaId",
                table: "Comentarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pelicula",
                table: "Pelicula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actor",
                table: "Actor");

            migrationBuilder.RenameTable(
                name: "Pelicula",
                newName: "Peliculas");

            migrationBuilder.RenameTable(
                name: "Actor",
                newName: "Actores");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Peliculas",
                table: "Peliculas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actores",
                table: "Actores",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GeneroPelicula",
                columns: table => new
                {
                    GenerosId = table.Column<int>(type: "int", nullable: false),
                    PeliculasId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroPelicula", x => new { x.GenerosId, x.PeliculasId });
                    table.ForeignKey(
                        name: "FK_GeneroPelicula_Generos_GenerosId",
                        column: x => x.GenerosId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneroPelicula_Peliculas_PeliculasId",
                        column: x => x.PeliculasId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneroPelicula_PeliculasId",
                table: "GeneroPelicula",
                column: "PeliculasId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaId",
                table: "Comentarios",
                column: "PeliculaId",
                principalTable: "Peliculas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_Peliculas_PeliculaId",
                table: "Comentarios");

            migrationBuilder.DropTable(
                name: "GeneroPelicula");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Peliculas",
                table: "Peliculas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actores",
                table: "Actores");

            migrationBuilder.RenameTable(
                name: "Peliculas",
                newName: "Pelicula");

            migrationBuilder.RenameTable(
                name: "Actores",
                newName: "Actor");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pelicula",
                table: "Pelicula",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actor",
                table: "Actor",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_Pelicula_PeliculaId",
                table: "Comentarios",
                column: "PeliculaId",
                principalTable: "Pelicula",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
