using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager> 
{
    public Weather currentWeather;

    protected override void Awake()
    {
        base.Awake();

        // TODO: Need a resolution setting option screen
        // TODO: Figure out Refreshrate setting
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);

        // Set starting weather
        currentWeather = Weather.dry;
    }
}
