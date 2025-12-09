using UnityEngine;
using System.Collections;

public class RottenFleshPattern : MonoBehaviour, IAttackPattern
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject rottenFleshPrefab;
    [SerializeField] private Transform[] spawnPoints;

    [Header("Pattern Lifetime")]
    [SerializeField] private float patternDuration = 4f;

    public IEnumerator Execute()
    {

        Vector3[] directions = new Vector3[]
        {
            Vector3.down,
            new Vector3(-1f, -1f, 0f),
            new Vector3(1f, -1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3(1f, 1f, 0f)
        };

        // Spawn each RottenFlesh with its direction
        for (int i = 0; i < spawnPoints.Length && i < directions.Length; i++)
        {
            GameObject flesh = Instantiate(rottenFleshPrefab, spawnPoints[i].position, Quaternion.identity);
            flesh.GetComponent<RottenFleshObject>().SetDirection(directions[i]);
        }

        yield return new WaitForSeconds(patternDuration);
    }
}


