using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

public class PostgreSqlDataBase : IDataBase
{
    public IAnimalRepository AnimalRepository => animals;
    public IAnimalTypeRepository AnimalTypeRepository => types;
    public IAnimalBreedRepository AnimalBreedRepository => breeds;

    public IEmpoyeeRepository EmployeeRepository => employees;

    public IFormRepository FormRepository => forms;

    public IGuardianRepository GuardianRepository => guardians;

    public IMedicalCardRepository MedicalCardRepository => medicalCards;

    public IRequestRepository RequestRepository => requests;

    public IUserRepository UserRepository => users;
    public void Initialize()
    {
        CreateDbIfNotExists();
    }
    public void DeleteDb()
    {
        // using var dataSource = NpgsqlDataSource.Create(GetConnectionStringWithoutDb());
        using var dataSource = new NpgsqlConnection(GetConnectionStringWithoutDb());
        dataSource.Open();
        using var command = new NpgsqlCommand(
            $"DROP DATABASE {dbName};", dataSource
        );
        command.ExecuteNonQuery();
        dataSource.Close();
    }
    public PostgreSqlDataBase(
        string host, int port, string dbName, string userName, string password
    )
    {
        this.host = host;
        this.port = port;
        this.dbName = dbName.ToLower();
        this.userName = userName;
        this.password = password;
        this.animals = new AnimalPostgreSql(GetConnectionString());
        this.types = new AnimalTypePostgreSql(GetConnectionString());
        this.breeds = new AnimalBreedPostgreSql(GetConnectionString());
        this.employees = new EmployeePostgreSql(GetConnectionString());
        this.forms = new FormPostgreSql(GetConnectionString());
        this.guardians = new GuardianPostgreSql(GetConnectionString());
        this.medicalCards = new MedicalCardPostgreSql(GetConnectionString(), this.AnimalRepository);
        this.requests = new RequestPostgreSql(GetConnectionString());
        this.users = new UserPostgreSql(GetConnectionString());
    }

    public string GetConnectionString()
    {
        return $"Host={host};Port={port};Username={userName};Password={password};Database={dbName};";
    }

    private string GetConnectionStringWithoutDb()
    {
        return $"Host={host};Port={port};Username={userName};Password={password};";
    }

    private void CreateDbIfNotExists()
    {
        if (DbExists())
            return;
        
        throw new Exception("Database not exists");
    }
    private bool DbExists()
    {
        using var dataSource = NpgsqlDataSource.Create(GetConnectionStringWithoutDb());
        using var command = dataSource.CreateCommand($"SELECT FROM pg_database WHERE datname = '{dbName}';");
        var reader = command.ExecuteReader();

        return reader.Read();
    }
    public void CreateDB()
    {
        using var dataSource = NpgsqlDataSource.Create(GetConnectionStringWithoutDb());
        using var command = dataSource.CreateCommand($"CREATE DATABASE {dbName};");
        command.ExecuteNonQuery();
    }    
    private string host;
    private int port;
    private string dbName;
    private string userName;
    private string password;
    private AnimalPostgreSql animals;
    private AnimalTypePostgreSql types;
    private AnimalBreedPostgreSql breeds;
    private EmployeePostgreSql employees;
    private FormPostgreSql forms;
    private GuardianPostgreSql guardians;
    private MedicalCardPostgreSql medicalCards;
    private RequestPostgreSql requests;
    private UserPostgreSql users;
}