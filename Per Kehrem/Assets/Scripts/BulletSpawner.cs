using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform spawnPoint;
    public PhaseManager phaseManager;

    void Start(){
        SpawnBullet();
    }

    public void SpawnBullet()
    {
        Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
