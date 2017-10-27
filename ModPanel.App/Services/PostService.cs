namespace ModPanel.App.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Infrastructure;
    using Models.Home;
    using Models.Posts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostService : IPostService
    {
        private readonly ModPanelDbContext db;

        public PostService(ModPanelDbContext db)
        {
            this.db = db;
        }

        public void Create(string title, string content, int userId)
        {
            var post = new Post
            {
                Title = title.Capitalize(),
                Content = content,
                UserId = userId,
                CreatedOn = DateTime.UtcNow
            };

            this.db.Posts.Add(post);
            this.db.SaveChanges();
        }

        public IEnumerable<PostListingModel> All()
            => this.db
                .Posts
                .ProjectTo<PostListingModel>()
                .ToList();

        public IEnumerable<HomeListingModel> AllWithDetails(string search = null)
        {
            var query = this.db.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p => p.Title.ToLower().Contains(search.ToLower()));
            }

            return query
                .OrderByDescending(p => p.Id)
                .ProjectTo<HomeListingModel>()
                .ToList();
        }

        public PostModel GetById(int id)
            => this.db
                .Posts
                .Where(p => p.Id == id)
                .ProjectTo<PostModel>()
                .FirstOrDefault();

        public void Update(int id, string title, string content)
        {
            var post = db.Posts.Find(id);

            if (post == null)
            {
                return;
            }

            post.Title = title.Capitalize();
            post.Content = content;

            this.db.SaveChanges();
        }

        public string Delete(int id)
        {
            var post = db.Posts.Find(id);

            if (post == null)
            {
                return null;
            }

            this.db.Posts.Remove(post);
            this.db.SaveChanges();

            return post.Title;
        }
    }
}
