using System.Collections.Generic;

namespace VehiGate.Domain.Entities
{
    public class CheckItem : BaseAuditableEntity
    {
        public string Name { get; set; }
        public CheckListAssociation AssociatedTo { get; set; }
        public ICollection<CheckListItem> CheckListItems { get; set; } = new List<CheckListItem>();
    }
}
