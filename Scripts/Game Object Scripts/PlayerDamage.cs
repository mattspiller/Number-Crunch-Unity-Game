// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;

// Responsible for creating the explosion when the player gets hit by the rock, and deactivating the Player object
public class PlayerDamage : MonoBehaviour
{
    public GameObject explosion;
    public AudioSource explosionAudio;

    // When the stone hits The Player, play explosion sound, create explosion effect (object),
    // disable The Player, and then disable the explosion
    public void DealDamage()
    {
        explosionAudio.Play();
        GameObject thisExplosion = Instantiate(explosion, transform.position, Quaternion.identity);
        gameObject.SetActive(false);
        thisExplosion.GetComponent<ExplosionScript>().StartCoroutine("DisableExplosion", 0.15f);
    }
}
