namespace VehiGate.Domain.Entities
{
    public class CheckListItem : BaseAuditableEntity
    {
        public string CheckListId { get; set; }
        public Checklist CheckList { get; set; }

        public string CheckItemId { get; set; }
        public CheckItem CheckItem { get; set; }

        public bool State { get; set; }
        public string Note { get; set; }
    }
}
