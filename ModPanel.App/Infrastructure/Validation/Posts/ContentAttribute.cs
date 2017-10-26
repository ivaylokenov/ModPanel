namespace ModPanel.App.Infrastructure.Validation.Posts
{
    using SimpleMvc.Framework.Attributes.Validation;

    public class ContentAttribute : PropertyValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var content = value as string;
            if (content == null)
            {
                return true;
            }

            return content.Length >= 10;
        }
    }
}
