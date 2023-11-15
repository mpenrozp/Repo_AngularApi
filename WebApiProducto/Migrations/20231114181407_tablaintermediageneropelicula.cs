using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApiProducto.Migrations
{
    /// <inheritdoc />
    public partial class tablaintermediageneropelicula : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.CreateTable(
                name: "GeneroPeliculas",
                columns: table => new
                {
                    PeliculaId = table.Column<int>(type: "int", nullable: false),
                    GeneroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneroPeliculas", x => new { x.GeneroId, x.PeliculaId });
                    table.ForeignKey(
                        name: "FK_GeneroPeliculas_Generos_GeneroId",
                        column: x => x.GeneroId,
                        principalTable: "Generos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GeneroPeliculas_Peliculas_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Peliculas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneroPeliculas_PeliculaId",
                table: "GeneroPeliculas",
                column: "PeliculaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneroPeliculas");

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

            migrationBuilder.InsertData(
                table: "Actores",
                columns: new[] { "Id", "FechaNacimiento", "Fortuna", "Nombre" },
                values: new object[,]
                {
                    { 2, new DateTime(1948, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), 15000m, "Samuel L. Jackson" },
                    { 3, new DateTime(1965, 4, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 18000m, "Robert Downey Jr." }
                });

            migrationBuilder.InsertData(
                table: "Peliculas",
                columns: new[] { "Id", "EnCines", "FechaEstreno", "Titulo" },
                values: new object[,]
                {
                    { 2, false, new DateTime(2019, 4, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), "Avengers Endgame" },
                    { 3, false, new DateTime(2021, 12, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spider-Man: No Way Home" },
                    { 4, false, new DateTime(2022, 10, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spider-Man: Across the Spider-Verse (Part One)" }
                });

            migrationBuilder.InsertData(
                table: "Comentarios",
                columns: new[] { "Id", "Contenido", "PeliculaId", "Recomendar" },
                values: new object[,]
                {
                    { 2, "Muy buena!!!", 2, true },
                    { 3, "Dura dura", 2, true },
                    { 4, "no debieron hacer eso...", 3, false }
                });

            migrationBuilder.InsertData(
                table: "GeneroPelicula",
                columns: new[] { "GenerosId", "PeliculasId" },
                values: new object[,]
                {
                    { 5, 2 },
                    { 5, 3 },
                    { 6, 4 }
                });

            migrationBuilder.InsertData(
                table: "PeliculasActores",
                columns: new[] { "ActorId", "PeliculaId", "Orden", "Personaje" },
                values: new object[,]
                {
                    { 2, 2, 2, "Nick Fury" },
                    { 2, 3, 1, "Nick Fury" },
                    { 3, 2, 1, "Iron Man" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneroPelicula_PeliculasId",
                table: "GeneroPelicula",
                column: "PeliculasId");
        }
    }
}
