class LogRecordConverter :
    WriteOnlyJsonConverter<LogRecord>
{
    public override void Write(VerifyJsonWriter writer, LogRecord value)
    {
        writer.WriteStartObject();

        if (value.CategoryName != null)
        {
            writer.WriteMember(value, value.CategoryName, "CategoryName");
        }

        writer.WriteMember(value, value.LogLevel.ToString(), "LogLevel");

        if (value.Body != null)
        {
            writer.WriteMember(value, value.Body, "Body");
        }

        if (value.FormattedMessage != null)
        {
            writer.WriteMember(value, value.FormattedMessage, "FormattedMessage");
        }

        var eventId = value.EventId;
        if (eventId.Id != 0 || eventId.Name != null)
        {
            writer.WriteMember(value, eventId.ToString(), "EventId");
        }

        if (value.Exception != null)
        {
            writer.WriteMember(value, value.Exception, "Exception");
        }

        var attributes = value.Attributes;
        if (attributes is { Count: > 0 })
        {
            writer.WriteMember(value, attributes.ToDictionary(_ => _.Key, _ => _.Value), "Attributes");
        }

        if (value.TraceFlags != ActivityTraceFlags.None)
        {
            writer.WriteMember(value, value.TraceFlags.ToString(), "TraceFlags");
        }

        if (value.TraceState != null)
        {
            writer.WriteMember(value, value.TraceState, "TraceState");
        }

        writer.WriteEndObject();
    }
}
