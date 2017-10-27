namespace ModPanel.App.Services
{
    using AutoMapper.QueryableExtensions;
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Logs;
    using System.Collections.Generic;
    using System.Linq;

    public class LogService : ILogService
    {
        private readonly ModPanelDbContext db;

        public LogService(ModPanelDbContext db)
        {
            this.db = db;
        }

        public void Create(string admin, LogType type, string additionalInformation)
        {
            var log = new Log
            {
                Admin = admin,
                Type = type,
                AdditionalInformation = additionalInformation
            };

            this.db.Logs.Add(log);
            this.db.SaveChanges();
        }

        public IEnumerable<LogModel> All()
            => this.db
                .Logs
                .OrderByDescending(l => l.Id)
                .ProjectTo<LogModel>()
                .ToList();
    }
}
