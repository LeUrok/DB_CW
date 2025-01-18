namespace AnimalShelterCore.Models;

public enum FamilyStatus
{
    InMarriage,
    NotInMarriage
}

public class Form
{
    public long Id { get; set; } 
    public long RequestId { get; set; } 
    public FamilyStatus FamilyStatus { get; set; }
    public string Housing { get; set; } = string.Empty;
    public string AnimalExperience { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is Form form &&
               form.Id == Id &&
               form.RequestId == RequestId &&
               form.FamilyStatus == FamilyStatus &&
               form.Housing == Housing &&
               form.AnimalExperience == AnimalExperience;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, FamilyStatus, Housing, AnimalExperience
        );
    }

    public override string ToString()
    {
        return $"Анкета № {Id}, запрос № {RequestId}";
    }
}
