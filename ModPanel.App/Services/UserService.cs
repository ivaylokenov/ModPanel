namespace ModPanel.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        public bool Create(string email, string password, PositionType position)
        {
            using (var db = new ModPanelDbContext())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    return false;
                }

                var isAdmin = !db.Users.Any();

                var user = new User
                {
                    Email = email,
                    Password = password,
                    IsAdmin = isAdmin,
                    Position = position,
                    IsApproved = isAdmin
                };

                db.Add(user);
                db.SaveChanges();

                return true;
            }
        }

        public bool UserExists(string email, string password)
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Users
                    .Any(u => u.Email == email && u.Password == password);
            }
        }

        public bool UserIsApproved(string email)
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Users
                    .Any(u => u.Email == email && u.IsApproved);
            }
        }

        public IEnumerable<AdminUserModel> All()
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Users
                    .Select(u => new AdminUserModel
                    {
                        Id = u.Id,
                        Email = u.Email,
                        Position = u.Position,
                        IsApproved = u.IsApproved,
                        Posts = u.Posts.Count
                    })
                    .ToList();
            }
        }

        public string Approve(int id)
        {
            using (var db = new ModPanelDbContext())
            {
                var user = db.Users.Find(id);

                if (user != null)
                {
                    user.IsApproved = true;

                    db.SaveChanges();
                }

                return user?.Email;
            }
        }
    }
}
