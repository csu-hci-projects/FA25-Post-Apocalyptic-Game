using UnityEngine;

public class HayBale : MonoBehaviour
{
    [SerializeField] private float speed = 5000f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime, Space.Self);
    }
}
