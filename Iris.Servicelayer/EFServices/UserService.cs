using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Iris.Datalayer.Context;
using Iris.DomainClasses.Entities;
using Iris.Model;
using Iris.Model.AdminModel;
using Iris.Servicelayer.EFServices.Enums;
using Iris.Servicelayer.Interfaces;
using Iris.Utilities.Caching;
using Iris.Utilities.DateAndTime;
using Iris.Utilities.Security;

namespace Iris.Servicelayer.EFServices
{
    public class UserService : IUserService
    {
        private static int _searchTakeCount;
        private readonly IDbSet<User> _users;

        public UserService(IUnitOfWork uow)
        {
            _users = uow.Set<User>();
            _searchTakeCount = 10;
        }

        #region Private Members

        private bool isUserNameAndEmailExist(string userName, string email)
        {
            return _users.Any(x => x.UserName == userName || x.Email == email);
        }

        #endregion

        #region Adding new user

        public AddUserStatus Add(User user)
        {
            if (ExistsByEmail(user.Email))
                return AddUserStatus.EmailExist;
            if (ExistsByUserName(user.UserName))
                return AddUserStatus.UserNameExist;

            _users.Add(user);
            return AddUserStatus.AddingUserSuccessfully;
        }

        #endregion

        public EditedUserStatus EditUser(User user)
        {
            User selectedUser = GetUserById(user.Id);
            if (selectedUser.UserName != user.UserName || selectedUser.Email != user.Email)
            {
                if (isUserNameAndEmailExist(user.UserName, user.Email))
                {
                    return EditedUserStatus.UserNameOrEmailExist;
                }
            }
            selectedUser.Email = user.Email;
            selectedUser.IsBaned = user.IsBaned;
            selectedUser.Role = user.Role;
            if (user.BanedDate != null)
            {
                selectedUser.BanedDate = user.BanedDate;
            }
            if (!string.IsNullOrEmpty(user.Password))
            {
                selectedUser.Password = Encryption.EncryptingPassword(user.Password);
                selectedUser.LastPasswordChange = user.LastPasswordChange;
            }
            selectedUser.UserName = user.UserName;
            selectedUser.UserMetaData.BirthDay = user.UserMetaData.BirthDay;
            selectedUser.UserMetaData.Description = user.UserMetaData.Description;
            selectedUser.UserMetaData.FirstName = user.UserMetaData.FirstName;
            selectedUser.UserMetaData.LastName = user.UserMetaData.LastName;
            selectedUser.UserMetaData.Major = user.UserMetaData.Major;
            return EditedUserStatus.UpdatingUserSuccessfully;
        }

        public IList<string> SearchUserName(string userName)
        {
            return _users.Where(x => x.UserName.Contains(userName)).Select(x => x.UserName).Take(10).ToList();
        }


        public IList<string> SearchUserId(string userId)
        {
            int id = Convert.ToInt32(userId);
            return
                _users.Select(x => new { x.Id, x.UserName })
                    .ToList()
                    .Where(x => x.Id.ToString().Contains(userId))
                    .Select(x => x.UserName)
                    .ToList();
        }

        [CacheMethod]
        public Role GetUserRole(int userId)
        {
            return _users.Find(userId).Role;
        }


        public EditUserModel GetUserDataForEdit(int userId)
        {
            return
                _users.Include(x => x.UserMetaData)
                    .Where(x => x.Id == userId)
                    .Select(
                        user =>
                            new EditUserModel
                            {
                                BirthDay = user.UserMetaData.BirthDay,
                                Description = user.UserMetaData.Description,
                                Email = user.Email,
                                FirstName = user.UserMetaData.FirstName,
                                UserName = user.UserName,
                                Id = user.Id,
                                LastName = user.UserMetaData.LastName,
                                Major = user.UserMetaData.Major,
                                RoleId = user.Role.Id,
                                IsBaned = user.IsBaned
                            })
                    .FirstOrDefault();
        }


        [CacheMethod]
        public User Find(string userName)
        {
            return _users.FirstOrDefault(user => user.UserName.Equals(userName));
        }


        public IList<UserIdAndUserName> SearchUser(string userName)
        {
            return _users.Where(user => user.UserName.Contains(userName)).Select(user =>
                new UserIdAndUserName { Id = user.Id, UserName = user.UserName }).ToList();
        }


        public IList<string> GetUsersEmails()
        {
            return _users.Select(user => user.Email).ToList();
        }

        public string GetUserNameByEmail(string email)
        {
            return _users.Where(user => user.Email.Equals(email)).Select(user => user.UserName).FirstOrDefault();
        }


        public int Count
        {
            get { return _users.Count(); }
        }

        public IList<string> SearchByUserName(string userName)
        {
            return
                _users.Where(user => user.UserName.Contains(userName))
                    .Take(_searchTakeCount)
                    .Select(user => user.UserName)
                    .ToList();
        }

        public IList<string> SearchByRoleDescription(string roleDescription)
        {
            return _users.Where(user => user.Role.Description.Contains(roleDescription))
                .Take(_searchTakeCount).Select(user => user.Role.Description).Distinct().ToList();
        }

        public IList<string> SearchByFirstName(string firstName)
        {
            return _users.Where(user => user.UserMetaData.FirstName.Contains(firstName))
                .Take(_searchTakeCount).Select(user => user.UserMetaData.FirstName).Distinct().ToList();
        }

        public IList<string> SearchByLastName(string lastName)
        {
            return _users.Where(user => user.UserMetaData.LastName.Contains(lastName))
                .Take(_searchTakeCount).Select(user => user.UserMetaData.LastName).Distinct().ToList();
        }

        public IList<string> SearchByEmail(string email)
        {
            return _users.Where(user => user.Email.Contains(email))
                .Take(_searchTakeCount).Select(user => user.Email).ToList();
        }

        public IList<string> SearchByIP(string ip)
        {
            return _users.Where(user => user.IP.Contains(ip))
                .Take(_searchTakeCount).Select(user => user.IP).Distinct().ToList();
        }


        public EditProfileModel GetProfileData(string userName)
        {
            return _users.Where(user => user.UserName.Equals(userName)).Select(user =>
                new EditProfileModel
                {
                    BirthDay = user.UserMetaData.BirthDay,
                    Email = user.Email,
                    FirstName = user.UserMetaData.FirstName,
                    Description = user.UserMetaData.Description,
                    LastName = user.UserMetaData.LastName,
                    Major = user.UserMetaData.Major
                }).FirstOrDefault();
        }


        public EditedUserStatus UpdateProfile(EditProfileModel userModel)
        {
            EditedUserStatus result;
            if (ExistsByEmail(userModel.Email) && !GetUserNameByEmail(userModel.Email).Equals(userModel.UserName))
            {
                result = EditedUserStatus.EmailExist;
            }
            else
            {
                User selectedUser = _users.FirstOrDefault(user => user.UserName.Equals(userModel.UserName));
                selectedUser.Email = userModel.Email;
                selectedUser.UserMetaData.FirstName = userModel.FirstName;
                selectedUser.UserMetaData.LastName = userModel.LastName;
                selectedUser.UserMetaData.Major = userModel.Major;
                selectedUser.UserMetaData.BirthDay = userModel.BirthDay;
                selectedUser.UserMetaData.Description = userModel.Description;
                if (!string.IsNullOrEmpty(userModel.NewPassword))
                {
                    selectedUser.Password = Encryption.EncryptingPassword(userModel.NewPassword);
                    selectedUser.LastPasswordChange = DateAndTime.GetDateTime();
                }
                result = EditedUserStatus.UpdatingUserSuccessfully;
            }
            return result;
        }

        public bool Authenticate(string userName, string password)
        {
            bool result = false;
            string userPass =
                _users.Where(user => user.UserName.Equals(userName)).Select(user => user.Password).FirstOrDefault();
            if (userPass == null)
            {
                result = false;
            }
            else
            {
                if (Encryption.VerifyPassword(password, userPass))
                {
                    result = true;
                }
            }
            return result;
        }

        [CacheMethod]
        public IList<PostDetailModel> GetUserPostsDetail(int userId, int count, int page)
        {
            return _users.Find(userId).Posts.Select(post => new PostDetailModel
            {
                CommnetCount = post.Comments.Count,
                Id = post.Id,
                LikeCount = post.Like,
                PostedDate = post.CreatedDate,
                Title = post.Title,
                VisitedCount = post.VisitedNumber
            }).OrderByDescending(post => post.PostedDate).Skip(count * page).Take(count).ToList();
        }

        [CacheMethod]
        public IList<PostDetailModel> GetUserPostsDetail(string userName, int count, int page)
        {
            return
                _users.AsNoTracking()
                    .Include(user => user.Posts)
                    .First(user => user.UserName == userName)
                    .Posts.Select(post => new PostDetailModel
                    {
                        CommnetCount = post.Comments.Count,
                        Id = post.Id,
                        LikeCount = post.Like,
                        PostedDate = post.CreatedDate,
                        Title = post.Title,
                        VisitedCount = post.VisitedNumber
                    }).OrderByDescending(post => post.PostedDate).Skip(count * page).Take(count).ToList();
        }


        public bool IsBaned(string userName)
        {
            return _users.Single(x => x.UserName == userName).IsBaned;
        }

        [CacheMethod]
        public UserStatus GetStatus(string userName)
        {
            return
                _users.AsNoTracking()
                    .Where(user => user.UserName == userName)
                    .Select(user => new UserStatus { IsBaned = user.IsBaned, Role = user.Role.Name })
                    .Single();
        }

        #region Role Operations

        [CacheMethod]
        public string GetRoleByUserName(string userName)
        {
            return _users.First(user => user.UserName.Equals(userName)).Role.Name;
        }

        public string GetRoleByEmail(string email)
        {
            return _users.First(user => user.Email.Equals(email)).Role.Name;
        }

        #endregion

        #region Verification Operations

        #region private members

        private static VerifyUserStatus Verify(User selectedUser, string password)
        {
            var result = VerifyUserStatus.VerifiedFaild;

            bool verifiedPassword = Encryption.VerifyPassword(password, selectedUser.Password);

            if (!verifiedPassword) return result;
            if (selectedUser.IsBaned)
            {
                result = VerifyUserStatus.UserIsbaned;
            }
            else
            {
                selectedUser.LastLoginDate = DateAndTime.GetDateTime();
                result = VerifyUserStatus.VerifiedSuccessfully;
            }

            return result;
        }

        #endregion

        public VerifyUserStatus VerifyUserByUserName(string userName, string password, ref string correctUserName,
            ref int userId, string ip)
        {
            User selectedUser = _users.SingleOrDefault(x => x.UserName == userName);
            var result = VerifyUserStatus.VerifiedFaild;
            if (selectedUser == null) return result;
            result = Verify(selectedUser, password);
            if (result != VerifyUserStatus.VerifiedSuccessfully) return result;
            correctUserName = selectedUser.UserName;
            userId = selectedUser.Id;
            selectedUser.IP = ip;
            return result;
        }

        public VerifyUserStatus VerifyUserByEmail(string email, string password, ref string userName, ref int userId,
            string ip)
        {
            User selectedUser = _users.SingleOrDefault(x => x.Email == email);
            var result = VerifyUserStatus.VerifiedFaild;
            if (selectedUser == null) return result;
            result = Verify(selectedUser, password);
            if (result != VerifyUserStatus.VerifiedSuccessfully) return result;
            userName = selectedUser.UserName;
            userId = selectedUser.Id;
            selectedUser.IP = ip;
            return result;
        }

        #endregion

        #region Activing and deactiving operations

        public bool IsUserActive(int id)
        {
            return !Convert.ToBoolean(_users.Find(id).IsBaned);
        }

        public void DeActiveUser(int id)
        {
            User selectedUser = _users.Find(id);
            selectedUser.IsBaned = true;
            selectedUser.BanedDate = DateAndTime.GetDateTime();
        }

        public void DeActiveUsers(int[] usersId)
        {
            foreach (User selectedUser in usersId.Select(id => _users.Find(id)))
            {
                selectedUser.IsBaned = true;
                selectedUser.BanedDate = DateAndTime.GetDateTime();
            }
        }

        public void ActiveUser(int id)
        {
            _users.Find(id).IsBaned = true;
        }

        public void ActiveUsers(int[] usersId)
        {
            foreach (int id in usersId)
            {
                _users.Find(id).IsBaned = false;
            }
        }

        #endregion

        #region Get User

        public User GetUserById(int id)
        {
            return _users.Find(id);
        }

        [CacheMethod]
        public User GetUserByUserName(string userName)
        {
            return _users.FirstOrDefault(x => x.UserName.Equals(userName));
        }

        public User GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(x => x.Email.Equals(email));
        }

        public IList<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public IList<User> GetUser(Func<User, bool> expression)
        {
            return _users.Where(expression).ToList();
        }

        public bool ExistsByUserName(string userName)
        {
            return _users.Any(user => user.UserName == userName);
        }

        public bool ExistsByEmail(string email)
        {
            return _users.Any(user => user.Email == email);
        }

        public IList<UserDataTableModel> GetDataTable(string term, int page, int count,
            Order order, UserOrderBy orderBy, UserSearchBy searchBy)
        {
            IQueryable<User> selectedUsers = _users.Include(x => x.UserMetaData).Include(x => x.Role).AsQueryable();


            if (!string.IsNullOrEmpty(term))
            {
                switch (searchBy)
                {
                    case UserSearchBy.UserName:
                        selectedUsers = selectedUsers.Where(user => user.UserName.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.RoleDescription:
                        selectedUsers = selectedUsers.Where(user => user.Role.Description.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.FirstName:
                        selectedUsers =
                            selectedUsers.Where(user => user.UserMetaData.FirstName.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.LastName:
                        selectedUsers =
                            selectedUsers.Where(user => user.UserMetaData.LastName.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.Email:
                        selectedUsers = selectedUsers.Where(user => user.Email.Contains(term)).AsQueryable();
                        break;
                    case UserSearchBy.Ip:
                        selectedUsers = selectedUsers.Where(user => user.IP.Contains(term)).AsQueryable();
                        break;
                }
            }


            if (order == Order.Asscending)
            {
                switch (orderBy)
                {
                    case UserOrderBy.UserName:
                        selectedUsers = selectedUsers.OrderBy(user => user.UserName).AsQueryable();
                        break;
                    case UserOrderBy.PostCount:
                        selectedUsers = selectedUsers.OrderBy(user => user.Posts.Count).AsQueryable();
                        break;
                    case UserOrderBy.CommentCount:
                        selectedUsers = selectedUsers.OrderBy(user => user.Comments.Count).AsQueryable();
                        break;
                    case UserOrderBy.RegisterDate:
                        selectedUsers = selectedUsers.OrderBy(user => user.CreatedDate).AsQueryable();
                        break;
                    case UserOrderBy.IsBaned:
                        selectedUsers = selectedUsers.OrderBy(user => user.IsBaned).AsQueryable();
                        break;
                    case UserOrderBy.LoginDate:
                        selectedUsers = selectedUsers.OrderBy(user => user.LastLoginDate).AsQueryable();
                        break;
                    case UserOrderBy.Ip:
                        selectedUsers = selectedUsers.OrderBy(user => user.LastLoginDate).AsQueryable();
                        break;
                }
            }
            else
            {
                switch (orderBy)
                {
                    case UserOrderBy.UserName:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.UserName).AsQueryable();
                        break;
                    case UserOrderBy.PostCount:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.Posts.Count).AsQueryable();
                        break;
                    case UserOrderBy.CommentCount:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.Comments.Count).AsQueryable();
                        break;
                    case UserOrderBy.RegisterDate:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.CreatedDate).AsQueryable();
                        break;
                    case UserOrderBy.IsBaned:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.IsBaned).AsQueryable();
                        break;
                    case UserOrderBy.LoginDate:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.LastLoginDate).AsQueryable();
                        break;
                    case UserOrderBy.Ip:
                        selectedUsers = selectedUsers.OrderByDescending(user => user.LastLoginDate).AsQueryable();
                        break;
                }
            }
            return selectedUsers.Skip(page * count).Take(count).Select(user =>
                new UserDataTableModel
                {
                    Email = user.Email,
                    Id = user.Id,
                    IsBaned = user.IsBaned,
                    RoleDescription = user.Role.Description,
                    UserName = user.UserName,
                    FullName = user.UserMetaData.FirstName + " " + user.UserMetaData.LastName,
                    CommentCount = user.Comments.Count,
                    PostCount = user.Posts.Count,
                    LastLoginDate = user.LastLoginDate,
                    RegisterDate = user.CreatedDate
                }).ToList();
        }


        public IList<UserDataTableModel> GetAllUsersDataTable(int page, int count, Order orderBy)
        {
            IQueryable<UserDataTableModel> selectedUsers =
                _users.Include(x => x.UserMetaData)
                    .Include(x => x.Role)
                    .Select(
                        x =>
                            new UserDataTableModel
                            {
                                Email = x.Email,
                                Id = x.Id,
                                IsBaned = x.IsBaned,
                                RoleDescription = x.Role.Description,
                                UserName = x.UserName,
                                FullName = x.UserMetaData.FirstName + " " + x.UserMetaData.LastName
                            })
                    .AsQueryable();
            selectedUsers = orderBy == Order.Asscending
                ? selectedUsers.OrderBy(x => x.Id).Skip(count * page).Take(count).AsQueryable()
                : selectedUsers.OrderByDescending(x => x.Id).Skip(count * page).Take(count).AsQueryable();

            return selectedUsers.ToList();
        }

        public UserDetailModel GetUserDetail(int id)
        {
            return
                _users.Where(x => x.Id == id)
                    .Include(x => x.UserMetaData)
                    .Include(x => x.Role)
                    .Select(
                        x =>
                            new UserDetailModel
                            {
                                AvatarPath = x.UserMetaData.AvatarPath,
                                BanedDate = x.BanedDate,
                                BirthDay = x.UserMetaData.BirthDay,
                                CreatedDate = x.CreatedDate,
                                Description = x.UserMetaData.Description,
                                Email = x.Email,
                                FullName = x.UserMetaData.FirstName + " " + x.UserMetaData.LastName,
                                Id = x.Id,
                                IP = x.IP,
                                IsBaned = x.IsBaned,
                                LastLoginDate = x.LastLoginDate,
                                LastPasswordChange = x.LastPasswordChange,
                                Major = x.UserMetaData.Major,
                                UserName = x.UserName,
                                RoleName = x.Role.Description,
                                CommentNumber = x.Comments.Count(),
                                PostNumber = x.Posts.Count()
                            })
                    .FirstOrDefault();
        }

        public int GetUsersNumber()
        {
            return _users.Count();
        }

        #endregion

        #region Change Password

        public ChangePasswordResult ChangePasswordByUserName(string username, string oldPassword, string newPassword)
        {
            User selectedUser = _users.FirstOrDefault(x => x.UserName.Equals(username));
            if (!Encryption.VerifyPassword(oldPassword, selectedUser.Password))
                return ChangePasswordResult.ChangedFaild;
            selectedUser.Password = Encryption.EncryptingPassword(newPassword);
            selectedUser.LastPasswordChange = DateAndTime.GetDateTime();
            return ChangePasswordResult.ChangedSuccessfully;
        }

        public ChangePasswordResult ChangePasswordByUserId(int Id, string oldPassword, string newPassword)
        {
            User selectedUser = _users.Find(Id);
            if (!Encryption.VerifyPassword(oldPassword, selectedUser.Password))
                return ChangePasswordResult.ChangedFaild;
            selectedUser.Password = Encryption.EncryptingPassword(newPassword);
            selectedUser.LastPasswordChange = DateAndTime.GetDateTime();
            return ChangePasswordResult.ChangedSuccessfully;
        }

        #endregion
    }
}