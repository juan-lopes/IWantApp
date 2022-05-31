using Dapper;
using IWantApp.Endpoints.Employees;
using Microsoft.Data.SqlClient;

namespace IWantApp.Infra.Data;

public class QueryAllUsersWithClaimName
{
    private readonly IConfiguration configuration;

    public QueryAllUsersWithClaimName(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<IEnumerable<EmployeeResponse>> Execute(int page, int rows)
    {
        var db = new SqlConnection(configuration["ConnectionString:IWantDb"]);
        var query =
                @"SELECT Email, ClaimValue AS Name
                FROM AspNetUsers u 
                INNER JOIN AspNetUsersClaim c
                ON u.id = c.UserId
                AND claimtype = 'Name'
                ORDER BY Name
                OFSSET (@page -1) * rows ROWS FETCH NEXT @rows ROWS ONLY";
        return await db.QueryAsync<EmployeeResponse>(
            query,
            new { page, rows }
            );
    }
}
