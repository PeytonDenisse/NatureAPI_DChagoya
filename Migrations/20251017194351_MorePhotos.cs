using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NatureAPI.Migrations
{
    /// <inheritdoc />
    public partial class MorePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PlaceId", "Url" },
                values: new object[] { 1, "https://cdn.visitmexico.com/sites/default/files/styles/explore_hero_desktop/public/2020-03/Nevado-Toluca.jpg" });

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PlaceId", "Url" },
                values: new object[] { 1, "https://escapadas.mexicodesconocido.com.mx/wp-content/uploads/2020/10/nevado-de-toluca-foto-fernando-acosta.jpg" });

            migrationBuilder.InsertData(
                table: "Photos",
                columns: new[] { "Id", "Description", "PlaceId", "Url" },
                values: new object[,]
                {
                    { 4, null, 1, "https://volcanesdemexico.org/wp-content/uploads/2018/12/clima-nevado-de-toluca.jpg" },
                    { 5, null, 2, "https://cloudfront-us-east-1.images.arcpublishing.com/infobae/6VJHMRXIY5HKDGJ775PGPVH65Q.jpg" },
                    { 6, null, 2, "https://www.mexicodesconocido.com.mx/wp-content/uploads/2017/09/Cascadas-de-Tamul-San-Luis-Potosi_1200.jpg" },
                    { 7, null, 2, "https://www.mexicodesconocido.com.mx/wp-content/uploads/2017/01/Cascada-de-Tamasopo-San-Luis-Potosi_1920p.jpg" },
                    { 8, null, 2, "https://www.civitatis.com/f/mexico/san-luis-potosi/excursion-cascada-tamul-cueva-agua-589x392.jpg" },
                    { 9, null, 3, "https://www.caminoreal.com/storage/app/media/Blog/la-bufadora-baja-california.jpg" },
                    { 10, null, 3, "https://pacifica-ensenada.com/wp-content/uploads/2024/09/1000_F_347652456_i1yUhivVTHJowe9ItdXRBQntdPbc6jNe.jpg" },
                    { 11, null, 3, "https://www.debate.com.mx/__export/1727036211378/sites/debate/img/2024/09/22/la_bufadora.jpg_466078407.jpg" },
                    { 12, null, 3, "https://mediaim.expedia.com/destination/2/9a586ef50a9488138b6a62269172663b.jpg" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "PlaceId", "Url" },
                values: new object[] { 2, "https://cloudfront-us-east-1.images.arcpublishing.com/infobae/6VJHMRXIY5HKDGJ775PGPVH65Q.jpg" });

            migrationBuilder.UpdateData(
                table: "Photos",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "PlaceId", "Url" },
                values: new object[] { 3, "https://www.caminoreal.com/storage/app/media/Blog/la-bufadora-baja-california.jpg" });
        }
    }
}
