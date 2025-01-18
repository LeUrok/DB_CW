namespace AnimalShelterCore.Models;

public enum Gender
{
    Male,
    Female
}

public class Animal
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public long AnimalTypeId { get; set; }
    public long AnimalBreedId { get; set; }
    public Gender Gender { get; set; }
    public int Age { get; set; }
    public double Weight { get; set; }
    public string Description { get; set; } = string.Empty;
    public long? MedicalCardId { get; set; }
    public List<long> GuardianIds { get; set; } = new ();

    public override bool Equals(object? obj)
    {
        return obj is Animal animal &&
               animal.Id == Id &&
               animal.Name == Name &&
               animal.AnimalTypeId == AnimalTypeId &&
               animal.AnimalBreedId == AnimalBreedId &&
               animal.Gender == Gender &&
               animal.Age == Age &&
               animal.Weight == Weight &&
               animal.MedicalCardId == MedicalCardId &&
               animal.Description == Description;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description
        );
    }

    public override string ToString()
    {
        return Name;
    }
}