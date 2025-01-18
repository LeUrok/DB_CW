namespace AnimalShelterCore.Models;

public class Guardian
{
    public long Id { get; set; } 
    public List<long> AnimalIds { get; set; } = new ();
    public long UserId { get; set; } 
    public DateOnly StartDate { get; set; } 
    public DateOnly? EndDate { get; set; } 

    public override bool Equals(object? obj)
    {
        return obj is Guardian guardian &&
               guardian.Id == Id &&
               guardian.UserId == UserId &&
               guardian.StartDate == StartDate &&
               guardian.EndDate == EndDate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, UserId, StartDate, EndDate
        );
    }

    public override string ToString()
    {
        return $"Опекун № {Id}";
    }
}
