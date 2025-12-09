using UnityEngine;

public class RottenFleshObject : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float lifetime = 3f;

    private Vector3 moveDirection = Vector3.down;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime, Space.Self);
    }

    public void SetDirection(Vector3 dir)
    {
        moveDirection = dir.normalized;
    }
}


