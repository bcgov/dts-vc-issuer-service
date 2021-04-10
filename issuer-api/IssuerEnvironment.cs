using System;

namespace Issuer
{
    public static class IssuerEnvironment
    {
        public static readonly string Name = Environment.GetEnvironmentVariable("APP") ?? "local";
        public static readonly string FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "localhost:4200";
        public static readonly string BackendUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? "http://localhost:5000/api";
        public static readonly string LogFile = Environment.GetEnvironmentVariable("LOG_FILE_PATH") ?? "logs";

        public static bool IsProduction { get => Name == "prod"; }
        public static bool IsLocal { get => Name == "local"; }

        public static class Postgres
        {
            public static readonly string Server = System.Environment.GetEnvironmentVariable("POSTGRES_SERVER");
            public static readonly string Username = System.Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
            public static readonly string Db = System.Environment.GetEnvironmentVariable("POSTGRES_DB");
            public static readonly string PgPassword = System.Environment.GetEnvironmentVariable("PGPASSWORD");
            public static readonly string ConnectionString = System.Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
        }

        /// <summary>
        /// Aries Prime Agent
        /// </summary>
        public static class VerifiableCredentialApi
        {
            public static readonly string Url =  "http://agent:8024/";
            public static readonly string Key = Environment.GetEnvironmentVariable("ISSUER_AGENT_ADMIN_API_KEY");
        }

        /// <summary>
        /// Immunization Api
        /// </summary>
        public static class ImmunizationApi
        {
            public static readonly string Url = Environment.GetEnvironmentVariable("IMMUNIZATION_API_URL") ?? "https://test.ca";
        }
    }
}
