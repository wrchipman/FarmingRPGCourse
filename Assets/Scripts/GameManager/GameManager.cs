using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager> 
{
    protected override void Awake()
    {
        base.Awake();

        // TODO: Need a resolution setting option screen
        // TODO: Figure out Refreshrate setting
        Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);
    }
}
