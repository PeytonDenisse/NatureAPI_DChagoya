using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NatureAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePhotoSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "https://www.mexicodesconocido.com.mx/wp-content/uploads/2016/12/nevado-de-toluca-parque-1600.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                column: "Url",
                value: "https://cloudfront-us-east-1.images.arcpublishing.com/infobae/6VJHMRXIY5HKDGJ775PGPVH65Q.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://www.caminoreal.com/storage/app/media/Blog/la-bufadora-baja-california.jpg");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 1,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/6/69/Nevado_de_Toluca_crater_lakes.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/8/85/Cascada_de_Tamul_SLP.jpg");

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3,
                column: "Url",
                value: "https://upload.wikimedia.org/wikipedia/commons/7/70/La_Bufadora_Blowhole.jpg");
        }
    }
}
