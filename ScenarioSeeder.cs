using FirstAidAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstAidAPI
{
    public class ScenarioSeeder
    {
        public static void SeedScenarios(ModelBuilder modelBuilder)
        {
            // ============================================
            // SCENARIO 1: Tai nạn xe đạp (Beginner)
            // ============================================

            var scenario1 = new Scenario
            {
                Id = 1,
                Name = "bicycle-accident",
                Title = "Tai nạn xe đạp tại công viên",
                Description = "Xử lý tình huống một người bị ngã xe đạp với vết thương nhẹ. Scenario này phù hợp cho người mới học.",
                Type = "outdoor",
                Difficulty = "Dễ",
                Duration = 10,
                Icon = "🚴",
                PassingScore = 70,
                IsPublished = true
            };

            // Steps cho Scenario 1
            var scenario1Steps = new List<ScenarioStep>
        {
            // Step 1: Information (Giới thiệu tình huống)
            new ScenarioStep
            {
                Id = 1,
                ScenarioId = 1,
                Order = 1,
                StepType = "information",
                Title = "Tình huống ban đầu",
                Description = "Bạn đang đi dạo ở công viên và nhìn thấy một người đi xe đạp bị mất thăng bằng và ngã. Người đó đang nằm trên đường, tỉnh táo nhưng trông có vẻ đau đớn. Bạn nhận thấy đầu gối của họ đang chảy máu.",
                Question = "",
                ImageUrl = "/images/steps/bicycle-fall.jpg",
                VideoUrl = "",
                TimeLimit = 0,
                MaxScore = 0,
                TechniqueId = null
            },

            // Step 2: Question (Đánh giá tình huống)
            new ScenarioStep
            {
                Id = 2,
                ScenarioId = 1,
                Order = 2,
                StepType = "question",
                Title = "Đánh giá ban đầu",
                Description = "Bạn đến gần nạn nhân. Điều đầu tiên bạn cần làm là gì?",
                Question = "Hành động đầu tiên bạn nên thực hiện là gì?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 30,
                MaxScore = 10,
                TechniqueId = null
            },

            // Step 3: Question (Kiểm tra ý thức)
            new ScenarioStep
            {
                Id = 3,
                ScenarioId = 1,
                Order = 3,
                StepType = "question",
                Title = "Kiểm tra tình trạng nạn nhân",
                Description = "Hiện trường đã an toàn. Bạn tiếp cận nạn nhân. Người đó tỉnh táo, đang ngồi dậy và nói 'Đầu gối tôi đau quá'.",
                Question = "Bước tiếp theo bạn nên làm gì?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 30,
                MaxScore = 10,
                TechniqueId = null
            },

            // Step 4: Question (Xử lý vết thương)
            new ScenarioStep
            {
                Id = 4,
                ScenarioId = 1,
                Order = 4,
                StepType = "question",
                Title = "Xử lý vết thương",
                Description = "Bạn đã đeo găng tay. Vết thương ở đầu gối đang chảy máu nhưng không quá nhiều. Có một số vết trầy xước và bụi bẩn.",
                Question = "Cách xử lý vết thương đúng là gì?",
                ImageUrl = "/images/steps/knee-wound.jpg",
                VideoUrl = "",
                TimeLimit = 40,
                MaxScore = 15,
                TechniqueId = null
            },

            // Step 5: Question (Băng bó)
            new ScenarioStep
            {
                Id = 5,
                ScenarioId = 1,
                Order = 5,
                StepType = "question",
                Title = "Băng bó vết thương",
                Description = "Vết thương đã được làm sạch. Máu vẫn còn chảy một chút nhưng đã giảm đáng kể.",
                Question = "Bạn nên băng bó vết thương như thế nào?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 30,
                MaxScore = 15,
                TechniqueId = null
            }
        };

            // Options cho các steps của Scenario 1
            var scenario1Options = new List<StepOption>
        {
            // Options cho Step 2
            new StepOption
            {
                Id = 1,
                StepId = 2,
                OptionKey = "A",
                Text = "Chạy ngay đến bên nạn nhân và đỡ họ đứng dậy",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không nên vội vàng. Cần đảm bảo an toàn trước.",
                Explanation = "Di chuyển nạn nhân ngay lập tức có thể gây thêm chấn thương nếu có gãy xương hoặc chấn thương nghiêm trọng khác.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 2,
                StepId = 2,
                OptionKey = "B",
                Text = "Đảm bảo an toàn hiện trường (kiểm tra xe cộ xung quanh)",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Chính xác! An toàn của bạn và nạn nhân là ưu tiên hàng đầu.",
                FeedbackIncorrect = "",
                Explanation = "Trong mọi tình huống sơ cứu, đảm bảo an toàn hiện trường giúp tránh tai nạn thứ cấp cho cả bạn và nạn nhân.",
                NextStepId = 3,
                EndScenario = false
            },
            new StepOption
            {
                Id = 3,
                StepId = 2,
                OptionKey = "C",
                Text = "Gọi 115 ngay lập tức",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 5,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Chưa cần thiết. Hãy đánh giá tình huống trước.",
                Explanation = "Với tai nạn xe đạp nhẹ, nên đánh giá tình huống trước khi gọi cấp cứu. Gọi 115 khi thực sự cần thiết.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 4,
                StepId = 2,
                OptionKey = "D",
                Text = "Chụp ảnh để làm bằng chứng",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Ưu tiên là sơ cứu, không phải chụp ảnh.",
                Explanation = "Sơ cứu nạn nhân luôn là ưu tiên số một. Chụp ảnh có thể làm sau hoặc nhờ người khác.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 3
            new StepOption
            {
                Id = 5,
                StepId = 3,
                OptionKey = "A",
                Text = "Rửa vết thương ngay bằng nước sạch",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Chưa đúng thứ tự. Cần chuẩn bị trước.",
                Explanation = "Trước khi xử lý vết thương, bạn cần đeo găng tay để bảo vệ bản thân và tránh nhiễm khuẩn.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 6,
                StepId = 3,
                OptionKey = "B",
                Text = "Đeo găng tay và chuẩn bị dụng cụ sơ cứu",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Đúng rồi! Bảo vệ bản thân trước khi sơ cứu.",
                FeedbackIncorrect = "",
                Explanation = "Đeo găng tay giúp tránh tiếp xúc với máu và dịch cơ thể, bảo vệ cả bạn và nạn nhân khỏi nguy cơ nhiễm trùng.",
                NextStepId = 4,
                EndScenario = false
            },
            new StepOption
            {
                Id = 7,
                StepId = 3,
                OptionKey = "C",
                Text = "Hỏi thông tin cá nhân của nạn nhân",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 3,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Có thể hỏi nhưng không phải ưu tiên.",
                Explanation = "Hỏi thông tin là tốt nhưng nên xử lý vết thương trước để tránh nhiễm trùng.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 4
            new StepOption
            {
                Id = 8,
                StepId = 4,
                OptionKey = "A",
                Text = "Dùng bông gòn khô để lau máu",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không nên dùng bông khô. Cần làm sạch trước.",
                Explanation = "Dùng bông khô không loại bỏ được bụi bẩn và vi khuẩn. Cần rửa sạch vết thương bằng nước.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 9,
                StepId = 4,
                OptionKey = "B",
                Text = "Rửa nhẹ nhàng bằng nước sạch, sau đó dùng gạc sạch thấm khô",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 15,
                FeedbackCorrect = "Xuất sắc! Đây là cách xử lý vết thương đúng chuẩn.",
                FeedbackIncorrect = "",
                Explanation = "Rửa bằng nước sạch loại bỏ bụi bẩn và vi khuẩn, giảm nguy cơ nhiễm trùng. Dùng gạc sạch thấm khô tránh làm tổn thương thêm.",
                NextStepId = 5,
                EndScenario = false
            },
            new StepOption
            {
                Id = 10,
                StepId = 4,
                OptionKey = "C",
                Text = "Thoa thuốc đỏ ngay lên vết thương",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Cần làm sạch vết thương trước khi bôi thuốc.",
                Explanation = "Bôi thuốc lên vết thương bẩn có thể nhốt vi khuẩn bên trong, gây nhiễm trùng.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 11,
                StepId = 4,
                OptionKey = "D",
                Text = "Thổi vào vết thương để làm sạch",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Tuyệt đối không! Thổi sẽ đưa vi khuẩn vào vết thương.",
                Explanation = "Hơi thở chứa rất nhiều vi khuẩn. Thổi vào vết thương là cách dễ gây nhiễm trùng nhất.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 5
            new StepOption
            {
                Id = 12,
                StepId = 5,
                OptionKey = "A",
                Text = "Dán băng keo trực tiếp lên vết thương",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không tốt. Cần đệm gạc trước.",
                Explanation = "Dán băng keo trực tiếp sẽ dính vào vết thương, gây đau và tổn thương khi gỡ ra.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 13,
                StepId = 5,
                OptionKey = "B",
                Text = "Đặt gạc sạch lên vết thương, sau đó dùng băng cuộn cố định (không quá chặt)",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 15,
                FeedbackCorrect = "Hoàn hảo! Bạn đã xử lý vết thương rất chuyên nghiệp.",
                FeedbackIncorrect = "",
                Explanation = "Gạc sạch bảo vệ vết thương khỏi nhiễm khuẩn. Băng vừa đủ chặt giúp cầm máu mà không cản trở tuần hoàn.",
                NextStepId = null,
                EndScenario = true
            },
            new StepOption
            {
                Id = 14,
                StepId = 5,
                OptionKey = "C",
                Text = "Băng rất chặt để cầm máu nhanh",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 5,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Băng quá chặt có thể gây nguy hiểm.",
                Explanation = "Băng quá chặt có thể cản trở lưu thông máu, gây tê và tổn thương mô. Băng vừa đủ là được.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 15,
                StepId = 5,
                OptionKey = "D",
                Text = "Để vết thương hở thoáng khí",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không an toàn. Vết thương cần được che phủ.",
                Explanation = "Để vết thương hở dễ bị nhiễm khuẩn từ môi trường. Luôn băng bó để bảo vệ vết thương.",
                NextStepId = null,
                EndScenario = false
            }
        };

            // ============================================
            // SCENARIO 2: Bỏng nhẹ ở bếp (Beginner)
            // ============================================

            var scenario2 = new Scenario
            {
                Id = 2,
                Name = "kitchen-burn",
                Title = "Bỏng nhẹ khi nấu ăn",
                Description = "Xử lý tình huống bị bỏng nhẹ khi đang nấu ăn trong bếp. Học cách sơ cứu bỏng cấp độ 1-2.",
                Type = "home",
                Difficulty = "Dễ",
                Duration = 8,
                Icon = "🔥",
                PassingScore = 70,
                IsPublished = true
            };

            // Steps cho Scenario 2
            var scenario2Steps = new List<ScenarioStep>
        {
            // Step 1: Information
            new ScenarioStep
            {
                Id = 6,
                ScenarioId = 2,
                Order = 1,
                StepType = "information",
                Title = "Tình huống",
                Description = "Bạn đang nấu ăn trong bếp. Trong lúc vội vàng, bạn vô tình chạm tay vào nồi nóng. Bạn cảm thấy đau rát ở mu bàn tay. Khi nhìn xuống, bạn thấy vùng da bị bỏng đỏ lên và hơi sưng, khoảng bằng đồng xu 500 đồng.",
                Question = "",
                ImageUrl = "/images/steps/hand-burn.jpg",
                VideoUrl = "",
                TimeLimit = 0,
                MaxScore = 0,
                TechniqueId = null
            },

            // Step 2: Question
            new ScenarioStep
            {
                Id = 7,
                ScenarioId = 2,
                Order = 2,
                StepType = "question",
                Title = "Xử lý ngay lập tức",
                Description = "Bạn vừa bị bỏng. Tay đang rất đau và rát.",
                Question = "Điều đầu tiên bạn cần làm ngay là gì?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 20,
                MaxScore = 15,
                TechniqueId = null
            },

            // Step 3: Question
            new ScenarioStep
            {
                Id = 8,
                ScenarioId = 2,
                Order = 3,
                StepType = "question",
                Title = "Làm mát vết bỏng",
                Description = "Bạn đang làm mát vết bỏng dưới vòi nước.",
                Question = "Bạn nên làm mát vết bỏng trong bao lâu?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 30,
                MaxScore = 10,
                TechniqueId = null
            },

            // Step 4: Question
            new ScenarioStep
            {
                Id = 9,
                ScenarioId = 2,
                Order = 4,
                StepType = "question",
                Title = "Xử lý sau khi làm mát",
                Description = "Sau khi làm mát 10-15 phút, vết bỏng đã bớt đau. Da vẫn đỏ nhưng không có phồng rộp.",
                Question = "Bước tiếp theo bạn nên làm gì?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 30,
                MaxScore = 15,
                TechniqueId = null
            },

            // Step 5: Question
            new ScenarioStep
            {
                Id = 10,
                ScenarioId = 2,
                Order = 5,
                StepType = "question",
                Title = "Đánh giá mức độ nghiêm trọng",
                Description = "Vết bỏng đã được xử lý và băng bó. Tuy nhiên, bạn cần biết khi nào cần đến bác sĩ.",
                Question = "Trong trường hợp nào bạn NÊN đi khám bác sĩ?",
                ImageUrl = "",
                VideoUrl = "",
                TimeLimit = 40,
                MaxScore = 10,
                TechniqueId = null
            }
        };

            // Options cho Scenario 2
            var scenario2Options = new List<StepOption>
        {
            // Options cho Step 7 (Scenario 2, Step 2)
            new StepOption
            {
                Id = 16,
                StepId = 7,
                OptionKey = "A",
                Text = "Chạy ngay đến tủ lạnh lấy đá chườm",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không nên dùng đá trực tiếp! Có thể gây tổn thương thêm.",
                Explanation = "Đá lạnh quá có thể làm tổn thương mô da đang bị bỏng. Nên dùng nước mát (15-20°C).",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 17,
                StepId = 7,
                OptionKey = "B",
                Text = "Ngâm vết bỏng vào nước mát (không quá lạnh) hoặc để dưới vòi nước chảy",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 15,
                FeedbackCorrect = "Chính xác! Đây là cách xử lý bỏng đúng nhất.",
                FeedbackIncorrect = "",
                Explanation = "Làm mát vết bỏng ngay lập tức giúp giảm đau, giảm sưng và hạn chế tổn thương sâu hơn vào da.",
                NextStepId = 8,
                EndScenario = false
            },
            new StepOption
            {
                Id = 18,
                StepId = 7,
                OptionKey = "C",
                Text = "Bôi kem đánh răng lên vết bỏng",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Không đúng! Kem đánh răng có thể gây nhiễm trùng.",
                Explanation = "Kem đánh răng, dầu ăn, bơ... không phải là thuốc trị bỏng và có thể gây nhiễm trùng nghiêm trọng.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 19,
                StepId = 7,
                OptionKey = "D",
                Text = "Băng bó ngay lập tức",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Chưa đúng. Cần làm mát trước khi băng bó.",
                Explanation = "Băng bó ngay sẽ giữ nhiệt bên trong, làm vết bỏng nghiêm trọng hơn. Luôn làm mát trước.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 8
            new StepOption
            {
                Id = 20,
                StepId = 8,
                OptionKey = "A",
                Text = "2-3 phút là đủ",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 3,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Chưa đủ thời gian. Cần làm mát lâu hơn.",
                Explanation = "2-3 phút quá ngắn để giảm nhiệt độ ở sâu bên trong da.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 21,
                StepId = 8,
                OptionKey = "B",
                Text = "10-15 phút",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Đúng rồi! Đây là thời gian khuyến cáo.",
                FeedbackIncorrect = "",
                Explanation = "10-15 phút là thời gian đủ để làm mát vết bỏng hiệu quả, giảm tổn thương và đau.",
                NextStepId = 9,
                EndScenario = false
            },
            new StepOption
            {
                Id = 22,
                StepId = 8,
                OptionKey = "C",
                Text = "30 phút đến 1 giờ",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 5,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Quá lâu và không cần thiết với bỏng nhẹ.",
                Explanation = "Với bỏng nhẹ, 10-15 phút là đủ. Quá lâu có thể gây hạ thân nhiệt.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 9
            new StepOption
            {
                Id = 23,
                StepId = 9,
                OptionKey = "A",
                Text = "Chọc vỡ phồng rộp (nếu có) để dẫn lưu",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Tuyệt đối không! Có thể gây nhiễm trùng.",
                Explanation = "Phồng rộp là lớp bảo vệ tự nhiên. Chọc vỡ sẽ làm tăng nguy cơ nhiễm trùng.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 24,
                StepId = 9,
                OptionKey = "B",
                Text = "Thoa thuốc bỏng (nếu có) hoặc gel lô hội, sau đó băng bó nhẹ nhàng",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 15,
                FeedbackCorrect = "Xuất sắc! Đây là cách xử lý bỏng đúng chuẩn.",
                FeedbackIncorrect = "",
                Explanation = "Thuốc bỏng hoặc gel lô hội giúp làm dịu và tăng tốc quá trình lành vết thương. Băng bó nhẹ bảo vệ vết bỏng.",
                NextStepId = 10,
                EndScenario = false
            },
            new StepOption
            {
                Id = 25,
                StepId = 9,
                OptionKey = "C",
                Text = "Bôi dầu ăn hoặc mỡ lên vết bỏng",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 0,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Sai hoàn toàn! Dầu mỡ giữ nhiệt và gây nhiễm trùng.",
                Explanation = "Dầu, mỡ, bơ sẽ giữ nhiệt bên trong, làm vết bỏng nghiêm trọng hơn và dễ nhiễm trùng.",
                NextStepId = null,
                EndScenario = false
            },

            // Options cho Step 10
            new StepOption
            {
                Id = 26,
                StepId = 10,
                OptionKey = "A",
                Text = "Vết bỏng chỉ bằng đồng xu, đỏ nhẹ, không phồng rộp",
                ImageUrl = "",
                IsCorrect = false,
                ScoreValue = 5,
                FeedbackCorrect = "",
                FeedbackIncorrect = "Trường hợp này có thể tự xử lý tại nhà.",
                Explanation = "Bỏng nhẹ, nhỏ không có phồng rộp thường tự lành sau vài ngày với chăm sóc đúng cách tại nhà.",
                NextStepId = null,
                EndScenario = false
            },
            new StepOption
            {
                Id = 27,
                StepId = 10,
                OptionKey = "B",
                Text = "Vết bỏng lớn hơn lòng bàn tay hoặc có nhiều phồng rộp",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Đúng rồi! Trường hợp này cần gặp bác sĩ.",
                FeedbackIncorrect = "",
                Explanation = "Vết bỏng lớn hoặc có nhiều phồng rộp cần được bác sĩ đánh giá và xử lý chuyên nghiệp để tránh biến chứng.",
                NextStepId = null,
                EndScenario = true
            },
            new StepOption
            {
                Id = 28,
                StepId = 10,
                OptionKey = "C",
                Text = "Vết bỏng ở mặt, tay, chân, hoặc bộ phận sinh dục",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Chính xác! Vùng này rất nhạy cảm, cần bác sĩ.",
                FeedbackIncorrect = "",
                Explanation = "Bỏng ở các vùng này có thể gây biến chứng nghiêm trọng về thẩm mỹ và chức năng, cần điều trị chuyên khoa.",
                NextStepId = null,
                EndScenario = true
            },
            new StepOption
            {
                Id = 29,
                StepId = 10,
                OptionKey = "D",
                Text = "Cả B và C đều đúng",
                ImageUrl = "",
                IsCorrect = true,
                ScoreValue = 10,
                FeedbackCorrect = "Hoàn hảo! Bạn đã hiểu rõ khi nào cần đến bác sĩ.",
                FeedbackIncorrect = "",
                Explanation = "Vết bỏng lớn, nhiều phồng rộp, hoặc ở vùng nhạy cảm đều cần được bác sĩ khám và điều trị.",
                NextStepId = null,
                EndScenario = true
            }
        };

            // ============================================
            // TECHNIQUE RELATIONSHIPS
            // ============================================

            var scenarioTechniques = new List<ScenarioTechnique>
        {
            // Scenario 1 liên quan đến các Techniques
            new ScenarioTechnique { ScenarioId = 1, TechniqueId = 1, Order = 1 }, // Đánh giá hiện trường
            new ScenarioTechnique { ScenarioId = 1, TechniqueId = 4, Order = 2 }, // Xử lý vết thương
            new ScenarioTechnique { ScenarioId = 1, TechniqueId = 16, Order = 3 }, // Băng bó cơ bản

            // Scenario 2 liên quan đến các Techniques
            new ScenarioTechnique { ScenarioId = 2, TechniqueId = 2, Order = 1 }, // Xử lý bỏng
        };

            // ============================================
            // SEED DATA
            // ============================================

            modelBuilder.Entity<Scenario>().HasData(scenario1, scenario2);
            modelBuilder.Entity<ScenarioStep>().HasData(scenario1Steps);
            modelBuilder.Entity<ScenarioStep>().HasData(scenario2Steps);
            modelBuilder.Entity<StepOption>().HasData(scenario1Options);
            modelBuilder.Entity<StepOption>().HasData(scenario2Options);

            // Chỉ seed nếu bạn đã có Techniques với Id 1-5
            // Nếu chưa có, comment dòng này lại
            modelBuilder.Entity<ScenarioTechnique>().HasData(scenarioTechniques);
        }
    }
}