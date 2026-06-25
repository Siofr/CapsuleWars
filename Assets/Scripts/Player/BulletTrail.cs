using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    private float trailDuration;
    private LineRenderer lineRender;

    private void Awake()
    {
        lineRender = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (trailDuration > 0)
        {
            trailDuration -= Time.deltaTime;
            lineRender.enabled = true;
        }
        else
        {
            lineRender.enabled = false;
        }
    }

    public void DrawBulletTrail(Vector3 startPosition, Vector3 endPosition, float duration)
    {
        lineRender.SetPosition(0, startPosition);
        lineRender.SetPosition(1, endPosition);
        trailDuration = duration;
    }
}
