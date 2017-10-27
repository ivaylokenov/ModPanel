namespace ModPanel.App.Models.Posts
{
    using Data.Models;
    using Infrastructure.Mapping;
    using Infrastructure.Validation;
    using Infrastructure.Validation.Posts;

    public class PostModel : IMapFrom<Post>
    {
        [Required]
        [Title]
        public string Title { get; set; }

        [Required]
        [Content]
        public string Content { get; set; }
    }
}
