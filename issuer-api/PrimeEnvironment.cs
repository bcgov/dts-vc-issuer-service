using System;

namespace Prime
{
    public static class PrimeEnvironment
    {
        public readonly static string Name = Environment.GetEnvironmentVariable("APP") ?? "local";
        public readonly static string FrontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "localhost:4200";
        public readonly static string BackendUrl = Environment.GetEnvironmentVariable("BACKEND_URL") ?? "http://localhost:5000/api";
        public readonly static string LogFile = Environment.GetEnvironmentVariable("LOG_FILE_PATH") ?? "logs";

        public static bool IsProduction { get => Name == "prod"; }
        public static bool IsLocal { get => Name == "local"; }

        public static class Postgres
        {
            public readonly static string Server = System.Environment.GetEnvironmentVariable("POSTGRES_SERVER");
            public readonly static string Username = System.Environment.GetEnvironmentVariable("POSTGRES_USERNAME");
            public readonly static string Db = System.Environment.GetEnvironmentVariable("POSTGRES_DB");
            public readonly static string PgPassword = System.Environment.GetEnvironmentVariable("PGPASSWORD");
            public readonly static string ConnectionString = $"Server={Server}; User Id={Username}; Database=${Db}; Port=5432; Password={PgPassword}; SSLMode=Prefer";
        }

        /// <summary>
        /// Aries Prime Agent
        /// </summary>
        public static class VerifiableCredentialApi
        {
            public static readonly string Url = Environment.GetEnvironmentVariable("VERIFIABLE_CREDENTIAL_API_URL") ?? "https://prime-agent-admin-dev.pathfinder.gov.bc.ca/";
            public static readonly string Key = Environment.GetEnvironmentVariable("VERIFIABLE_CREDENTIAL_API_KEY") ?? "P8ZmRJ05biXGWI1/bDtXcp1pixtWdsAqhcUJcn4S7QQ=";
            public static readonly string WebhookKey = Environment.GetEnvironmentVariable("VERIFIABLE_CREDENTIAL_WEBHOOK_KEY") ?? "0ce755d5-1fb1-483a-ba22-439061aa8f67";
        }
    }
}
