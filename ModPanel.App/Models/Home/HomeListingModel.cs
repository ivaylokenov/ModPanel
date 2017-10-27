namespace ModPanel.App.Models.Home
{
    using Data.Models;
    using Infrastructure.Mapping;
    using System;
    using AutoMapper;

    public class HomeListingModel : IMapFrom<Post>, IHaveCustomMapping
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public void Configure(IMapperConfigurationExpression config)
        {
            config
                .CreateMap<Post, HomeListingModel>()
                .ForMember(hl => hl.CreatedBy, cfg => cfg.MapFrom(p => p.User.Email));
        }
    }
}
