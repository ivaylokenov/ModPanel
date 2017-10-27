namespace ModPanel.App.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Admin;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly ModPanelDbContext db;

        public UserService(ModPanelDbContext db)
        {
            this.db = db;
        }

        public bool Create(string email, string password, PositionType position)
        {
            if (this.db.Users.Any(u => u.Email == email))
            {
                return false;
            }

            var isAdmin = !this.db.Users.Any();

            var user = new User
            {
                Email = email,
                Password = password,
                IsAdmin = isAdmin,
                Position = position,
                IsApproved = isAdmin
            };

            this.db.Add(user);
            this.db.SaveChanges();

            return true;
        }

        public bool UserExists(string email, string password)
            => this.db
                .Users
                .Any(u => u.Email == email && u.Password == password);

        public bool UserIsApproved(string email)
            => this.db
                .Users
                .Any(u => u.Email == email && u.IsApproved);

        public IEnumerable<AdminUserModel> All()
            => this.db
                .Users
                .ProjectTo<AdminUserModel>()
                .ToList();

        public string Approve(int id)
        {
            var user = this.db.Users.Find(id);

            if (user != null)
            {
                user.IsApproved = true;

                this.db.SaveChanges();
            }

            return user?.Email;
        }
    }
}
