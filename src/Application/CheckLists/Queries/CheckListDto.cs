using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehiGate.Domain.Enums;

namespace VehiGate.Application.CheckLists.Queries;

public class CheckListItemDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public CheckListAssociation Association { get; set; }
    public bool State { get; set; } = false;
    public string Note { get; set; } = String.Empty;
}
