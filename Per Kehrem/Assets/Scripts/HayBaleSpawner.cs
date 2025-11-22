using UnityEngine;

public class HayBaleSpawner : MonoBehaviour
{
    public GameObject hayBalePrefab;
    public Transform spawnPoint;
    public PhaseManager phaseManager;

    void Start(){
        SpawnBale();
    }

    public void SpawnBale()
    {
        Instantiate(hayBalePrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
