using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    // The direction the snake is moving in
    Vector2 dir = Vector2.right;

    // The list of all the snake's body parts
    List<Transform> tail = new List<Transform>();

    // Did the snake collide with food?
    bool ate = false;

    // The snake's speed
    public float speed = 0.1f;

    public float speedIncrease = 0.4f;

    // The prefab for the snake's body part
    public GameObject tailPrefab;

    public int lengthAdded = 1;

    // The object pool for the snake's body parts
    Queue<Transform> bodyPartsPool = new Queue<Transform>();

    // Update is called once per frame
    void Update()
    {
        // Move in the direction the snake is facing
        transform.Translate(dir * speed * Time.deltaTime);

        // Did the snake collide with food?
        if (ate)
        {
            speed += speedIncrease;

            for (int i = 0; i < lengthAdded; i++)
            {
                // Get a body part from the object pool
                Transform g = GetBodyPartFromPool();

                // Set the body part's position and rotation
                g.position = transform.position;
                g.rotation = Quaternion.identity;

                // Add the new body part to the list
                tail.Insert(0, g.transform);
            }

            // Reset the flag
            ate = false;
        }
        // Otherwise, move the last body part to where the head was
        else if (tail.Count > 0)
        {
            tail.Last().position = transform.position;

            // Add the last body part to the front of the list
            tail.Insert(0, tail.Last());

            // Remove the last body part from the list
            tail.RemoveAt(tail.Count - 1);
        }

        // Change the direction the snake is facing based on input
        if (Input.GetKey(KeyCode.RightArrow))
            dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            dir = -Vector2.up;
        else if (Input.GetKey(KeyCode.LeftArrow))
            dir = -Vector2.right;
        else if (Input.GetKey(KeyCode.UpArrow))
            dir = Vector2.up;
    }

    // Gets a body part from the object pool
    Transform GetBodyPartFromPool()
    {
        // If the object pool is empty, create a new body part
        if (bodyPartsPool.Count == 0)
        {
            GameObject g = (GameObject)Instantiate(tailPrefab);
            g.SetActive(false);
            bodyPartsPool.Enqueue(g.transform);
        }
        // Get the first body part from the queue
        Transform t = bodyPartsPool.Dequeue();
        // Make sure the body part is active
        t.gameObject.SetActive(true);
        return t;
    }

    // Returns a body part to the object pool
    void ReturnBodyPartToPool(Transform t)
    {
        // Make sure the body part is inactive
        t.gameObject.SetActive(false);
        // Add the body part to the end of the queue
        bodyPartsPool.Enqueue(t.gameObject.transform);
    }


    // Called when the snake collides with something
    void OnTriggerEnter2D(Collider2D coll)
    {
        // Did the snake collide with food?
        if (coll.name.StartsWith("FoodPrefab"))
        {
            // Set the ate flag to true
            ate = true;

            // Destroy the food
            Destroy(coll.gameObject);
        }
        // Otherwise, the snake collided with itself or a wall
        else
        {
            // Restart the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}