using FirstAidAPI.Extensions;
using FirstAidAPI.Models;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;

namespace FirstAidAPI.Data

{
    public class FirstAidContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public FirstAidContext(DbContextOptions<FirstAidContext> options)
            : base(options)
        {
        }

        public DbSet<Technique> Techniques { get; set; }
        public DbSet<TechniqueStep> TechniqueSteps { get; set; }
        public DbSet<Scenario> Scenarios { get; set; }
        public DbSet<QuizQuestion> QuizQuestions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }
        public DbSet<ScenarioStep> ScenarioSteps { get; set; }
        public DbSet<StepOption> StepOptions { get; set; }
        public DbSet<UserScenarioProgress> UserScenarioProgresses { get; set; }
        public DbSet<UserTechniqueProgress> UserTechniqueProgresses { get; set; }
        public DbSet<SavedTechnique> SavedTechniques { get; set; }
        public DbSet<ScenarioAttempt> ScenarioAttempts { get; set; }
        public DbSet<StepAnswer> StepAnswers { get; set; }
        public DbSet<UserAchievement> UserAchievements { get; set; }
        public DbSet<TechniqueType> TechniqueTypes { get; set; }
        public DbSet<PracticalCourse> PracticalCourses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Nurse> Nurses { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Receptionist> Receptionists { get; set; }
        public DbSet<MedicalRecord> MedicalRecords { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Speciality> Specialties { get; set; }
        public DbSet<VitalSign> VitalSigns { get; set; }
        public DbSet<Ward> Wards { get; set; }
        public DbSet<Bed> Beds { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<MedicalService> MedicalServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<DoctorSchedule> DoctorSchedules { get; set; }
        public DbSet<NurseSchedule> NurseSchedules { get; set; }
        public DbSet<LabOrder> LabOrders { get; set; }
        public DbSet<LabOrderItem> LabOrderItems { get; set; }
        public DbSet<AdmissionRecord> AdmissionRecords { get; set; }
        public DbSet<WardOrder> WardOrders { get; set; }
        public DbSet<WardNote> WardNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Đổi tên các bảng của Identity
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable(name: "Users"); // Đổi AspNetUsers -> Users
            });

            modelBuilder.Entity<IdentityRole<int>>(entity =>
            {
                entity.ToTable(name: "Roles"); // Đổi AspNetRoles -> Roles
            });

            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.ToTable("UserRoles"); // Đổi AspNetUserRoles -> UserRoles
            });

            modelBuilder.Entity<IdentityUserClaim<int>>(entity =>
            {
                entity.ToTable("UserClaims"); // Đổi AspNetUserClaims -> UserClaims
            });

            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.ToTable("UserLogins"); // Đổi AspNetUserLogins -> UserLogins
            });

            modelBuilder.Entity<IdentityRoleClaim<int>>(entity =>
            {
                entity.ToTable("RoleClaims"); // Đổi AspNetRoleClaims -> RoleClaims
            });

            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.ToTable("UserTokens"); // Đổi AspNetUserTokens -> UserTokens
            });

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");
            });
            modelBuilder.Entity<VitalSign>(entity =>
            {
                entity.ToTable("VitalSigns");
            });
            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.ToTable("Invoices");
            });
            modelBuilder.Entity<MedicalRecord>(entity =>
            {
                entity.ToTable("MedicalRecords");
            });
            modelBuilder.Entity<Bed>(entity =>
            {
                entity.ToTable("Beds");
            });
            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.ToTable("Specialties");
            });
            modelBuilder.Entity<Doctor>(entity =>
            {
                entity.ToTable("Doctors");
            });
            modelBuilder.Entity<Receptionist>(entity =>
            {
                entity.ToTable("Receptionists");
            });
            modelBuilder.Entity<Nurse>(entity =>
            {
                entity.ToTable("Nurses");
            });
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.ToTable("Patients");
            });
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payments");
            });

            modelBuilder.Entity<QuizQuestion>()
                .HasOne(q => q.Technique)
                .WithMany(t => t.QuizQuestions)
                .HasForeignKey(q => q.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Technique>()
                .HasMany(t => t.TechniqueSteps)
                .WithOne(ts => ts.Technique)
                .HasForeignKey(s => s.TechniqueId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Technique>()
                .HasOne(t => t.Type)
                .WithMany(tt => tt.Techniques)
                .HasForeignKey(t => t.TechniqueTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QuizQuestion>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(a => a.QuizQuestion)
                .HasForeignKey(a => a.QuizQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Scenario>()
                .HasMany(s => s.ScenarioSteps)
                .WithOne(ss => ss.Scenario)
                .HasForeignKey(ss => ss.ScenarioId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ScenarioStep>()
                .HasMany(ss => ss.Options)
                .WithOne(so => so.Step)
                .HasForeignKey(so => so.StepId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Speciality>()
                .HasOne(s => s.HeadDoctor)
                .WithOne(d => d.HeadOfSpeciality)
                .HasForeignKey<Speciality>(s => s.HeadDoctorId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Speciality>()
                .HasOne(s => s.HeadNurse)
                .WithOne(n => n.HeadOfSpeciality)
                .HasForeignKey<Speciality>(s => s.HeadNurseId)
                .OnDelete(DeleteBehavior.SetNull);

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.FullName)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(u => u.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(u => u.IsActive)
                    .HasDefaultValue(true);

                entity.Property(u => u.Avatar)
                    .HasMaxLength(500);

                entity.Property(u => u.DateOfBirth);
            });
            //Patient configuration
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.User)
                .WithOne(u => u.Patient)
                .HasForeignKey<Patient>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Nurse configuration
            modelBuilder.Entity<Nurse>()
                .HasOne(n => n.User)
                .WithOne(u => u.Nurse)
                .HasForeignKey<Nurse>(n => n.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Doctor configuration
            modelBuilder.Entity<Doctor>()
                .HasOne(d => d.User)
                .WithOne(u => u.Doctor)
                .HasForeignKey<Doctor>(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            // Receptionist configuration
            modelBuilder.Entity<Receptionist>()
                .HasOne(r => r.User)
                .WithOne(u => u.Receptionist)
                .HasForeignKey<Receptionist>(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One -to-One relationships for Appointment
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.MedicalRecord)
                .WithOne(mr => mr.Appointment)
                .HasForeignKey<MedicalRecord>(mr => mr.AppointmentId);

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Invoice)
                .WithOne(i => i.Appointment)
                .HasForeignKey<Invoice>(i => i.AppointmentId);

            //Prevent cascade delete
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .OnDelete(DeleteBehavior.Restrict);

            // UserScenarioProgress configuration
            modelBuilder.Entity<UserScenarioProgress>(entity =>
            {
                entity.HasKey(usp => usp.Id);

                // Tạo composite index để tránh duplicate progress records
                entity.HasIndex(usp => new { usp.UserId, usp.ScenarioId })
                    .IsUnique();

                entity.HasOne(usp => usp.User)
                    .WithMany(u => u.ScenarioProgresses)
                    .HasForeignKey(usp => usp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(usp => usp.Scenario)
                    .WithMany()
                    .HasForeignKey(usp => usp.ScenarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(usp => usp.Status)
                    .HasDefaultValue(false);

                entity.Property(usp => usp.HighestScore)
                    .HasDefaultValue(0);

                entity.Property(usp => usp.LastAccessedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // UserTechniqueProgress configuration
            modelBuilder.Entity<UserTechniqueProgress>(entity =>
            {
                entity.HasKey(utp => utp.Id);

                // Tạo composite index để tránh duplicate progress records
                entity.HasIndex(utp => new { utp.UserId, utp.TechniqueId })
                    .IsUnique();

                entity.HasOne(utp => utp.User)
                    .WithMany(u => u.TechniqueProgresses)
                    .HasForeignKey(utp => utp.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(utp => utp.Technique)
                    .WithMany()
                    .HasForeignKey(utp => utp.TechniqueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(utp => utp.Status)
                    .HasDefaultValue(false);

                entity.Property(utp => utp.IsVideoWatched)
                    .HasDefaultValue(false);

                entity.Property(utp => utp.LastAccessedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // SavedTechnique configuration
            modelBuilder.Entity<SavedTechnique>(entity =>
            {
                entity.HasKey(st => st.Id);

                // Tránh user lưu duplicate technique
                entity.HasIndex(st => new { st.UserId, st.TechniqueId })
                    .IsUnique();

                entity.HasOne(st => st.User)
                    .WithMany(u => u.SavedTechniques)
                    .HasForeignKey(st => st.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(st => st.Technique)
                    .WithMany()
                    .HasForeignKey(st => st.TechniqueId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(st => st.SavedAt)
                    .HasDefaultValueSql("NOW()");

                entity.Property(st => st.Priority)
                    .HasDefaultValue(0);
            });

            // ScenarioAttempt configuration
            modelBuilder.Entity<ScenarioAttempt>(entity =>
            {
                entity.HasKey(sa => sa.Id);

                // Index để query nhanh attempts của user
                entity.HasIndex(sa => new { sa.UserId, sa.AttemptedAt });

                entity.HasOne(sa => sa.User)
                    .WithMany(u => u.ScenarioAttempts)
                    .HasForeignKey(sa => sa.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sa => sa.Scenario)
                    .WithMany()
                    .HasForeignKey(sa => sa.ScenarioId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(sa => sa.AttemptedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // StepAnswer configuration
            modelBuilder.Entity<StepAnswer>(entity =>
            {
                entity.HasKey(sa => sa.Id);

                entity.HasOne(sa => sa.ScenarioAttempt)
                    .WithMany(attempt => attempt.StepAnswers)
                    .HasForeignKey(sa => sa.ScenarioAttemptId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(sa => sa.Step)
                    .WithMany()
                    .HasForeignKey(sa => sa.StepId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(sa => sa.AnsweredAt)
                    .HasDefaultValueSql("NOW()");
            });

            // UserAchievement configuration
            modelBuilder.Entity<UserAchievement>(entity =>
            {
                entity.HasKey(ua => ua.Id);

                // Tránh user nhận duplicate achievement
                entity.HasIndex(ua => new { ua.UserId, ua.AchievementType })
                    .IsUnique();

                entity.HasOne(ua => ua.User)
                    .WithMany()
                    .HasForeignKey(ua => ua.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(ua => ua.AchievementType)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(ua => ua.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(ua => ua.EarnedAt)
                    .HasDefaultValueSql("NOW()");
            });

            // Cấu hình PracticalCourse
            modelBuilder.Entity<PracticalCourse>(entity =>
            {
                entity.HasKey(e => e.Id);

                // Đặt precision cho decimal
                entity.Property(e => e.Price)
                    .HasColumnType("decimal(18,2)");

                // Index cho tìm kiếm khóa học đã publish
                entity.HasIndex(e => e.IsPublished);

                // Index cho tìm kiếm theo ngày
                entity.HasIndex(e => e.StartDate);
                entity.Property(e => e.Highlights).HasJsonConversion();
                entity.Property(e => e.Requirements).HasJsonConversion();
            });

            //Cấu hình Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(o => o.Id);

                entity.Property(o => o.OrderNumber)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasIndex(o => o.OrderNumber)
                    .IsUnique(); // Đảm bảo OrderNumber là duy nhất

                entity.Property(o => o.TotalAmount)
                    .HasColumnType("decimal(18,2)"); // Chính xác cho tiền

                entity.Property(o => o.PaymentMethod)
                    .HasConversion<int>(); // Lưu enum dưới dạng int

                entity.Property(o => o.PaymentStatus)
                    .HasConversion<int>();

                entity.Property(o => o.OrderStatus)
                    .HasConversion<int>();

                entity.Property(o => o.CreatedAt)
                    .IsRequired();

                // Relationship: Order - User (Many-to-One)
                entity.HasOne(o => o.User)
                    .WithMany() // Hoặc .WithMany(u => u.Orders) nếu User có ICollection<Order>
                    .HasForeignKey(o => o.UserId)
                    .OnDelete(DeleteBehavior.Restrict); // Không xóa Order khi xóa User
            });

            // Cấu hình OrderItem
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.HasKey(oi => oi.Id);

                entity.Property(oi => oi.Price)
                    .HasColumnType("decimal(18,2)");

                entity.Property(oi => oi.Subtotal)
                    .HasColumnType("decimal(18,2)");

                entity.Property(oi => oi.Quantity)
                    .IsRequired()
                    .HasDefaultValue(1);

                // Relationship: OrderItem - Order (Many-to-One)
                entity.HasOne(oi => oi.Order)
                    .WithMany(o => o.OrderItems)
                    .HasForeignKey(oi => oi.OrderId)
                    .OnDelete(DeleteBehavior.Cascade); // Xóa Order thì xóa hết OrderItems

                // Relationship: OrderItem - PracticalCourse (Many-to-One)
                entity.HasOne(oi => oi.PracticalCourse)
                    .WithMany()
                    .HasForeignKey(oi => oi.PracticalCourseId)
                    .OnDelete(DeleteBehavior.Restrict); // Không xóa Course khi có OrderItem
            });

            // Cấu hình CourseEnrollment
            modelBuilder.Entity<CourseEnrollment>(entity =>
            {
                // Composite index để query nhanh
                entity.HasIndex(e => new { e.UserId, e.PracticalCourseId });

                // Unique constraint: 1 user chỉ đăng ký 1 lần cho 1 khóa
                entity.HasIndex(e => new { e.UserId, e.PracticalCourseId })
                      .IsUnique();

                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.PracticalCourse)
                      .WithMany()
                      .HasForeignKey(e => e.PracticalCourseId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Order)
                      .WithMany()
                      .HasForeignKey(e => e.OrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Cấu hình PasswordResetToken
            modelBuilder.Entity<PasswordResetToken>(entity =>
            {
                entity.HasKey(prt => prt.Id);

                entity.Property(prt => prt.Otp)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(prt => prt.IsUsed)
                    .HasDefaultValue(false);

                entity.Property(prt => prt.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(prt => prt.User)
                    .WithMany()
                    .HasForeignKey(prt => prt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Index để query nhanh
                entity.HasIndex(prt => new { prt.UserId, prt.Otp });
            });
            // Cấu hình vital sign và medical record
            modelBuilder.Entity<VitalSign>()
                .HasIndex(v => v.MedicalRecordId)
                .IsUnique();

            modelBuilder.Entity<VitalSign>()
                .HasOne(v => v.AdmissionRecord)
                .WithMany(a => a.VitalSigns)
                .HasForeignKey(v => v.AdmissionRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình LabOrder
            modelBuilder.Entity<LabOrder>(entity =>
            {
                entity.HasKey(lo => lo.Id);

                entity.Property(lo => lo.Status)
                    .HasConversion<int>();

                entity.Property(lo => lo.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.HasOne(lo => lo.Appointment)
                    .WithMany()
                    .HasForeignKey(lo => lo.AppointmentId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(lo => lo.Items)
                    .WithOne(li => li.LabOrder)
                    .HasForeignKey(li => li.LabOrderId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Liên kết Invoice ↔ LabOrder (1-1)
                entity.HasOne<Invoice>()
                    .WithOne(i => i.LabOrder)
                    .HasForeignKey<Invoice>(i => i.LabOrderId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // Cấu hình LabOrderItem
            modelBuilder.Entity<LabOrderItem>(entity =>
            {
                entity.HasKey(li => li.Id);

                entity.Property(li => li.UnitPrice)
                    .HasColumnType("decimal(18,2)");

                entity.Property(li => li.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(li => li.Quantity)
                    .HasDefaultValue(1);

                entity.HasOne(li => li.MedicalService)
                    .WithMany()
                    .HasForeignKey(li => li.MedicalServiceId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Cấu hình MedicalService decimal
            modelBuilder.Entity<MedicalService>(entity =>
            {
                entity.Property(ms => ms.Price)
                    .HasColumnType("decimal(18,2)");
            });

            // Cấu hình AdmissionRecord
            modelBuilder.Entity<AdmissionRecord>(entity =>
            {
                entity.HasKey(a => a.Id);

                entity.HasOne(a => a.Patient)
                    .WithMany()
                    .HasForeignKey(a => a.PatientId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.Bed)
                    .WithMany()
                    .HasForeignKey(a => a.BedId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.MedicalRecord)
                    .WithMany()
                    .HasForeignKey(a => a.MedicalRecordId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(a => a.AdmittedByNurse)
                    .WithMany()
                    .HasForeignKey(a => a.AdmittedByNurseId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.AdmittedAt)
                    .HasDefaultValueSql("NOW()");

                // Index để query nhanh bệnh nhân đang nằm viện
                entity.HasIndex(a => new { a.PatientId, a.DischargedAt });
            });

            // Cấu hình WardOrder
            modelBuilder.Entity<WardOrder>(entity =>
            {
                entity.HasKey(wo => wo.Id);

                entity.HasOne(wo => wo.AdmissionRecord)
                    .WithMany()
                    .HasForeignKey(wo => wo.AdmissionRecordId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(wo => wo.CreatedByDoctor)
                    .WithMany()
                    .HasForeignKey(wo => wo.CreatedByDoctorId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(wo => wo.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.HasIndex(wo => new { wo.AdmissionRecordId, wo.Status });
            });

            // Cấu hình WardNote
            modelBuilder.Entity<WardNote>(entity =>
            {
                entity.HasKey(wn => wn.Id);

                entity.HasOne(wn => wn.AdmissionRecord)
                    .WithMany()
                    .HasForeignKey(wn => wn.AdmissionRecordId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(wn => wn.Author)
                    .WithMany()
                    .HasForeignKey(wn => wn.AuthorUserId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.Property(wn => wn.CreatedAt)
                    .HasDefaultValueSql("NOW()");

                entity.HasIndex(wn => new { wn.AdmissionRecordId, wn.CreatedAt });
            });

            ScenarioSeeder.SeedScenarios(modelBuilder);
        }
    }
}
