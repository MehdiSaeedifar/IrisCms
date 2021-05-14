using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Servicelayer.Interfaces;

namespace Iris.Servicelayer.EFServices
{
    public class RoleService : IRoleService
    {
        private readonly DbSet<Role> _roles;

        public RoleService(IUnitOfWork uow) //, IUserService userService)
        {
            _roles = uow.Set<Role>();
        }


        public bool CreateRole(string roleName, string description = "")
        {
            if (RoleExist(roleName))
                return false;
            _roles.Add(new Role {Name = roleName, Description = description});
            return true;
        }

        public void AddUserToRole(User user, string roleName)
        {
            user.Role = GetRoleByRoleName(roleName);
        }

        public bool RoleExist(string roleName)
        {
            return _roles.Any(x => x.Name.Equals(roleName));
        }

        public void RemoveRole(string roleName)
        {
            _roles.Remove(_roles.FirstOrDefault(x => x.Name.Equals(roleName)));
        }


        public IList<Role> GetAllRoles()
        {
            return _roles.ToList();
        }

        public IList<User> UsersInRole(string roleName)
        {
            //return this._users.GetUser(x => x.Role.Name.Equals(roleName)).ToList();
            throw new NotImplementedException();
        }

        public Role GetRoleByUserName(string userName)
        {
            //return this._users.GetUserByUserName(userName).Role;
            throw new NotImplementedException();
        }

        public Role GetRoleByUserId(int userId)
        {
            return
                _roles.Where(role => role.Users.Where(user => user.Id == userId).FirstOrDefault().Id == userId)
                    .FirstOrDefault();
        }

        public void RemoveUserFromRole(string userName)
        {
            //this._roles.Remove(this._roles.Where(x=>x.Users.Where(y=>y.UserName.Equals(userName)).FirstOrDefault());
            throw new NotImplementedException();
        }

        public void EditRoleForUser(string userName, string roleName)
        {
            //this._users.GetUserByUserName(userName).Role = this.GetRoleByRoleName(roleName);
        }

        public Role GetRoleByRoleId(int roleId)
        {
            return _roles.Find(roleId);
        }

        public Role GetRoleByRoleName(string roleName)
        {
            return _roles.Where(x => x.Name.Equals(roleName)).FirstOrDefault();
        }
    }
}