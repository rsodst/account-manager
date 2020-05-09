using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Modulbank.Data.Context;
using Modulbank.Users.Extensions;
using Modulbank.Users.Models;
using Modulbank.Users.Tables;

namespace Modulbank.Users.Stores
{
    public class RoleStore : IQueryableRoleStore<ApplicationRole>, IRoleClaimStore<ApplicationRole>, IRoleStore<ApplicationRole>
    {
        private readonly RoleClaimsTable _roleClaimsTable;
        private readonly RolesTable _rolesTable;

        public RoleStore(IUsersContext context)
        {
            _rolesTable = new RolesTable(context);
            _roleClaimsTable = new RoleClaimsTable(context);
        }

        #region IQueryableRoleStore Implementation

        public IQueryable<ApplicationRole> Roles => Task.Run(() => _rolesTable.GetAllRolesAsync()).Result.AsQueryable();

        #endregion

        #region IRoleStore<ApplicationRole> Implementation

        public Task<IdentityResult> CreateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesTable.CreateAsync(role);
        }

        public Task<IdentityResult> UpdateAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesTable.UpdateAsync(role);
        }

        public Task<IdentityResult> DeleteAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return _rolesTable.DeleteAsync(role);
        }

        public Task<string> GetRoleIdAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.Id.ToString());
        }

        public Task<string> GetRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.Name);
        }

        public Task SetRoleNameAsync(ApplicationRole role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            roleName.ThrowIfNull(nameof(roleName));
            role.Name = roleName;
            return Task.CompletedTask;
        }

        public Task<string> GetNormalizedRoleNameAsync(ApplicationRole role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            return Task.FromResult(role.NormalizedName);
        }

        public Task SetNormalizedRoleNameAsync(ApplicationRole role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            normalizedName.ThrowIfNull(nameof(normalizedName));
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }

        public Task<ApplicationRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            roleId.ThrowIfNull(nameof(roleId));
            var isValIdGuId = Guid.TryParse(roleId, out var roleGuId);

            if (!isValIdGuId) throw new ArgumentException("Parameter roleId is not a valId Guid.", nameof(roleId));

            return _rolesTable.FindByIdAsync(roleGuId);
        }

        public Task<ApplicationRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            normalizedRoleName.ThrowIfNull(nameof(normalizedRoleName));
            return _rolesTable.FindByNameAsync(normalizedRoleName);
        }

        public void Dispose()
        {
            /* Nothing to dispose. */
        }

        #endregion

        #region IRoleClaimStore<ApplicationRole> Implementation

        public async Task<IList<Claim>> GetClaimsAsync(ApplicationRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            role.Claims = role.Claims ?? (await _roleClaimsTable.GetClaimsAsync(role.Id)).ToList();
            return role.Claims;
        }

        public async Task AddClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims ??= (await _roleClaimsTable.GetClaimsAsync(role.Id)).ToList();
            var foundClaim = role.Claims.FirstOrDefault(x => x.Type == claim.Type);

            if (foundClaim != null)
            {
                role.Claims.Remove(foundClaim);
                role.Claims.Add(claim);
            }
            else
            {
                role.Claims.Add(claim);
            }
        }

        public async Task RemoveClaimAsync(ApplicationRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            role.ThrowIfNull(nameof(role));
            claim.ThrowIfNull(nameof(claim));
            role.Claims ??= (await _roleClaimsTable.GetClaimsAsync(role.Id)).ToList();
            role.Claims.Remove(claim);
        }

        #endregion
    }
}