class ActivityConverter :
    WriteOnlyJsonConverter<Activity>
{
    public override void Write(VerifyJsonWriter writer, Activity activity)
    {
        writer.WriteStartObject();
        writer.WritePropertyName(activity.OperationName);
        writer.WriteStartObject();

        if (activity.DisplayName != activity.OperationName)
        {
            writer.WriteMember(activity, activity.DisplayName, "DisplayName");
        }

        if (activity.Kind != ActivityKind.Internal)
        {
            writer.WriteMember(activity, activity.Kind.ToString(), "Kind");
        }

        if (activity.Status != ActivityStatusCode.Unset)
        {
            writer.WriteMember(activity, activity.Status.ToString(), "Status");
            writer.WriteMember(activity, activity.StatusDescription, "StatusDescription");
        }

        var tags = activity.TagObjects.ToList();
        if (tags.Count != 0)
        {
            writer.WriteMember(activity, tags.ToDictionary(_ => _.Key, _ => _.Value), "Tags");
        }

        var events = activity.Events.ToList();
        if (events.Count != 0)
        {
            writer.WriteMember(activity, events, "Events");
        }

        var links = activity.Links.ToList();
        if (links.Count != 0)
        {
            writer.WriteMember(activity, links, "Links");
        }

        var baggage = activity.Baggage.ToList();
        if (baggage.Count != 0)
        {
            writer.WriteMember(activity, baggage.ToDictionary(_ => _.Key, _ => _.Value), "Baggage");
        }

        if (activity.HasRemoteParent)
        {
            writer.WriteMember(activity, true, "HasRemoteParent");
        }

        if (activity.ParentId != null)
        {
            writer.WriteMember(activity, true, "HasParent");
        }

        writer.WriteEndObject();
        writer.WriteEndObject();
    }
}
