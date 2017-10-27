namespace ModPanel.App.Models.Admin
{
    using AutoMapper;
    using Data.Models;
    using Infrastructure.Mapping;

    public class AdminUserModel : IMapFrom<User>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public PositionType Position { get; set; }

        public int Posts { get; set; }

        public bool IsApproved { get; set; }

        public void Configure(IMapperConfigurationExpression config)
        {
            config
                .CreateMap<User, AdminUserModel>()
                .ForMember(au => au.Posts, cfg => cfg.MapFrom(u => u.Posts.Count));
        }
    }
}
