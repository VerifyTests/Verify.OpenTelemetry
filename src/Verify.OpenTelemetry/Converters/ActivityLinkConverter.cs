class ActivityLinkConverter :
    WriteOnlyJsonConverter<ActivityLink>
{
    public override void Write(VerifyJsonWriter writer, ActivityLink value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.Context, "Context");

        var tags = value.Tags?.ToList();
        if (tags is { Count: > 0 })
        {
            writer.WriteMember(value, tags.ToDictionary(_ => _.Key, _ => _.Value), "Tags");
        }

        writer.WriteEndObject();
    }
}
