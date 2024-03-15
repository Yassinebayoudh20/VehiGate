using Newtonsoft.Json;

namespace VehiGate.Domain.Entities;
public class Company : BaseAuditableEntity
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Contact { get; set; }

    [JsonIgnore]
    public virtual ICollection<Driver> Drivers { get; set; } = new List<Driver>();
}
