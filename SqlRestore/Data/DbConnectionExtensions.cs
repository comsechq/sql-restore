using System.Data;
using System.Linq;
using Dapper;

namespace Comsec.SqlRestore.Data
{
    /// <summary>
    /// Extension methods for Database interaction.
    /// </summary>
    public static class DbConnectionExtensions
    {
        /// <summary>
        /// Checks if a database with the given name existing on the server.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static bool DatabaseExists(this IDbConnection connection, string name)
        {
            var sql = string.Format("if db_id('{0}') is not null select 1;if not db_id('{0}') is not null select 0", name);

            var result = connection.Query<int>(sql)
                                   .First();

            return result == 1;
        }

        /// <summary>
        /// Drops the database with the given name
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="name">The name.</param>
        public static void DropDatabase(this IDbConnection connection, string name)
        {
            var sql = string.Format("ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE [{0}];", name);

            connection.Execute(sql);
        }
    }
}
