using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns enemies around the player in a circle-ish pattern (not perfectly uniform).
/// Supports multiple enemy prefabs with weights (probabilities).
/// Works for 2D (uses Physics2D). Convert checks to Physics.* if using 3D.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyEntry
    {
        public string label;
        public GameObject prefab;
        [Tooltip("Relative weight used for weighted random selection. Higher = more likely.")]
        public float weight = 1f;
    }

    [Header("Enemy types (weighted probabilities)")]
    public EnemyEntry[] enemyTypes;

    [Header("Spawn placement")]
    [Tooltip("Transform of the player. If null, the spawner will try to find the GameObject tagged 'Player' at Start().")]
    public Transform player;
    [Tooltip("Parent for spawned enemies (optional).")]
    public Transform spawnParent;

    public float spawnRadiusMin = 6f;
    public float spawnRadiusMax = 12f;

    [Header("Wave settings")]
    public int spawnPerWave = 8;
    public float waveInterval = 8f;
    public bool spawnOnStart = true;
    public int maxConcurrentEnemies = 50; // safety cap

    [Header("Non-uniform placement / clustering")]
    [Tooltip("Angle jitter applied to each evenly spaced segment (degrees).")]
    public float angleJitter = 20f;
    [Tooltip("Enable clustering for more natural groupings.")]
    public bool useClustering = false;
    [Tooltip("How many clusters to create when clustering is enabled.")]
    public int clusterCount = 3;
    [Tooltip("How wide (degrees) each cluster spreads.")]
    public float clusterSpread = 60f;

    [Header("Collision / obstacle checks")]
    [Tooltip("Layer mask used to check if the spawn point is clear (walls, ground blocked layers, etc).")]
    public LayerMask obstacleMask;
    [Tooltip("Radius used for overlap checks to avoid spawning inside obstacles or other objects.")]
    public float spawnClearCheckRadius = 0.5f;
    [Tooltip("How many attempts to try to find a valid location for each spawned enemy.")]
    public int maxAttemptsPerSpawn = 8;

    // runtime
    private List<GameObject> spawned = new List<GameObject>();
    private Coroutine spawnRoutine;

    private void Start()
    {
        if (player == null)
        {
            var pgo = GameObject.FindGameObjectWithTag("Player");
            if (pgo != null) player = pgo.transform;
        }

        if (spawnOnStart) StartSpawning();
    }

    #region Public API
    public void StartSpawning()
    {
        if (spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    /// <summary>Spawn a single wave immediately (uses spawnPerWave count and respects maxConcurrentEnemies).</summary>
    public void SpawnWave()
    {
        CleanupSpawnedList();
        if (player == null) return;

        int allowed = Mathf.Max(0, maxConcurrentEnemies - spawned.Count);
        if (allowed <= 0) return;

        int toSpawn = Mathf.Min(spawnPerWave, allowed);
        SpawnN(toSpawn);
    }

    /// <summary>Spawn N enemies right now (respects maxConcurrentEnemies and checks for valid positions).</summary>
    public void SpawnN(int n)
    {
        CleanupSpawnedList();
        if (player == null) return;

        int allowed = Mathf.Max(0, maxConcurrentEnemies - spawned.Count);
        n = Mathf.Min(n, allowed);
        if (n <= 0) return;

        // precompute angles for more natural distribution
        float[] angles = GenerateAngles(n);

        for (int i = 0; i < n; i++)
        {
            GameObject prefab = PickEnemyPrefab();
            if (prefab == null) continue;

            bool created = false;
            for (int attempt = 0; attempt < maxAttemptsPerSpawn && !created; attempt++)
            {
                float angleDeg = angles[i] + Random.Range(-angleJitter * 0.5f, angleJitter * 0.5f); // small extra jitter
                float angleRad = angleDeg * Mathf.Deg2Rad;
                float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);
                Vector2 spawnPos2D = (Vector2)player.position + new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
                Vector3 spawnPos = new Vector3(spawnPos2D.x, spawnPos2D.y, 0f);

                // Check clear area
                var overlap = Physics2D.OverlapCircle(spawnPos2D, spawnClearCheckRadius, obstacleMask);
                if (overlap != null)
                {
                    // blocked - try again
                    continue;
                }

                // instantiate
                GameObject go = Instantiate(prefab, spawnPos, Quaternion.identity, spawnParent != null ? spawnParent : null);
                spawned.Add(go);
                created = true;
            }

            // if we couldn't find a free spot after attempts, skip this spawn
        }
    }
    #endregion

    #region Internals
    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(waveInterval);
        }
    }

    private void CleanupSpawnedList()
    {
        // remove destroyed entries
        for (int i = spawned.Count - 1; i >= 0; i--)
        {
            if (spawned[i] == null) spawned.RemoveAt(i);
        }
    }

    private GameObject PickEnemyPrefab()
    {
        if (enemyTypes == null || enemyTypes.Length == 0) return null;

        // compute total weight
        float total = 0f;
        foreach (var e in enemyTypes)
        {
            if (e == null) continue;
            total += Mathf.Max(0f, e.weight);
        }
        if (total <= 0f) return enemyTypes[0].prefab;

        float r = Random.value * total;
        float acc = 0f;
        foreach (var e in enemyTypes)
        {
            if (e == null) continue;
            acc += Mathf.Max(0f, e.weight);
            if (r <= acc)
            {
                return e.prefab;
            }
        }

        // fallback
        return enemyTypes[enemyTypes.Length - 1].prefab;
    }

    /// <summary>
    /// Generate a list of angles (degrees) to place N spawns around the circle.
    /// If clustering is disabled we use segment-based jitter (avoids perfect uniformity).
    /// If clustering is enabled we pick cluster centers then place spawns around those centers.
    /// </summary>
    private float[] GenerateAngles(int n)
    {
        float[] angles = new float[n];

        if (n <= 0) return angles;

        if (!useClustering)
        {
            // evenly spaced base plus jitter to avoid perfect uniformity
            float baseStart = Random.Range(0f, 360f);
            float segment = 360f / n;
            for (int i = 0; i < n; i++)
            {
                float a = baseStart + i * segment + Random.Range(-angleJitter, angleJitter);
                angles[i] = a;
            }
        }
        else
        {
            // clustering: pick cluster centers and distribute spawns into them
            int clusters = Mathf.Clamp(clusterCount, 1, n);
            float[] centers = new float[clusters];
            for (int c = 0; c < clusters; c++) centers[c] = Random.Range(0f, 360f);

            // assign each spawn to a random cluster (this makes cluster sizes variable)
            for (int i = 0; i < n; i++)
            {
                int clusterIndex = Random.Range(0, clusters);
                float center = centers[clusterIndex];
                float angleInCluster = center + Random.Range(-clusterSpread * 0.5f, clusterSpread * 0.5f);
                angles[i] = angleInCluster + Random.Range(-angleJitter, angleJitter);
            }
        }

        return angles;
    }
    #endregion

    #region Editor helpers (optional)
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (player == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(player.position, spawnRadiusMin);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, spawnRadiusMax);
    }
#endif
    #endregion
}
