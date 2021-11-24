using UnityEngine;

public class TrackPos : MonoBehaviour
{
    public Vector2 position;

    private void Update()
    {
        transform.right = position - new Vector2(transform.position.x, transform.position.y);
    }
}
