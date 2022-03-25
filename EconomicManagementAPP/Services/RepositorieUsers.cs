using EconomicManagementAPP.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace EconomicManagementAPP.Services
{
    public interface IRepositorieUsers
    {
        Task Create(Users users); // Se agrega task por el asincronismo
        Task<bool> Exist(string Email); //Hacemos asincrona la validacion de un email existente
        Task<IEnumerable<Users>> getUser(); //Creamos un metodo al cual despues llamamos para la creacion de la lista
        Task Modify(Users users);      
        Task<Users> getUserById(int Id); // para el modify
        Task Delete(int Id);
    }

    public class RepositorieUsers : IRepositorieUsers
    {
        private readonly string connectionString;

        public RepositorieUsers(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // El async va acompañado de Task para crear el usuario
        public async Task Create(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<int>($@"INSERT INTO Users 
                                                (Email, StandarEmail, Password) 
                                                VALUES (@Email, @StandarEmail, @Password); SELECT SCOPE_IDENTITY();", users);
            users.Id = id;
        }

        //Consulta para generar un boleano mediante la posible existencia de un email ya registrado
        //Esta consulta de validacion se llama desde el controller 
        public async Task<bool> Exist(string Email)
        {
            using var connection = new SqlConnection(connectionString);
            var exist = await connection.QueryFirstOrDefaultAsync<int>(
                                    @"SELECT 1
                                    FROM Users
                                    WHERE Email = @Email;",
                                    new { Email });
            return exist == 1;
        }

        // Obtenemos las cuentas de la tabla users que son llamadas en Views/Users/Index
        //Con esta query dentro de un foreach generamos el listado en Views/Users/Index
        public async Task<IEnumerable<Users>> getUser()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Users>(@"SELECT Id, Email, StandarEmail FROM Users");
        }





        // Actualizar
        public async Task Modify(Users users)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users
                                            SET Email = @Email, StandarEmail=@StandarEmail, Password=@Password
                                            WHERE Id = @Id", users);
        }

        //Para actualizar se necesita obtener el tipo de cuenta por el id
        public async Task<Users> getUserById(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"
                                                                SELECT Id, Email, StandarEmail, Password
                                                                FROM Users
                                                                WHERE Id = @Id",
                                                                new { Id });
        }

        //Eliminar
        public async Task Delete(int Id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync("DELETE Users WHERE Id = @Id", new { Id });
        }       
    }


}
