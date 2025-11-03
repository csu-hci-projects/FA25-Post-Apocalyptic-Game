using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5000f;

    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
