using System.Configuration;

namespace Change_Point.Infrastructure
{
    /// <summary>
    /// Centralised schema name helpers.
    /// appSettings keys:
    ///   PostgresqlSchema_ChangePoint  (default: change_point)
    ///   PostgresqlSchema_Main         (default: htmfg2t)
    /// </summary>
    public static class GlobalConfig
    {
        public static string CpSchema =>
            ConfigurationManager.AppSettings["PostgresqlSchema_ChangePoint"] ?? "change_point";

        public static string MainSchema =>
            ConfigurationManager.AppSettings["PostgresqlSchema_Main"] ?? "htmfg2t";
    }
}
