namespace Lesson1.Models
{
    public enum PermissionRight
    {
        None = 0,
        View = 1,
        Create = 2,
        Update = 4,
        Delete = 8,
    }
    public enum PermissionEntity
    {
        User = 16,
        Product = 32
    }
    public class UserPermission
    {
        public int ID { get; set; }
        public PermissionRight Right { get; set; }
        public PermissionEntity Entity { get; set;}

        public int UserID { get; set; }
        public User? User { get; set; }

        public UserPermission() { }

        public UserPermission(PermissionEntity entity, PermissionRight right, int userID, User user)
        {
            Right = right;
            Entity = entity;
            UserID = userID;
            User = user;
        }
    }
}
