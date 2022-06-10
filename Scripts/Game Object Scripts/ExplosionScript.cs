// Author: Matthew Spiller
// Updated: June 9, 2022

using System.Collections;
using UnityEngine;

// Allows for the explosion to be deactivated after some amount of time
public class ExplosionScript : MonoBehaviour
{
    IEnumerator DisableExplosion(float timer)
    {
        yield return new WaitForSeconds(timer);
        gameObject.SetActive(false);
    }
}
