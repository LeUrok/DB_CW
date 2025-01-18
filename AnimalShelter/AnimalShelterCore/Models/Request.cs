namespace AnimalShelterCore.Models;

public enum Status
{
    InProgress,
    Cancelled,
    Approved
}
public class Request
{
    public long Id { get; set; } 
    public long UserId { get; set; } 
    public long AnimalId { get; set; } 
    public long FormId { get; set; } 
    public Status Status { get; set; }

    public override bool Equals(object? obj)
    {
        return obj is Request request &&
               request.Id == Id &&
               request.UserId == UserId &&
               request.AnimalId == AnimalId &&
               request.FormId == FormId &&
               request.Status == Status;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, UserId, AnimalId, FormId, Status
        );
    }

    public override string ToString()
    {
        return $"Заявка № {Id}, пользователь № {UserId}, животное № {AnimalId}";
    }
}