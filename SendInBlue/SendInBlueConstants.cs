namespace SendInBlue
{
    public static class SendInBlueConstants
    {
        public const string ApiKey = "api-key";
        public const string MaKey = "ma-key";

        public static class ApiEndpoints
        {
            public const string AutomationEndpointAddress = "https://in-automate.sendinblue.com/api/v2";
            public const string UserEndpointAddress = "https://api.sendinblue.com/v2.0/user";
        }

        public static class MediaTypes
        {
            ///<summary>JavaScript Object Notation JSON; Defined in RFC 4627.</summary>
            public const string ApplicationJson = "application/json";
        }
    }
}
