using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    private static float factor;
    private static bool slowMotionEnabled=false;

    public static void StartSlowMotion(float slowDownFactor)
    {
        factor = slowDownFactor;
        Time.timeScale = 1f/slowDownFactor;
        Time.fixedDeltaTime /= slowDownFactor;
        slowMotionEnabled = true;
    }
    public static void StopSlowMotion()
    {
        if (slowMotionEnabled)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime *= factor;
            slowMotionEnabled = false;
        }
    }
}
