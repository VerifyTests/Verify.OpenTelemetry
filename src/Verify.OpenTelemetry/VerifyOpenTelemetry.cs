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

        ActivitySource.AddActivityListener(listener);
        VerifierSettings.AddExtraSettings(settings =>
        {
            var converters = settings.Converters;
            converters.Add(new ActivityConverter());
            converters.Add(new ActivityEventConverter());
            converters.Add(new ActivityLinkConverter());
            converters.Add(new ActivityContextConverter());
            converters.Add(new LogRecordConverter());
        });
    }
}
