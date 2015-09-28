using System;
using System.Collections.Generic;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;

namespace Iris.Servicelayer.Interfaces
{
    public class UserIdAndUserName
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class UserStatus
    {
        public bool IsBaned { get; set; }
        public string Role { get; set; }
    }

    public interface IUserService
    {
        int Count { get; }

        /// <summary>
        ///     Add new user
        /// </summary>
        /// <param name="user">User instance</param>
        /// <returns>AddedUserStatus enum</returns>
        AddUserStatus Add(User user);

        /// <summary>
        ///     Verify user by user name and password
        /// </summary>
        /// <param name="userName">User name of the user</param>
        /// <param name="password">Password if the user</param>
        /// <param name="correctUserName"></param>
        /// <returns>VerifiedUserStatus Enum</returns>
        VerifyUserStatus VerifyUserByUserName(string userName, string password, ref string correctUserName,
            ref int userI, string ip);

        /// <summary>
        ///     Verify user by email and password
        /// </summary>
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <param name="userName"></param>
        /// <returns></returns>
        VerifyUserStatus VerifyUserByEmail(string email, string password, ref string userName, ref int userId, string ip);

        ChangePasswordResult ChangePasswordByUserName(string userName, string oldPassword, string newPassword);
        ChangePasswordResult ChangePasswordByUserId(int Id, string oldPassword, string newPassword);
        void DeActiveUser(int id);
        void DeActiveUsers(int[] usersId);
        void ActiveUser(int id);
        void ActiveUsers(int[] usersId);
        User GetUserById(int id);
        User GetUserByUserName(string userName);
        User GetUserByEmail(string email);
        IList<User> GetAllUsers();
        IList<User> GetUser(Func<User, bool> expression);
        bool ExistsByUserName(string userName);
        bool ExistsByEmail(string email);

        /// <summary>
        ///     Get the status of user by id
        ///     return true if user is active, else false
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>true if user is active;else false</returns>
        bool IsUserActive(int id);

        IList<UserDataTableModel> GetAllUsersDataTable(int page, int count, Order orderBy);
        UserDetailModel GetUserDetail(int id);
        int GetUsersNumber();
        IList<string> SearchUserName(string userName);
        IList<string> SearchUserId(string userId);
        Role GetUserRole(int userId);
        EditUserModel GetUserDataForEdit(int userId);
        EditedUserStatus EditUser(User user);

        IList<UserDataTableModel> GetDataTable(string term, int page, int count,
            Order order, UserOrderBy orderBy, UserSearchBy searchBy);

        User Find(string userName);
        IList<UserIdAndUserName> SearchUser(string userName);
        IList<string> GetUsersEmails();

        /// <summary>
        ///     Get role name by username
        /// </summary>
        /// <param name="userName">Username of selected user</param>
        /// <returns></returns>
        string GetRoleByUserName(string userName);

        string GetRoleByEmail(string email);
        string GetUserNameByEmail(string email);

        IList<string> SearchByUserName(string userName);
        IList<string> SearchByRoleDescription(string roleDescription);
        IList<string> SearchByFirstName(string firstName);
        IList<string> SearchByLastName(string lastName);
        IList<string> SearchByEmail(string email);
        IList<string> SearchByIP(string ip);

        EditProfileModel GetProfileData(string userName);

        EditedUserStatus UpdateProfile(EditProfileModel userModel);

        bool Authenticate(string userName, string password);

        IList<PostDetailModel> GetUserPostsDetail(int userId, int count, int page);
        IList<PostDetailModel> GetUserPostsDetail(string userName, int count, int page);

        bool IsBaned(string userName);

        UserStatus GetStatus(string userName);

        void UpdateUserLastActivity(string userName, DateTime time);
        int GetLastMonthActiveUsersCount();
    }
}