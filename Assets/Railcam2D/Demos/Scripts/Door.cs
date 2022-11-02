using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen;
    public void Open()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            transform.position = new Vector3(transform.position.x, transform.position.y + 5.5f, transform.position.z);
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            IsOpen = false;
            transform.position = new Vector3(transform.position.x, transform.position.y - 5.5f, transform.position.z);
        }
    }
}
