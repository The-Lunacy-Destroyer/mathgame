using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;

    void Update()
    {
        if (player)
        {
            transform.position = new Vector3(
                player.transform.position.x, 
                player.transform.position.y,
                transform.position.z
            );
        }
    }
}
