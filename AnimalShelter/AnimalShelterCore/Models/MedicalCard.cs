namespace AnimalShelterCore.Models;

public class MedicalCard
{
    public long Id { get; set; }
    public long AnimalId { get; set; }
    public DateTime DateOfLastVisit { get; set; }
    public string Diagnoses { get; set; } = string.Empty;
    public string Therapy { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is MedicalCard medicalCard &&
               medicalCard.Id == Id &&
               medicalCard.AnimalId == AnimalId &&
               medicalCard.DateOfLastVisit == DateOfLastVisit &&
               medicalCard.Diagnoses == Diagnoses &&
               medicalCard.Therapy == Therapy;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, AnimalId, DateOfLastVisit, Diagnoses, Therapy
        );
    }

    public override string ToString()
    {
        return $"Медкарта № {Id} (Id животного {AnimalId})";
    }
}