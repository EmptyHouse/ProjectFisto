

public static class EHTime
{
    private const float TimePerFrame = 1f / 60f;
    public static float TimeScale
    {
        get => timeScale;
        set
        {
            timeScale = value;
            deltaTime = timeScale * TimePerFrame;
        }
    }
    private static float timeScale = 1;
    public static float DeltaTime => deltaTime;
    private static float deltaTime = TimePerFrame;

    
}