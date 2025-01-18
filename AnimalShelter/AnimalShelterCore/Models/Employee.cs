namespace AnimalShelterCore.Models;

public class Employee
{
    public long Id { get; set; } 
    public long UserId { get; set; } 
    public string Job { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is Employee employee &&
               employee.Id == Id &&
               employee.UserId == UserId &&
               employee.Job == Job;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, Job
        );
    }

    public override string ToString()
    {
        return $"Сотрудник {Id} № {Id}, должность -- {Job}";
    }
}
