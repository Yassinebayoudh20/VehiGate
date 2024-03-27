namespace VehiGate.Domain.Entities
{
    public class DriverInspectionChecklist : BaseAuditableEntity
    {
        public string DriverInspectionId { get; set; }
        public DriverInspection DriverInspection { get; set; }

        public string ChecklistId { get; set; }
        public Checklist Checklist { get; set; }
    }
}
