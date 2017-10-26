namespace ModPanel.App.Services.Contracts
{
    using Data.Models;
    using Models.Admin;
    using System.Collections.Generic;

    public interface IUserService
    {
        bool Create(string email, string password, PositionType position);

        bool UserExists(string email, string password);

        bool UserIsApproved(string email);

        IEnumerable<AdminUserModel> All();

        string Approve(int id);
    }
}
