using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de villa...", new DateTime(2023, 6, 20, 17, 53, 16, 174, DateTimeKind.Local).AddTicks(362), new DateTime(2023, 6, 20, 17, 53, 16, 174, DateTimeKind.Local).AddTicks(346), "", 50, "Villa Real", 5, 200.0 },
                    { 2, "", "Detalle de villa...", new DateTime(2023, 6, 20, 17, 53, 16, 174, DateTimeKind.Local).AddTicks(364), new DateTime(2023, 6, 20, 17, 53, 16, 174, DateTimeKind.Local).AddTicks(364), "", 40, "Premium Vista a la Piscina", 4, 150.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
