using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Carfup.XTBPlugins.AppCode
{
    public class PluginSettings
    {
        public bool? AllowLogUsage { get; set; }
        public string CurrentVersion { get; set; } = PersonalViewsMigration.PersonalViewsMigration.CurrentVersion;
        public bool UsersDisplayAll { get; set; } = true;
        public bool UsersDisplayEnabled { get; set; } = true;
        public bool UsersDisplayDisabled { get; set; } = true;
        public SortOrder? SortOrderPref { get; set; } = SortOrder.Ascending;
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

    static class UserDataType
    {
        public const string Views = "views";
        public const string Dashboards = "dashboards";
        public const string Charts = "charts";
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
        public const string EnvironmentOnPremise = "EnvironmentOnPremise";
        public const string UsersLoaded = "UsersLoaded";
        public const string UserViewsLoaded = "UserViewsLoaded";
        public const string UserChartsLoaded = "UserChartsLoaded";
        public const string UserDashboardsLoaded = "UserDashboardsLoaded";
        public const string ViewsCopied = "ViewsCopied";
        public const string ViewsDeleted = "ViewsDeleted";
        public const string ViewsReAssigned = "ViewsReAssigned";
        public const string ShowHelpScreen = "ShowHelpScreen";
    }
}
