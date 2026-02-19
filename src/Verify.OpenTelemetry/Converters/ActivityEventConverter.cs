class ActivityEventConverter :
    WriteOnlyJsonConverter<ActivityEvent>
{
    public override void Write(VerifyJsonWriter writer, ActivityEvent value)
    {
        writer.WriteStartObject();
        writer.WriteMember(value, value.Name, "Name");

        var tags = value.Tags.ToList();
        if (tags.Count != 0)
        {
            writer.WriteMember(value, tags.ToDictionary(_ => _.Key, _ => _.Value), "Tags");
        }

        writer.WriteEndObject();
    }
}
