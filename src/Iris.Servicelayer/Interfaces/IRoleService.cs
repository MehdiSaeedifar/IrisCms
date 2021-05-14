using System.Collections.Generic;
using Iris.DomainClasses.Entities;

namespace Iris.Servicelayer.Interfaces
{
    public interface IRoleService
    {
        /// <summary>
        ///     Add new role to roles
        /// </summary>
        /// <param name="roleName">The name of the role to create</param>
        bool CreateRole(string roleName, string description = "");

        void AddUserToRole(User user, string roleName);
        bool RoleExist(string roleName);
        void RemoveRole(string roleName);
        IList<Role> GetAllRoles();
        IList<User> UsersInRole(string roleName);
        Role GetRoleByUserName(string userName);
        Role GetRoleByUserId(int userId);
        void RemoveUserFromRole(string userName);
        void EditRoleForUser(string userName, string roleName);
        Role GetRoleByRoleName(string roleName);
        Role GetRoleByRoleId(int roleId);
    }
}