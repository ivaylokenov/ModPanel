namespace ModPanel.App.Services
{
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
        public void Create(string title, string content, int userId)
        {
            using (var db = new ModPanelDbContext())
            {
                var post = new Post
                {
                    Title = title.Capitalize(),
                    Content = content,
                    UserId = userId,
                    CreatedOn = DateTime.UtcNow
                };

                db.Posts.Add(post);
                db.SaveChanges();
            }
        }

        public IEnumerable<PostListingModel> All()
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Posts
                    .Select(p => new PostListingModel
                    {
                        Id = p.Id,
                        Title = p.Title
                    })
                    .ToList();
            }
        }

        public IEnumerable<HomeListingModel> AllWithDetails(string search = null)
        {
            using (var db = new ModPanelDbContext())
            {
                var query = db.Posts.AsQueryable();

                if (!string.IsNullOrWhiteSpace(search))
                {
                    query = query.Where(p => p.Title.ToLower().Contains(search.ToLower()));
                }

                return query
                    .OrderByDescending(p => p.Id)
                    .Select(p => new HomeListingModel
                    {
                        Title = p.Title,
                        Content = p.Content,
                        CreatedBy = p.User.Email,
                        CreatedOn = p.CreatedOn
                    })
                    .ToList();
            }
        }

        public PostModel GetById(int id)
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Posts
                    .Where(p => p.Id == id)
                    .Select(p => new PostModel
                    {
                        Title = p.Title,
                        Content = p.Content
                    })
                    .FirstOrDefault();
            }
        }

        public void Update(int id, string title, string content)
        {
            using (var db = new ModPanelDbContext())
            {
                var post = db.Posts.Find(id);

                if (post == null)
                {
                    return;
                }

                post.Title = title.Capitalize();
                post.Content = content;

                db.SaveChanges();
            }
        }

        public string Delete(int id)
        {
            using (var db = new ModPanelDbContext())
            {
                var post = db.Posts.Find(id);

                if (post == null)
                {
                    return null;
                }

                db.Posts.Remove(post);
                db.SaveChanges();

                return post.Title;
            }
        }
    }
}
