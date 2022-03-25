using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieAccounts
    {
        Task Create(Accounts accounts); // Se agrega task por el asincronismo
        Task<bool> Exist(string Name);
        Task<IEnumerable<Accounts>> getAccounts();
        Task Modify(Accounts accounts);
        Task<Accounts> getAccountsById(int Id); // para el modify
        Task Delete(int Id);
    }

    public class RepositorieAccounts : IRepositorieAccounts
    {
        private readonly string connectionString;

        public RepositorieAccounts(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task Create(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Accounts 
                                                (Name, AccountTypeId, Balance, Description) 
                                                VALUES (@Name, @AccountTypeId, @Balance, @Description); SELECT SCOPE_IDENTITY();", accounts);
            accounts.Id = id;
        }

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Name)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM Accounts
                                    WHERE Name = @Name;",
                                    new { Name });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<Accounts>> getAccounts()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Accounts>(@"SELECT Id, Name, AccountTypeId, Balance, Description
                                                            FROM Accounts");
        }



        // Actualizar
        public async Task Modify(Accounts accounts)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Accounts
                                            SET Name = @Name, AccountTypeId=@AccountTypeId, Balance=@Balance, Description=@Description
                                            WHERE Id = @Id", accounts);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<Accounts> getAccountsById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Accounts>(@"
                                                                SELECT Id, Name, AccountTypeId, Balance, Description
                                                                FROM Accounts
                                                                WHERE Id = @Id",
                                                                new { Id });
        }

        //Eliminar
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Accounts WHERE Id = @Id", new { Id });
        }
       
    }
}
