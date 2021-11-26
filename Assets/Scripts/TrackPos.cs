using UnityEngine;

public class TrackPos : MonoBehaviour
{
    public Vector2 position;

    private void OnEnable()
    {
        trackPos(); //This is done to stop a very small visual glitch
    }

    private void Update()
    {
        trackPos();
    }

    private void trackPos()
    {
        transform.right = position - new Vector2(transform.position.x, transform.position.y);
    }
}
