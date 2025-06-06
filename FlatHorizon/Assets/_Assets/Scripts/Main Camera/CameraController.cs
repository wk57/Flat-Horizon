using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // 
    public float offsetX = 3f; //Desplazamiento horizontal de la cámara respecto al jugador

    void Update()
    {
        if (player != null)
        {
            // La cámara se mueve solo en el eje X con el jugador
            transform.position = new Vector3(player.position.x + offsetX, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(0,0,0);
        }
    }
}
