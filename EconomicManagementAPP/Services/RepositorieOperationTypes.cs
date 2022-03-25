using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieOperationTypes
    {
        Task Create(OperationTypes operationTypes); // Se agrega task por el asincronismo
        Task<bool> Exist(string Description);
        Task<IEnumerable<OperationTypes>> getOperationTypes();
        Task Modify(OperationTypes operationTypes);
        Task<OperationTypes> getOperationTypesById(int Id); // para el modify
        Task Delete(int Id);
    }
    public class RepositorieOperationTypes : IRepositorieOperationTypes
    {
        private readonly string connectionString;

        public RepositorieOperationTypes(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        // El async va acompañado de Task
        public async Task Create(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            // Requiere el await - tambien requiere el Async al final de la query
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO OperationTypes 
                                                (Description) 
                                                VALUES (@Description); SELECT SCOPE_IDENTITY();", operationTypes);
            operationTypes.Id = id;            
        }
        

        //Cuando retorna un tipo de dato se debe poner en el Task Task<bool>
        public async Task<bool> Exist(string Description)
        {
            using var connection = new SqlConnection(connectionString);
            // El select 1 es traer lo primero que encuentre y el default es 0
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM OperationTypes
                                    WHERE Description = @Description;",
                                    new { Description });
            return exist == 1;
        }

        // Obtenemos las cuentas del usuario
        public async Task<IEnumerable<OperationTypes>> getOperationTypes()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<OperationTypes>(@"SELECT Id, Description FROM OperationTypes");
        }



        // Actualizar
        public async Task Modify(OperationTypes operationTypes)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE OperationTypes
                                            SET Description = @Description
                                            WHERE Id = @Id", operationTypes);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<OperationTypes> getOperationTypesById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<OperationTypes>(@"
                                                                SELECT Description
                                                                FROM OperationTypes
                                                                WHERE Id = @Id",
                                                                new { Id });
        }

        //Eliminar
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE OperationTypes WHERE Id = @Id", new { Id });
        }

    }


}
