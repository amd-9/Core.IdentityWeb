using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.IdentityWeb
{
    public class IdentityWebUserStore : IUserStore<IdentityWebUser>, IUserPasswordStore<IdentityWebUser>
    {
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Data Source=192.168.88.24;Initial Catalog=IdentityWeb;User ID=sa;Password=a12345678!;");

            connection.Open();
            return connection;
        }

        public async Task<IdentityResult> CreateAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("insert into IdentityWebUsers([Id],[UserName],[NormalizedUserName],[PasswordHash]) values (@id, @userName, @normalizedUserName, @passwordHash)", new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                });
            }

            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        public async Task<IdentityWebUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<IdentityWebUser>(
                    "select * from IdentityWebUsers where Id = @id",
                    new {id = userId}
                    );
            }
        }

        public async Task<IdentityWebUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<IdentityWebUser>(
                    "select * from IdentityWebUsers where NormalizedUserName = @name",
                    new { name = normalizedUserName }
                    );
            }
        }

        public Task<string> GetNormalizedUserNameAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetPasswordHashAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<string> GetUserIdAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task<bool> HasPasswordAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetNormalizedUserNameAsync(IdentityWebUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetPasswordHashAsync(IdentityWebUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(IdentityWebUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(IdentityWebUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("update IdentityWebUsers set [Id] = @Id, [UserName] = @userName, [NormalizedUserName[ = @normalizedUserName, [PasswordHash] = @passwordHash where [Id] = @id", new
                {
                    id = user.Id,
                    userName = user.UserName,
                    normalizedUserName = user.NormalizedUserName,
                    passwordHash = user.PasswordHash
                });
            }

            return IdentityResult.Success;
        }
    }
}
