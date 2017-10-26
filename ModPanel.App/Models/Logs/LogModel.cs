namespace ModPanel.App.Models.Logs
{
    using Data.Models;
    using System;

    public class LogModel
    {
        public string Admin { get; set; }

        public LogType Type { get; set; }

        public string AdditionalInformation { get; set; }

        public override string ToString()
        {
            string message = null;

            switch (this.Type)
            {
                case LogType.CreatePost:
                    message = $"created the post {this.AdditionalInformation}";
                    break;
                case LogType.EditPost:
                    message = $"edited the post {this.AdditionalInformation}";
                    break;
                case LogType.DeletePost:
                    message = $"deleted the post {this.AdditionalInformation}";
                    break;
                case LogType.UserApproval:
                    message = $"approved the registration of {this.AdditionalInformation}";
                    break;
                case LogType.OpenMenu:
                    message = $"opened {this.AdditionalInformation} menu";
                    break;
                default:
                    throw new InvalidOperationException($"Invalid log type: {this.Type}.");
            }

            return $"{this.Admin} {message}";
        }
    }
}
