﻿namespace bgTeam.Impl.Serilog.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using global::Serilog.Events;
    using global::Serilog.Formatting.Display;
    using global::Serilog.Sinks.PeriodicBatching;
    using Google.Api;
    using Google.Cloud.Logging.Type;
    using Google.Cloud.Logging.V2;

    internal class GoogleCloudLoggingSink : PeriodicBatchingSink
    {
        private readonly GoogleCloudLoggingSinkOptions _sinkOptions;
        private readonly LoggingServiceV2Client _client;
        private readonly string _logName;
        private readonly string _logNameToWrite;
        private readonly MonitoredResource _resource;
        private readonly MessageTemplateTextFormatter _messageTemplateTextFormatter;

        // TODO 
        // Check correctness of _logNameToWrite variable, because there were some changes in google API
        // and this variable was changed to build project successfully.
        public GoogleCloudLoggingSink(GoogleCloudLoggingSinkOptions sinkOptions, MessageTemplateTextFormatter messageTemplateTextFormatter, int batchSizeLimit, TimeSpan period)
            : base(batchSizeLimit, period)
        {
            _client = LoggingServiceV2Client.Create();
            _sinkOptions = sinkOptions;

            _resource = new MonitoredResource { Type = sinkOptions.ResourceType };

            var ln = new LogName(sinkOptions.ProjectId, sinkOptions.LogName);
            _logName = ln.ToString();
            _logNameToWrite = sinkOptions.LogName;

            _messageTemplateTextFormatter = messageTemplateTextFormatter;
        }

        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var logEntries = new List<LogEntry>();

            foreach (var e in events)
            {
                var entry = new LogEntry
                {
                    LogName = _logName,
                    Severity = TranslateSeverity(e.Level),
                    Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(e.Timestamp),
                };

                if (_messageTemplateTextFormatter != null)
                {
                    using (var stringWriter = new StringWriter())
                    {
                        _messageTemplateTextFormatter.Format(e, stringWriter);
                        entry.TextPayload = stringWriter.ToString();
                    }
                }
                else
                {
                    if (e.Exception == null)
                    {
                        entry.TextPayload = e.RenderMessage();
                    }
                    else
                    {
                        entry.TextPayload = e.Exception.ToString();
                    }
                }

                foreach (var property in e.Properties)
                {
                    WriteProperty(entry, property.Key, property.Value);
                }

                logEntries.Add(entry);
            }

            await _client.WriteLogEntriesAsync(_logNameToWrite, _resource, _sinkOptions.Labels, logEntries);
        }

        /// <summary>
        /// Writes event properties as labels for GCP log entry.
        /// GCP log labels are a flat key/value namespace so all child event properties will be prefixed with parent property names "parentkey.childkey" similar to json path.
        /// Scalar and sequence values will be written even if values are empty so that key names are still logged as labels.
        /// </summary>
        private void WriteProperty(LogEntry entry, string propertyKey, LogEventPropertyValue propertyValue)
        {
            switch (propertyValue)
            {
                case ScalarValue scalarValue:
                {
                    entry.Labels.Add(propertyKey, _sinkOptions.LogName);

                    if (_sinkOptions.UseSourceContextAsLogName && propertyKey.Equals("SourceContext", StringComparison.OrdinalIgnoreCase))
                    {
                        entry.LogName = new LogName(_sinkOptions.ProjectId, _sinkOptions.LogName).ToString();
                    }

                    break;
                }

                case SequenceValue sequenceValue:
                {
                    var value = string.Join(",", sequenceValue.Elements);
                    entry.Labels.Add(propertyKey, value);

                    break;
                }

                case StructureValue structureValue when structureValue.Properties.Count > 0:
                {
                    foreach (var childProperty in structureValue.Properties)
                    {
                        WriteProperty(entry, propertyKey + "." + childProperty.Name, childProperty.Value);
                    }

                    break;
                }

                case DictionaryValue dictionaryValue when dictionaryValue.Elements.Count > 0:
                {
                    foreach (var kv in dictionaryValue.Elements)
                    {
                        WriteProperty(entry, propertyKey + "." + kv.Key.ToString().Replace("\"", string.Empty), kv.Value);
                    }

                    break;
                }
            }
        }

        private LogSeverity TranslateSeverity(LogEventLevel level)
        {
            switch (level)
            {
                case LogEventLevel.Verbose:
                case LogEventLevel.Debug: return LogSeverity.Debug;
                case LogEventLevel.Information: return LogSeverity.Info;
                case LogEventLevel.Warning: return LogSeverity.Warning;
                case LogEventLevel.Error: return LogSeverity.Error;
                case LogEventLevel.Fatal: return LogSeverity.Critical;
                default: return LogSeverity.Default;
            }
        }
    }
}
