using System.ComponentModel.DataAnnotations;

namespace FirstAidAPI.DTO.Admission
{
    /// <summary>
    /// Payload để y tá gán giường cho bệnh nhân.
    /// </summary>
    public class AssignBedRequest
    {
        [Required]
        public int MedicalRecordId { get; set; }

        [Required]
        public int PatientId { get; set; }

        [Required]
        public int BedId { get; set; }

        public string? Notes { get; set; }
    }
}
