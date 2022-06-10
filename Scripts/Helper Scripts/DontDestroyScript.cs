// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;

// This is attached to AudioSource GameObjects which must not be destroyed between scenes (buttons) so that they can play audio
public class DontDestroyScript : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
