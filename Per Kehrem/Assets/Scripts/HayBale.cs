using UnityEngine;

public class HayBale : MonoBehaviour
{
    public float speed = 50f;
    public float rotationSpeed = 50f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime, Space.Self);
    }
}