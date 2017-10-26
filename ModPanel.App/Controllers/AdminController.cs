namespace ModPanel.App.Controllers
{
    using Data.Models;
    using Infrastructure;
    using Models.Posts;
    using Services;
    using Services.Contracts;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using System.Linq;

    public class AdminController : BaseController
    {
        private const string EditError = "<p>Check your form for errors.</p><p>Title must begin with uppercase letter and has length between 3 and 100 symbols (inclusive).</p><p>Content must be more than 10 symbols (inclusive).</p>";

        private readonly IUserService users;
        private readonly IPostService posts;
        private readonly ILogService logs;

        public AdminController()
        {
            this.users = new UserService();
            this.posts = new PostService();
            this.logs = new LogService();
        }

        public IActionResult Users()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var rows = this.users
                .All()
                .Select(u => $@"
                    <tr>
                        <td>{u.Id}</td>
                        <td>{u.Email}</td>
                        <td>{u.Position.ToFriendlyName()}</td>
                        <td>{u.Posts}</td>
                        <td>
                            {(u.IsApproved ? string.Empty : $@"<a class=""btn btn-primary btn-sm"" href=""/admin/approve?id={u.Id}"">Approve</a>")}
                        </td>
                    </tr>");
            
            this.ViewModel["users"] = string.Join(string.Empty, rows);

            this.Log(LogType.OpenMenu, nameof(Users));

            return this.View();
        }

        public IActionResult Approve(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var userEmail = this.users.Approve(id);

            if (userEmail != null)
            {
                this.Log(LogType.UserApproval, userEmail);
            }

            return this.Redirect("/admin/users");
        }

        public IActionResult Posts()
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var rows = this.posts
                .All()
                .Select(p => $@"
                    <tr>
                        <td>{p.Id}</td>
                        <td>{p.Title}</td>
                        <td>
                            <a class=""btn btn-warning btn-sm"" href=""/admin/edit?id={p.Id}"">Edit</a>
                            <a class=""btn btn-danger btn-sm"" href=""/admin/delete?id={p.Id}"">Delete</a>
                        </td>
                    </tr>");
            
            this.ViewModel["posts"] = string.Join(string.Empty, rows);

            this.Log(LogType.OpenMenu, nameof(Posts));

            return this.View();
        }

        public IActionResult Edit(int id)
            => this.PrepareEditAndDeleteView(id)
                ?? this.View();

        [HttpPost]
        public IActionResult Edit(int id, PostModel model)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            if (!this.IsValidModel(model))
            {
                this.ShowError(EditError);
                return this.View();
            }

            this.posts.Update(id, model.Title, model.Content);

            this.Log(LogType.EditPost, model.Title);

            return this.Redirect("/admin/posts");
        }

        public IActionResult Delete(int id)
        {
            this.ViewModel["id"] = id.ToString();
            
            return this.PrepareEditAndDeleteView(id) ?? this.View();
        }

        [HttpPost]
        public IActionResult Confirm(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var postTitle = this.posts.Delete(id);

            if (postTitle != null)
            {
                this.Log(LogType.DeletePost, postTitle);
            }

            return this.Redirect("/admin/posts");
        }

        private IActionResult PrepareEditAndDeleteView(int id)
        {
            if (!this.IsAdmin)
            {
                return this.RedirectToLogin();
            }

            var post = this.posts.GetById(id);

            if (post == null)
            {
                return this.NotFound();
            }

            this.ViewModel["title"] = post.Title;
            this.ViewModel["content"] = post.Content;

            return null;
        }

        public IActionResult Log()
        {
            this.Log(LogType.OpenMenu, nameof(Log));

            var rows = this.logs
                .All()
                .Select(l => l.ToHtml());

            this.ViewModel["logs"] = string.Join(string.Empty, rows);

            return this.View();
        }
    }
}
