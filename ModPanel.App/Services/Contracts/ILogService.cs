namespace ModPanel.App.Services.Contracts
{
    using Data.Models;
    using Models.Logs;
    using System.Collections.Generic;

    public interface ILogService
    {
        void Create(string admin, LogType type, string additionalInformation);

        IEnumerable<LogModel> All();
    }
}
