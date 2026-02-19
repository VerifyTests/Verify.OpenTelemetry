class ActivityContextConverter :
    WriteOnlyJsonConverter<ActivityContext>
{
    public override void Write(VerifyJsonWriter writer, ActivityContext value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.TraceFlags.ToString(), "TraceFlags");

        if (value.TraceState != null)
        {
            writer.WriteMember(value, value.TraceState, "TraceState");
        }

        writer.WriteMember(value, value.IsRemote, "IsRemote");
        writer.WriteEndObject();
    }
}
