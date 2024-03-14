namespace VehiGate.Domain.Common;

public class ContactDetails
{
    public string Address { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Contact { get; set; }

    public ContactDetails(string address, string email, string phone, string contact)
    {
        Address = address;
        Email = email;
        Phone = phone;
        Contact = contact;
    }
}
