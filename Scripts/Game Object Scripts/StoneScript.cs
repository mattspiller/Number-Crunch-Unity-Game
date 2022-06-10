// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;

// Used for Player-Stone collisions
public class StoneScript : MonoBehaviour
{
    void Start()
    {
        Invoke("Deactivate", 4f);
    }

    // Start() Calls Deactivate() when the stone doesn't hit the player and continues to fall
    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // When the stone hits the player
    private void OnTriggerEnter2D(Collider2D target)
    {
        if(target.tag == MyTags.PLAYER_TAG)
        {
            target.GetComponent<PlayerDamage>().DealDamage();
            gameObject.SetActive(false);
        }
    }
}
