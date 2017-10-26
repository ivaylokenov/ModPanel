namespace ModPanel.App.Services.Contracts
{
    using Models.Home;
    using Models.Posts;
    using System.Collections.Generic;

    public interface IPostService
    {
        void Create(string title, string content, int userId);

        IEnumerable<PostListingModel> All();

        IEnumerable<HomeListingModel> AllWithDetails(string search = null);

        PostModel GetById(int id);

        void Update(int id, string title, string content);

        string Delete(int id);
    }
}
