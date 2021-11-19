using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    // List of all asteroids.
    public List<GameObject> asteroids = new List<GameObject>();
    
    // Astroid to be spawned.
    public Astroid astroidPrefab;
    // How often Astroids are spawned.
    public float spawnRate = 2.0f;
    // How many are spawned when this is called.
    public int spawnAmount = 1;
    // The distance away from the origin.
    public float spawnDistance = 15.0f;
    // The angle it will spawn at difference.
    public float trajectoryVariance = 15.0f;
    void Start()
    {
        // Call the Astroid Spawner on a timer.
        InvokeRepeating(nameof(Spawn), this.spawnRate, this.spawnRate);
    }

    // Spawn an astroid.
    private void Spawn()
    {
        // Spaw 'spawnAmount' of astroids.
        for (int i = 0; i < this.spawnAmount; i++)
        {
            SpawnOneAsteroid();
        }
    }

    public GameObject SpawnOneAsteroid()
    {
        // Spawn at a Random point on the edge of a circle around the spawner at 'spawnDistance'.
        Vector3 spawnDirection = Random.insideUnitCircle.normalized * this.spawnDistance;
        Vector3 spawnPoint = this.transform.position + spawnDirection;
        // Spawn at a random point on the 'trajectoryVariance' angle.
        float variance = Random.Range(-trajectoryVariance, trajectoryVariance);
        // Get the rotation of the astroid at between 0 and 'trajectoryVariance' degrees.
        Quaternion rotation = Quaternion.AngleAxis(variance, Vector3.forward);
        // Instansiate this astroid with the variables above.
        Astroid astroid = Instantiate(this.astroidPrefab, spawnPoint, rotation);
        // Make its size between the min and max Size (defined in 'Astroid').
        astroid.size = Random.Range(astroid.minSize, astroid.maxSize);
        // Calls the Trajectory and Makes it always go towards the center of the screen.
        astroid.SetTrajectory(rotation * -spawnDirection);
        // Adds asteroid to the list of asteroids.
        asteroids.Add(astroid.gameObject);
        // Return a gameObject as this is needed for a test runner.
        return astroid.gameObject;
    }
    
    // Clears and resets the list of asteroids.
    public void ClearAsteroids()
    {
        foreach(GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }
        asteroids.Clear();
    }
}
