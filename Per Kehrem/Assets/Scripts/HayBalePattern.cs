using UnityEngine;
using System.Collections;

public class HayBalePattern : MonoBehaviour, IAttackPattern
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject hayBalePrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Pattern Lifetime")]
    [SerializeField] private float patternDuration = 4f;

    public IEnumerator Execute()
    {

        foreach (Transform p in spawnPoints)
        {
            Instantiate(hayBalePrefab, p.position, p.rotation);
        }

        yield return new WaitForSeconds(patternDuration);
    }
}

