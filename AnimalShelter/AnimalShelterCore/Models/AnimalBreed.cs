namespace AnimalShelterCore.Models;

public class AnimalBreed
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public override bool Equals(object? obj)
    {
        return obj is AnimalBreed animalBreed &&
               animalBreed.Id == Id &&
               animalBreed.Name == Name;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(
            Id, Name
        );
    }

    public override string ToString()
    {
        return Name;
    }
}