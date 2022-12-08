using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    // The prefab for the food
    public GameObject foodPrefab;

    // The area in which the food will be spawned
    public Vector2 spawnArea = new Vector2(20, 15);

    // The current food
    GameObject food;

    // Spawn a new piece of food
    void SpawnFood()
    {
        // Spawn the food at a random location within the spawn area
        int x = (int)Random.Range(-spawnArea.x, spawnArea.x);
        int y = (int)Random.Range(-spawnArea.y, spawnArea.y);

        // Create a new food prefab
        food = (GameObject)Instantiate(foodPrefab,
                                       new Vector2(x, y),
                                       Quaternion.identity);
    }

    // Called when the script is first loaded
    void Start()
    {
        // Spawn the initial food
        SpawnFood();
    }

    // Update is called once per frame
    void Update()
    {
        // Is the food still on the screen?
        if (food == null)
        {
            // If not, spawn a new piece of food
            SpawnFood();
        }
    }
}