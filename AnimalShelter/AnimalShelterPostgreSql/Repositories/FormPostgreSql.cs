using AnimalShelterCore.Models;
using AnimalShelterCore.Repositories;
using Npgsql;

namespace AnimalShelterPostgreSql.Repositories;

class FormPostgreSql : IFormRepository
{
    private string connectionString;

    public FormPostgreSql(string connectionString)
    {
        this.connectionString = connectionString;
    }
    public void Add(Form form)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"INSERT INTO Form (RequestId, FamilyStatus, Housing, AnimalExperience) " +
            $"VALUES ({form.RequestId}, '{form.FamilyStatus}', " +
            $"'{form.Housing}', '{form.AnimalExperience}') RETURNING Id;"
        );
        var reader = command.ExecuteReader();
        reader.Read();
        form.Id = reader.GetInt64(0);
    }

    public void Delete(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"DELETE FROM Form WHERE Id = {id};"
        );
        command.ExecuteNonQuery();
    }

    public List<Form> GetAll()
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand("SELECT * FROM Form;");
        var reader = command.ExecuteReader();
        List<Form> forms = new List<Form>();
        while(reader.Read())
        {
            forms.Add(new Form
            {
                Id = reader.GetInt64(0),
                RequestId = reader.GetInt64(1),
                FamilyStatus = (FamilyStatus)reader.GetInt32(2),
                Housing = reader.GetString(3),
                AnimalExperience = reader.GetString(4)
            });
        }
    
        return forms;

    }

    public Form GetById(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Form WHERE Id = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();

        return new Form
        {
            Id = reader.GetInt64(0),
            RequestId = reader.GetInt64(1),
            FamilyStatus = (FamilyStatus)reader.GetInt32(2),
            Housing = reader.GetString(3),
            AnimalExperience = reader.GetString(4)
        };
    }

    public Request GetRequestByFormId(long id)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"SELECT * FROM Request WHERE FormId = {id};"
        );
        var reader = command.ExecuteReader();
        reader.Read();

        return new Request
        {
            Id = reader.GetInt64(0),
            UserId = reader.GetInt64(1),
            AnimalId = reader.GetInt64(2),
            FormId = reader.GetInt64(3),
            Status = (Status)reader.GetInt32(4)
        };
    }

    public void Update(Form form)
    {
        using var dataSource = NpgsqlDataSource.Create(connectionString);
        using var command = dataSource.CreateCommand(
            $"UPDATE Form SET RequestId = {form.RequestId}, " +
            $"FamilyStatus = '{form.FamilyStatus}', " +
            $"Housing = '{form.Housing}', " +
            $"AnimalExperience = '{form.AnimalExperience}' " +
            $"WHERE Id = {form.Id};"
        );
        command.ExecuteNonQuery();

    }
}