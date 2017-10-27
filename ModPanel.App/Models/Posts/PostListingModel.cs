namespace ModPanel.App.Models.Posts
{
    using Data.Models;
    using Infrastructure.Mapping;

    public class PostListingModel : IMapFrom<Post>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
