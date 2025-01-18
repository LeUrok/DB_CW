using Npgsql;
using AnimalShelterPostgreSql.Repositories;
namespace AnimalShelterTests;
using AnimalShelterCore.Models;
class DatabaseTests
{
    [Test]
    public void GetAnimalsByBreed_GetOnEmptyDB_ReturnsEmptyResult()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        using var command = new NpgsqlCommand("SELECT * FROM GetAnimalsByBreed(2);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }
    [Test]
    public void GetAnimalsByBreed_WithExistingBreed_ReturnsCorrectAnimals()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                            "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                        "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByBreed(2);", dataSource);
        var reader = command.ExecuteReader();
        Assert.True(reader.Read());
            
        while (reader.Read())
        {
            Animal animal = new Animal
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                AnimalTypeId = reader.GetInt64(2),
                AnimalBreedId = reader.GetInt64(3),
                Gender = (Gender)reader.GetInt32(4),
                Age = reader.GetInt32(5),
                Weight = reader.GetDouble(6),
                Description = reader.GetString(7),
            };

            Assert.That(animal.Id, Is.EqualTo(1));
            Assert.That(animal.Name, Is.EqualTo("Барсик"));
            Assert.That(animal.AnimalTypeId, Is.EqualTo(1));
            Assert.That(animal.AnimalBreedId, Is.EqualTo(2));
            Assert.That(animal.Gender, Is.EqualTo(Gender.Male));
            Assert.That(animal.Age, Is.EqualTo(3));
            Assert.That(animal.Weight, Is.EqualTo(5.0));
            Assert.That(animal.Description, Is.EqualTo("Спокойный и дружелюбный кот"));

        }

        dataSource.Close();
        
    }
    [Test]
    public void GetAnimalsByBreed_WithInvalidBreedId_ReturnsNoRows()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                            "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                        "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByBreed(-1);", dataSource);
        using var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }
    [Test]
    public void GetAnimalsByBreed_WithNoAnimalsOfBreed_ReturnsEmptyResult()
    {        
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                            "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                        "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByBreed(3);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }
    [Test]
    public void GetAnimalsByType_GetOnEmptyDB_ReturnsEmptyResult()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        using var command = new NpgsqlCommand("SELECT * FROM GetAnimalsByType(1);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByType_WithExistingType_ReturnsCorrectAnimals()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 1, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();

        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByType(1);", dataSource);
        var reader = command.ExecuteReader();
        Assert.True(reader.Read());

        while (reader.Read())
        {
            Animal animal = new Animal
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                AnimalTypeId = reader.GetInt64(2),
                AnimalBreedId = reader.GetInt64(3),
                Gender = (Gender)reader.GetInt32(4),
                Age = reader.GetInt32(5),
                Weight = reader.GetDouble(6),
                Description = reader.GetString(7),
            };

            Assert.That(animal.Id, Is.EqualTo(1));
            Assert.That(animal.Name, Is.EqualTo("Барсик"));
            Assert.That(animal.AnimalTypeId, Is.EqualTo(1));
            Assert.That(animal.AnimalBreedId, Is.EqualTo(2));
            Assert.That(animal.Gender, Is.EqualTo(Gender.Male));
            Assert.That(animal.Age, Is.EqualTo(3));
            Assert.That(animal.Weight, Is.EqualTo(5.0));
            Assert.That(animal.Description, Is.EqualTo("Спокойный и дружелюбный кот"));

        }
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByType_WithInvalidTypeId_ReturnsNoRows()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 1, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();

        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByType(-1);", dataSource);
        using var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByType_WithNoAnimalsOfType_ReturnsEmptyResult()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 1, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();

        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByType(2);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }
    [Test]
    public void GetAnimalsByTypeAndBreed_GetOnEmptyDB_ReturnsEmptyResult()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        using var command = new NpgsqlCommand("SELECT * FROM GetAnimalsByTypeAndBreed(1, 1);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByTypeAndBreed_WithExistingTypeAndBreed_ReturnsCorrectAnimals()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByTypeAndBreed(1, 2);", dataSource);
        var reader = command.ExecuteReader();
        Assert.True(reader.Read());
        while (reader.Read())
        {
            Animal animal = new Animal
            {
                Id = reader.GetInt64(0),
                Name = reader.GetString(1),
                AnimalTypeId = reader.GetInt64(2),
                AnimalBreedId = reader.GetInt64(3),
                Gender = (Gender)reader.GetInt32(4),
                Age = reader.GetInt32(5),
                Weight = reader.GetDouble(6),
                Description = reader.GetString(7),
            };

            Assert.That(animal.Id, Is.EqualTo(1));
            Assert.That(animal.Name, Is.EqualTo("Барсик"));
            Assert.That(animal.AnimalTypeId, Is.EqualTo(1));
            Assert.That(animal.AnimalBreedId, Is.EqualTo(2));
            Assert.That(animal.Gender, Is.EqualTo(Gender.Male));
            Assert.That(animal.Age, Is.EqualTo(3));
            Assert.That(animal.Weight, Is.EqualTo(5.0));
            Assert.That(animal.Description, Is.EqualTo("Спокойный и дружелюбный кот"));

        }
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByTypeAndBreed_WithInvalidTypeIdAndBreedId_ReturnsNoRows()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByTypeAndBreed(-1, -1);", dataSource);
        using var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }

    [Test]
    public void GetAnimalsByTypeAndBreed_WithNoAnimalsOfGivenTypeAndBreed_ReturnsEmptyResult()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        var command = new NpgsqlCommand("INSERT INTO AnimalType (Name) VALUES ('Кошка'), ('Собака'); " +
                                        "INSERT INTO AnimalBreed (Name) VALUES ('Британская'), ('Сибирская'), ('Лабрадор'), ('Бульдог');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("INSERT INTO Animal (Name, AnimalTypeId, AnimalBreedId, Gender, Age, Weight, Description) " +
                                    "VALUES ('Барсик', 1, 2, 1, 3, 5.0, 'Спокойный и дружелюбный кот');", dataSource);
        command.ExecuteNonQuery();
        command = new NpgsqlCommand("SELECT * FROM GetAnimalsByTypeAndBreed(1, 3);", dataSource);
        var reader = command.ExecuteReader();
        Assert.False(reader.Read());
        dataSource.Close();
    }
    [SetUp]
    public void CreateDB()
    {
        PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "animalsheltertests", "postgres", "root1234");
        db.CreateDB();
        using var dataSource = new NpgsqlConnection(db.GetConnectionString());
        dataSource.Open();
        using var command = new NpgsqlCommand(File.ReadAllText("/Users/leurok/Study/DB_Course_work/test/create.sql"), dataSource);
        command.ExecuteNonQuery();
    }
    [TearDown]
    public void DropDB()
    {
        using (var conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=root1234; Database=postgres"))
        {
            conn.Open();
            using (var cmd = new NpgsqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "SELECT pg_terminate_backend(pg_stat_activity.pid) FROM pg_stat_activity WHERE pg_stat_activity.datname = 'animalsheltertests' AND pid <> pg_backend_pid();";
                cmd.ExecuteNonQuery();
                cmd.CommandText = "DROP DATABASE IF EXISTS animalsheltertests;";
                cmd.ExecuteNonQuery();
            }
        }
        NpgsqlConnection.ClearAllPools();
        
    }
}