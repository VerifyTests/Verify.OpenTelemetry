namespace VerifyTests;

public static class VerifyOpenTelemetry
{
    static ActivityListener? listener;
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();

        listener = new()
        {
            ShouldListenTo = _ => true,
            Sample = (ref _) =>
                ActivitySamplingResult.AllDataAndRecorded,
            ActivityStopped = activity => Recording.TryAdd("activity", activity)
        };

        var activityConverter = new ActivityConverter();
        var activityEventConverter = new ActivityEventConverter();
        var activityLinkConverter = new ActivityLinkConverter();
        var activityContextConverter = new ActivityContextConverter();
        var logRecordConverter = new LogRecordConverter();

        ActivitySource.AddActivityListener(listener);
        VerifierSettings.AddExtraSettings(_ =>
            _.Converters.AddRange(
                activityConverter,
                activityEventConverter,
                activityLinkConverter,
                activityContextConverter,
                logRecordConverter));
    }
}