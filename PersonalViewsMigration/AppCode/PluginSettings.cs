using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carfup.XTBPlugins.AppCode
{
    public class PluginSettings
    {
        public bool? AllowLogUsage { get; set; }
        public string CurrentVersion { get; set; } = PersonalViewsMigration.PersonalViewsMigration.CurrentVersion;
        public bool UsersDisplayAll { get; set; } = true;
        public bool UsersDisplayEnabled { get; set; } = true;
        public bool UsersDisplayDisabled { get; set; } = true;
    }

    // EventType to qualify which type of telemetry we send
    static class EventType
    {
        public const string Event = "event";
        public const string Trace = "trace";
        public const string Dependency = "dependency";
        public const string Exception = "exception";
    }

    public static class CustomParameter
    {
        public static string INSIGHTS_INTRUMENTATIONKEY = "INSIGHTS_INTRUMENTATIONKEY_TOREPLACE";
    }

    // EventType to qualify which action was performed by the plugin
    static class LogAction
    {
        public const string PluginClosed = "PluginClosed";
        public const string StatsAccepted = "StatsAccepted";
        public const string StatsDenied = "StatsDenied";
        public const string SettingsSaved = "SettingsSaved";
        public const string SettingsSavedWhenClosing = "SettingsSavedWhenClosing";
        public const string SettingLoaded = "SettingLoaded";
        public const string SolutionsCompared = "SolutionsCompared";
        public const string CRMAssembliesLoaded = "CRMAssembliesLoaded";
        public const string AssemblyLoaded = "AssemblyLoaded";
        public const string PluginsLoaded = "PluginsLoaded";
        public const string PluginsCompared = "PluginsCompared";
        public const string EnvironmentOnPremise = "EnvironmentOnPremise";
        public const string UsersLoaded = "UsersLoaded";
        public const string ViewsCopied = "ViewsCopied";
        public const string ViewsDeleted = "ViewsDeleted";
        public const string ViewsReAssigned = "ViewsReAssigned";
    }
}
