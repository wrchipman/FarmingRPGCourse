using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "so_LightingSchedule_", menuName = "Scriptable Objects/Lighting/LightingSchedule")]
public class SO_LightingSchedule : ScriptableObject
{
    public LightingBrightness[] lightingBrightnessArray;
}

[System.Serializable]
public struct LightingBrightness
{
    public Season season;
    public int hour;
    public float lightIntensity;
}
