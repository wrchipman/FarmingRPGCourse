using UnityEngine;
using Cinemachine;

public class SwitchConfineBoundingShape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SwitchBoundingShape();
    }
    /// <summary>
    /// Switch the collider that cinemachine uses to define the edges of the screen
    /// </summary>
    private void SwitchBoundingShape() 
    {
        // Get the polygon collider on the  'boundsconfiner' gameobject which is used by cinemachine to prevent the camera going beyond the screen edge
        PolygonCollider2D polygon2d = GameObject.FindGameObjectWithTag(Tags.BoundsConfiner).GetComponent<PolygonCollider2D>();

        CinemachineConfiner cinemachineConfiner = GetComponent<CinemachineConfiner>();

        cinemachineConfiner.m_BoundingShape2D = polygon2d;

        // since the confiner bounds have changed need to call this to clear the cache

        cinemachineConfiner.InvalidatePathCache();
    }
}
