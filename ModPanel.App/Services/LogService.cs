namespace ModPanel.App.Services
{
    using Contracts;
    using Data;
    using Data.Models;
    using Models.Logs;
    using System.Collections.Generic;
    using System.Linq;

    public class LogService : ILogService
    {
        public void Create(string admin, LogType type, string additionalInformation)
        {
            using (var db = new ModPanelDbContext())
            {
                var log = new Log
                {
                    Admin = admin,
                    Type = type,
                    AdditionalInformation = additionalInformation
                };

                db.Logs.Add(log);
                db.SaveChanges();
            }
        }

        public IEnumerable<LogModel> All()
        {
            using (var db = new ModPanelDbContext())
            {
                return db
                    .Logs
                    .OrderByDescending(l => l.Id)
                    .Select(l => new LogModel
                    {
                        Admin = l.Admin,
                        Type = l.Type,
                        AdditionalInformation = l.AdditionalInformation
                    })
                    .ToList();
            }
        }
    }
}
