using Argon;

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

        JsonConverter[] converters = [
            new ActivityConverter(),
            new ActivityEventConverter(),
            new ActivityLinkConverter(),
            new ActivityContextConverter(),
            new LogRecordConverter()
        ];

        ActivitySource.AddActivityListener(listener);
        VerifierSettings.AddExtraSettings(_ =>
            _.Converters.AddRange(converters));
    }
}