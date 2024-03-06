using System.ComponentModel.DataAnnotations;
using System.Net;
using Lesson1.Middleware;

namespace Lesson1.Models
{
    public enum UserRole { Client, Registered, Manager, Superviser, Admin }
    public class User
    {
        [Key]
        public int ID { get; set; }
        public string? Login { get; set; }
        public string? Password { get; set; }
        public UserRole Role { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Tel { get; set; }
        public string? Email { get; set; }


        public List<UserPermission>? UserPermissions { get; set; }

        public User() {
            UserPermissions = new List<UserPermission>();
        }

        public User(string login, string password, string firstName, string lastName, string tel, string email)
        {
            Login = login;
            Password = password;
            Role = UserRole.Registered;
            FirstName = firstName;
            LastName = lastName;
            Tel = tel;
            Email = email;
            UserPermissions = new List<UserPermission>();
        }

        public void Init()
        {
            if (UserPermissions == null) 
                UserPermissions = new List<UserPermission>();

            UserPermissions.Clear();

            if(Role == UserRole.Client)
                InitClient();
            if(Role == UserRole.Registered)
                InitRegistered();
            if(Role == UserRole.Manager)
                InitManager();
            if(Role == UserRole.Superviser)
                InitSuperViser();
            if(Role == UserRole.Admin)
                InitAdmin();
        }

        private void InitClient()
        {
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.View, ID, this));
        }

        private void InitRegistered()
        {
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.View, ID, this));
        }

        private void InitManager()
        {
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.View, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Update, ID, this));
        }

        private void InitSuperViser()
        {
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.View, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Update, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Create, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Delete, ID, this));

            UserPermissions.Add(new UserPermission(PermissionEntity.User, PermissionRight.View, ID, this));          
        }

        private void InitAdmin()
        {
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.View, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Update, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Create, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.Product, PermissionRight.Delete, ID, this));

            UserPermissions.Add(new UserPermission(PermissionEntity.User, PermissionRight.View, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.User, PermissionRight.Update, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.User, PermissionRight.Create, ID, this));
            UserPermissions.Add(new UserPermission(PermissionEntity.User, PermissionRight.Delete, ID, this));
        }

        public bool HasPermission(PermissionEntity entity, PermissionRight right)
        {
            foreach(var permission in UserPermissions)
            {
                if(permission.Right == right && permission.Entity == entity)
                    return true;
            }
            return false;
        }
    }
}
