using UnityEngine;

public class ParticlesSpawner : MonoBehaviour
{
    public static ParticlesSpawner Instance;

    [SerializeField] private GameObject particlesPrefab;

    private void Awake() => Instance = this;

    public void SpawnParticles(Vector3 position)
    {
        Debug.Log("spawned");
        Instantiate(particlesPrefab, position, Quaternion.identity);
    }
}
