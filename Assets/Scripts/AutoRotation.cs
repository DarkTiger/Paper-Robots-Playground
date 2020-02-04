using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(transform.InverseTransformDirection(transform.up) * 60 * Time.deltaTime);
    }
}
