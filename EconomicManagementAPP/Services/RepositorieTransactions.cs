using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieTransactions
    {
        Task Create(Transactions transactions); // Se agrega task por el asincronismo        
        Task<IEnumerable<Transactions>> getTransactions();
        Task Modify(Transactions transactions);
        Task<Transactions> getTransactionsById(int Id); // para el modify
        Task Delete(int Id);
    }

    public class RepositorieTransactions : IRepositorieTransactions
    {
        private readonly string connectionString;

        public RepositorieTransactions(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task Create(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Transactions 
                                                (UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId) 
                                                VALUES (@UserId, @TransactionDate, @Total, @OperationTypeId, @Description, @AccountId, @CategoryId); SELECT SCOPE_IDENTITY();", transactions);
            transactions.Id = id;
        }
        

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<Transactions>> getTransactions()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Transactions>(@"SELECT Id, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                            FROM Transactions");
        }



        // Actualizar
        public async Task Modify(Transactions transactions)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Transactions
                                            SET UserId = @UserId,
                                                TransactionDate=@TransactionDate,
                                                Total=@Total,
                                                OperationTypeId=@OperationTypeId,
                                                Description=@Description,
                                                AccountId=@AccountId,
                                                CategoryId=@CategoryId
                                            WHERE Id = @Id", transactions);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<Transactions> getTransactionsById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Transactions>(@"
                                                                SELECT Id, UserId, TransactionDate, Total, OperationTypeId, Description, AccountId, CategoryId
                                                                FROM Transactions
                                                                WHERE Id = @Id",
                                                                new { Id });
        }

        //Eliminar
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Transactions WHERE Id = @Id", new { Id });
        }

    }
}
