using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public int xSpacing;
    public int ySpacing;
    // Update is called once per frame
    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.x += xSpacing;
        mousePosition.y += ySpacing;    

        // Convert the screen position to a world position
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        // Set the object's position to the mouse's world position
        transform.position = mousePosition;
    }
}
