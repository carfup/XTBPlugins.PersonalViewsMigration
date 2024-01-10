using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Carfup.XTBPlugins.AppCode
{
    public class LogUsageManager
    {

        private TelemetryClient telemetry = null;
        private bool forceLog { get; set; } = false;

        private PersonalViewsMigration.PersonalViewsMigration pvm = null;
        public LogUsageManager(PersonalViewsMigration.PersonalViewsMigration pvm)
        {
            this.pvm = pvm;

            try
            {
                var configuration = new TelemetryConfiguration
                {
                    ConnectionString = CustomParameter.INSIGHTS_INTRUMENTATIONKEY
                };

                this.telemetry = new TelemetryClient(configuration);
                this.telemetry.Context.Component.Version = PersonalViewsMigration.PersonalViewsMigration.CurrentVersion;
                this.telemetry.Context.Device.Id = this.pvm.GetType().Name;
                this.telemetry.Context.User.Id = Guid.NewGuid().ToString();
            }
            catch (Exception ex)
            {
                // too bad
            }
        }

        public void UpdateForceLog()
        {
            forceLog = true;
        }

        public void LogData(string type, string action, Exception exception = null)
        {
            try
            {
                if (pvm.settings.AllowLogUsage == true || forceLog)
                {
                    switch (type)
                    {
                        case EventType.Event:
                            telemetry.TrackEvent(action, CompleteLog(action));
                            break;
                        case EventType.Dependency:
                            //this.telemetry.TrackDependency(todo);
                            break;
                        case EventType.Exception:
                            telemetry.TrackException(exception, CompleteLog(action));
                            break;
                        case EventType.Trace:
                            telemetry.TrackTrace(action, CompleteLog(action));
                            break;
                    }
                }

                if (forceLog)
                    forceLog = false;
            }
            catch (Exception ex)
            {
                // too bad
            }
            
        }

        public void Flush()
        {
            telemetry.Flush();
        }


        public Dictionary<string, string> CompleteLog(string action = null)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>
            {
                { "plugin", telemetry.Context.Device.Id },
                { "xtbversion", Assembly.GetEntryAssembly().GetName().Version.ToString() },
                { "pluginversion", PersonalViewsMigration.PersonalViewsMigration.CurrentVersion }
            };

            if (action != null)
                dictionary.Add("action", action);

            return dictionary;
        }

        internal void PromptToLog()
        {
            var msg = "Anonymous statistics will be collected to improve plugin functionalities.\n\n" +
                      "You can change this setting in plugin's options anytime.\n\n" +
                      "Thanks!";

            pvm.settings.AllowLogUsage = true;
            MessageBox.Show(msg);
        }
    }
}
