using System.Data.Common;
using AnimalShelterPostgreSql.Repositories;

PostgreSqlDataBase db = new PostgreSqlDataBase("localhost", 5432, "testdb", "postgres", "root1234");

try
{
    db.Initialize();
}
catch(Exception e)
{
    db.DeleteDb();
    Console.WriteLine(e.Message);
}
