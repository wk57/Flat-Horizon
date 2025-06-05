using UnityEngine;
using UnityEngine.UIElements;

public class SawSpin : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed;

    private void Update()
    {
        transform.Rotate(0,0, -rotationSpeed);
    }


}
