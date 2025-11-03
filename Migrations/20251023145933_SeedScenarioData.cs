using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstAidAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedScenarioData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScenarioStep",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ScenarioId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    StepType = table.Column<string>(type: "text", nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Question = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    VideoUrl = table.Column<string>(type: "text", nullable: true),
                    TimeLimit = table.Column<int>(type: "integer", nullable: false),
                    MaxScore = table.Column<int>(type: "integer", nullable: false),
                    TechniqueId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioStep", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScenarioStep_Scenarios_ScenarioId",
                        column: x => x.ScenarioId,
                        principalTable: "Scenarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScenarioStep_Techniques_TechniqueId",
                        column: x => x.TechniqueId,
                        principalTable: "Techniques",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "StepOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    StepId = table.Column<int>(type: "integer", nullable: false),
                    OptionKey = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    ScoreValue = table.Column<int>(type: "integer", nullable: false),
                    FeedbackCorrect = table.Column<string>(type: "text", nullable: true),
                    FeedbackIncorrect = table.Column<string>(type: "text", nullable: true),
                    Explanation = table.Column<string>(type: "text", nullable: true),
                    NextStepId = table.Column<int>(type: "integer", nullable: true),
                    EndScenario = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StepOption", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StepOption_ScenarioStep_StepId",
                        column: x => x.StepId,
                        principalTable: "ScenarioStep",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Scenarios",
                columns: new[] { "Id", "Description", "Difficulty", "Duration", "Icon", "IsPublished", "Name", "PassingScore", "Title", "Type" },
                values: new object[,]
                {
                    { 1, "Xử lý tình huống một người bị ngã xe đạp với vết thương nhẹ. Scenario này phù hợp cho người mới học.", "Dễ", 10, "🚴", true, "bicycle-accident", 70, "Tai nạn xe đạp tại công viên", "outdoor" },
                    { 2, "Xử lý tình huống bị bỏng nhẹ khi đang nấu ăn trong bếp. Học cách sơ cứu bỏng cấp độ 1-2.", "Dễ", 8, "🔥", true, "kitchen-burn", 70, "Bỏng nhẹ khi nấu ăn", "home" }
                });

            migrationBuilder.InsertData(
                table: "ScenarioStep",
                columns: new[] { "Id", "Description", "ImageUrl", "MaxScore", "Order", "Question", "ScenarioId", "StepType", "TechniqueId", "TimeLimit", "Title", "VideoUrl" },
                values: new object[,]
                {
                    { 1, "Bạn đang đi dạo ở công viên và nhìn thấy một người đi xe đạp bị mất thăng bằng và ngã. Người đó đang nằm trên đường, tỉnh táo nhưng trông có vẻ đau đớn. Bạn nhận thấy đầu gối của họ đang chảy máu.", "/images/steps/bicycle-fall.jpg", 0, 1, "", 1, "information", null, 0, "Tình huống ban đầu", "" },
                    { 2, "Bạn đến gần nạn nhân. Điều đầu tiên bạn cần làm là gì?", "", 10, 2, "Hành động đầu tiên bạn nên thực hiện là gì?", 1, "question", null, 30, "Đánh giá ban đầu", "" },
                    { 3, "Hiện trường đã an toàn. Bạn tiếp cận nạn nhân. Người đó tỉnh táo, đang ngồi dậy và nói 'Đầu gối tôi đau quá'.", "", 10, 3, "Bước tiếp theo bạn nên làm gì?", 1, "question", null, 30, "Kiểm tra tình trạng nạn nhân", "" },
                    { 4, "Bạn đã đeo găng tay. Vết thương ở đầu gối đang chảy máu nhưng không quá nhiều. Có một số vết trầy xước và bụi bẩn.", "/images/steps/knee-wound.jpg", 15, 4, "Cách xử lý vết thương đúng là gì?", 1, "question", null, 40, "Xử lý vết thương", "" },
                    { 5, "Vết thương đã được làm sạch. Máu vẫn còn chảy một chút nhưng đã giảm đáng kể.", "", 15, 5, "Bạn nên băng bó vết thương như thế nào?", 1, "question", null, 30, "Băng bó vết thương", "" },
                    { 6, "Bạn đang nấu ăn trong bếp. Trong lúc vội vàng, bạn vô tình chạm tay vào nồi nóng. Bạn cảm thấy đau rát ở mu bàn tay. Khi nhìn xuống, bạn thấy vùng da bị bỏng đỏ lên và hơi sưng, khoảng bằng đồng xu 500 đồng.", "/images/steps/hand-burn.jpg", 0, 1, "", 2, "information", null, 0, "Tình huống", "" },
                    { 7, "Bạn vừa bị bỏng. Tay đang rất đau và rát.", "", 15, 2, "Điều đầu tiên bạn cần làm ngay là gì?", 2, "question", null, 20, "Xử lý ngay lập tức", "" },
                    { 8, "Bạn đang làm mát vết bỏng dưới vòi nước.", "", 10, 3, "Bạn nên làm mát vết bỏng trong bao lâu?", 2, "question", null, 30, "Làm mát vết bỏng", "" },
                    { 9, "Sau khi làm mát 10-15 phút, vết bỏng đã bớt đau. Da vẫn đỏ nhưng không có phồng rộp.", "", 15, 4, "Bước tiếp theo bạn nên làm gì?", 2, "question", null, 30, "Xử lý sau khi làm mát", "" },
                    { 10, "Vết bỏng đã được xử lý và băng bó. Tuy nhiên, bạn cần biết khi nào cần đến bác sĩ.", "", 10, 5, "Trong trường hợp nào bạn NÊN đi khám bác sĩ?", 2, "question", null, 40, "Đánh giá mức độ nghiêm trọng", "" }
                });

            migrationBuilder.InsertData(
                table: "ScenarioTechnique",
                columns: new[] { "ScenarioId", "TechniqueId", "Order" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 1, 4, 2 },
                    { 1, 16, 3 },
                    { 2, 2, 1 }
                });

            migrationBuilder.InsertData(
                table: "StepOption",
                columns: new[] { "Id", "EndScenario", "Explanation", "FeedbackCorrect", "FeedbackIncorrect", "ImageUrl", "IsCorrect", "NextStepId", "OptionKey", "ScoreValue", "StepId", "Text" },
                values: new object[,]
                {
                    { 1, false, "Di chuyển nạn nhân ngay lập tức có thể gây thêm chấn thương nếu có gãy xương hoặc chấn thương nghiêm trọng khác.", "", "Không nên vội vàng. Cần đảm bảo an toàn trước.", "", false, null, "A", 0, 2, "Chạy ngay đến bên nạn nhân và đỡ họ đứng dậy" },
                    { 2, false, "Trong mọi tình huống sơ cứu, đảm bảo an toàn hiện trường giúp tránh tai nạn thứ cấp cho cả bạn và nạn nhân.", "Chính xác! An toàn của bạn và nạn nhân là ưu tiên hàng đầu.", "", "", true, 3, "B", 10, 2, "Đảm bảo an toàn hiện trường (kiểm tra xe cộ xung quanh)" },
                    { 3, false, "Với tai nạn xe đạp nhẹ, nên đánh giá tình huống trước khi gọi cấp cứu. Gọi 115 khi thực sự cần thiết.", "", "Chưa cần thiết. Hãy đánh giá tình huống trước.", "", false, null, "C", 5, 2, "Gọi 115 ngay lập tức" },
                    { 4, false, "Sơ cứu nạn nhân luôn là ưu tiên số một. Chụp ảnh có thể làm sau hoặc nhờ người khác.", "", "Ưu tiên là sơ cứu, không phải chụp ảnh.", "", false, null, "D", 0, 2, "Chụp ảnh để làm bằng chứng" },
                    { 5, false, "Trước khi xử lý vết thương, bạn cần đeo găng tay để bảo vệ bản thân và tránh nhiễm khuẩn.", "", "Chưa đúng thứ tự. Cần chuẩn bị trước.", "", false, null, "A", 0, 3, "Rửa vết thương ngay bằng nước sạch" },
                    { 6, false, "Đeo găng tay giúp tránh tiếp xúc với máu và dịch cơ thể, bảo vệ cả bạn và nạn nhân khỏi nguy cơ nhiễm trùng.", "Đúng rồi! Bảo vệ bản thân trước khi sơ cứu.", "", "", true, 4, "B", 10, 3, "Đeo găng tay và chuẩn bị dụng cụ sơ cứu" },
                    { 7, false, "Hỏi thông tin là tốt nhưng nên xử lý vết thương trước để tránh nhiễm trùng.", "", "Có thể hỏi nhưng không phải ưu tiên.", "", false, null, "C", 3, 3, "Hỏi thông tin cá nhân của nạn nhân" },
                    { 8, false, "Dùng bông khô không loại bỏ được bụi bẩn và vi khuẩn. Cần rửa sạch vết thương bằng nước.", "", "Không nên dùng bông khô. Cần làm sạch trước.", "", false, null, "A", 0, 4, "Dùng bông gòn khô để lau máu" },
                    { 9, false, "Rửa bằng nước sạch loại bỏ bụi bẩn và vi khuẩn, giảm nguy cơ nhiễm trùng. Dùng gạc sạch thấm khô tránh làm tổn thương thêm.", "Xuất sắc! Đây là cách xử lý vết thương đúng chuẩn.", "", "", true, 5, "B", 15, 4, "Rửa nhẹ nhàng bằng nước sạch, sau đó dùng gạc sạch thấm khô" },
                    { 10, false, "Bôi thuốc lên vết thương bẩn có thể nhốt vi khuẩn bên trong, gây nhiễm trùng.", "", "Cần làm sạch vết thương trước khi bôi thuốc.", "", false, null, "C", 0, 4, "Thoa thuốc đỏ ngay lên vết thương" },
                    { 11, false, "Hơi thở chứa rất nhiều vi khuẩn. Thổi vào vết thương là cách dễ gây nhiễm trùng nhất.", "", "Tuyệt đối không! Thổi sẽ đưa vi khuẩn vào vết thương.", "", false, null, "D", 0, 4, "Thổi vào vết thương để làm sạch" },
                    { 12, false, "Dán băng keo trực tiếp sẽ dính vào vết thương, gây đau và tổn thương khi gỡ ra.", "", "Không tốt. Cần đệm gạc trước.", "", false, null, "A", 0, 5, "Dán băng keo trực tiếp lên vết thương" },
                    { 13, true, "Gạc sạch bảo vệ vết thương khỏi nhiễm khuẩn. Băng vừa đủ chặt giúp cầm máu mà không cản trở tuần hoàn.", "Hoàn hảo! Bạn đã xử lý vết thương rất chuyên nghiệp.", "", "", true, null, "B", 15, 5, "Đặt gạc sạch lên vết thương, sau đó dùng băng cuộn cố định (không quá chặt)" },
                    { 14, false, "Băng quá chặt có thể cản trở lưu thông máu, gây tê và tổn thương mô. Băng vừa đủ là được.", "", "Băng quá chặt có thể gây nguy hiểm.", "", false, null, "C", 5, 5, "Băng rất chặt để cầm máu nhanh" },
                    { 15, false, "Để vết thương hở dễ bị nhiễm khuẩn từ môi trường. Luôn băng bó để bảo vệ vết thương.", "", "Không an toàn. Vết thương cần được che phủ.", "", false, null, "D", 0, 5, "Để vết thương hở thoáng khí" },
                    { 16, false, "Đá lạnh quá có thể làm tổn thương mô da đang bị bỏng. Nên dùng nước mát (15-20°C).", "", "Không nên dùng đá trực tiếp! Có thể gây tổn thương thêm.", "", false, null, "A", 0, 7, "Chạy ngay đến tủ lạnh lấy đá chườm" },
                    { 17, false, "Làm mát vết bỏng ngay lập tức giúp giảm đau, giảm sưng và hạn chế tổn thương sâu hơn vào da.", "Chính xác! Đây là cách xử lý bỏng đúng nhất.", "", "", true, 8, "B", 15, 7, "Ngâm vết bỏng vào nước mát (không quá lạnh) hoặc để dưới vòi nước chảy" },
                    { 18, false, "Kem đánh răng, dầu ăn, bơ... không phải là thuốc trị bỏng và có thể gây nhiễm trùng nghiêm trọng.", "", "Không đúng! Kem đánh răng có thể gây nhiễm trùng.", "", false, null, "C", 0, 7, "Bôi kem đánh răng lên vết bỏng" },
                    { 19, false, "Băng bó ngay sẽ giữ nhiệt bên trong, làm vết bỏng nghiêm trọng hơn. Luôn làm mát trước.", "", "Chưa đúng. Cần làm mát trước khi băng bó.", "", false, null, "D", 0, 7, "Băng bó ngay lập tức" },
                    { 20, false, "2-3 phút quá ngắn để giảm nhiệt độ ở sâu bên trong da.", "", "Chưa đủ thời gian. Cần làm mát lâu hơn.", "", false, null, "A", 3, 8, "2-3 phút là đủ" },
                    { 21, false, "10-15 phút là thời gian đủ để làm mát vết bỏng hiệu quả, giảm tổn thương và đau.", "Đúng rồi! Đây là thời gian khuyến cáo.", "", "", true, 9, "B", 10, 8, "10-15 phút" },
                    { 22, false, "Với bỏng nhẹ, 10-15 phút là đủ. Quá lâu có thể gây hạ thân nhiệt.", "", "Quá lâu và không cần thiết với bỏng nhẹ.", "", false, null, "C", 5, 8, "30 phút đến 1 giờ" },
                    { 23, false, "Phồng rộp là lớp bảo vệ tự nhiên. Chọc vỡ sẽ làm tăng nguy cơ nhiễm trùng.", "", "Tuyệt đối không! Có thể gây nhiễm trùng.", "", false, null, "A", 0, 9, "Chọc vỡ phồng rộp (nếu có) để dẫn lưu" },
                    { 24, false, "Thuốc bỏng hoặc gel lô hội giúp làm dịu và tăng tốc quá trình lành vết thương. Băng bó nhẹ bảo vệ vết bỏng.", "Xuất sắc! Đây là cách xử lý bỏng đúng chuẩn.", "", "", true, 10, "B", 15, 9, "Thoa thuốc bỏng (nếu có) hoặc gel lô hội, sau đó băng bó nhẹ nhàng" },
                    { 25, false, "Dầu, mỡ, bơ sẽ giữ nhiệt bên trong, làm vết bỏng nghiêm trọng hơn và dễ nhiễm trùng.", "", "Sai hoàn toàn! Dầu mỡ giữ nhiệt và gây nhiễm trùng.", "", false, null, "C", 0, 9, "Bôi dầu ăn hoặc mỡ lên vết bỏng" },
                    { 26, false, "Bỏng nhẹ, nhỏ không có phồng rộp thường tự lành sau vài ngày với chăm sóc đúng cách tại nhà.", "", "Trường hợp này có thể tự xử lý tại nhà.", "", false, null, "A", 5, 10, "Vết bỏng chỉ bằng đồng xu, đỏ nhẹ, không phồng rộp" },
                    { 27, true, "Vết bỏng lớn hoặc có nhiều phồng rộp cần được bác sĩ đánh giá và xử lý chuyên nghiệp để tránh biến chứng.", "Đúng rồi! Trường hợp này cần gặp bác sĩ.", "", "", true, null, "B", 10, 10, "Vết bỏng lớn hơn lòng bàn tay hoặc có nhiều phồng rộp" },
                    { 28, true, "Bỏng ở các vùng này có thể gây biến chứng nghiêm trọng về thẩm mỹ và chức năng, cần điều trị chuyên khoa.", "Chính xác! Vùng này rất nhạy cảm, cần bác sĩ.", "", "", true, null, "C", 10, 10, "Vết bỏng ở mặt, tay, chân, hoặc bộ phận sinh dục" },
                    { 29, true, "Vết bỏng lớn, nhiều phồng rộp, hoặc ở vùng nhạy cảm đều cần được bác sĩ khám và điều trị.", "Hoàn hảo! Bạn đã hiểu rõ khi nào cần đến bác sĩ.", "", "", true, null, "D", 10, 10, "Cả B và C đều đúng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioStep_ScenarioId",
                table: "ScenarioStep",
                column: "ScenarioId");

            migrationBuilder.CreateIndex(
                name: "IX_ScenarioStep_TechniqueId",
                table: "ScenarioStep",
                column: "TechniqueId");

            migrationBuilder.CreateIndex(
                name: "IX_StepOption_StepId",
                table: "StepOption",
                column: "StepId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StepOption");

            migrationBuilder.DropTable(
                name: "ScenarioStep");

            migrationBuilder.DeleteData(
                table: "ScenarioTechnique",
                keyColumns: new[] { "ScenarioId", "TechniqueId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "ScenarioTechnique",
                keyColumns: new[] { "ScenarioId", "TechniqueId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "ScenarioTechnique",
                keyColumns: new[] { "ScenarioId", "TechniqueId" },
                keyValues: new object[] { 1, 16 });

            migrationBuilder.DeleteData(
                table: "ScenarioTechnique",
                keyColumns: new[] { "ScenarioId", "TechniqueId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Scenarios",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
