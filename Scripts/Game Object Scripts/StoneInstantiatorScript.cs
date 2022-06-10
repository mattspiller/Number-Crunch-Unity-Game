// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;

// Responsible for spawning a stone in the left, right or center of the screen
public class StoneInstantiatorScript : MonoBehaviour
{
    public GameObject stone;

    public void InstantiateLeft()
    {
        Instantiate (stone, new Vector3(-4.5f, transform.position.y, transform.position.z), Quaternion.identity);
    }

    public void InstantiateRight()
    {
        Instantiate (stone, new Vector3(4.5f, transform.position.y, transform.position.z), Quaternion.identity);
    }

    public void InstantiateCenter()
    {
        Instantiate (stone, transform.position, Quaternion.identity);
    }
}
