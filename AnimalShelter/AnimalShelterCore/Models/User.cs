namespace AnimalShelterCore.Models;

public class User
{
    public long Id { get; set; } 
    public long? EmployeeId { get; set; } 
    public long? GuardianId { get; set; } 
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Mail { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is User user &&
               user.Id == Id &&
               user.EmployeeId == EmployeeId &&
               user.Name == Name &&
               user.Phone == Phone &&
               user.Mail == Mail;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, Name, Phone, Mail
        );
    }

    public override string ToString()
    {
        return $"{Name}: {Mail}";
    }
}
