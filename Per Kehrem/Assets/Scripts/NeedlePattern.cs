using UnityEngine;
using System.Collections;

public class NeedlePattern : MonoBehaviour, IAttackPattern
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject needlePrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Pattern Lifetime")]
    [SerializeField] private float patternDuration = 4f;

    public IEnumerator Execute()
    {
        foreach (Transform p in spawnPoints)
        {
            Instantiate(needlePrefab, p.position, p.rotation);
        }

        yield return new WaitForSeconds(patternDuration);
    }
}

