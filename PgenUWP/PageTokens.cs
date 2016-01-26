using PgenUWP.Views;

namespace PgenUWP
{
    public static class PageTokens
    {
        public static readonly string Services = GetToken(nameof(ServicesPage));
        public static readonly string GeneratePassword = GetToken(nameof(GeneratePasswordPage));
        public static readonly string AddService = GetToken(nameof(AddServicePage));

        private static string GetToken(string pageTypeName)
        {
            return pageTypeName.Replace(PageSuffix, string.Empty);
        }

        private const string PageSuffix = "Page";
    }
}
