// Author: Matthew Spiller
// Updated: June 9, 2022

using UnityEngine;
using UnityEngine.UI;

// Responsible for increasing and decreasing the transparency of the divider
public class DividerScript : MonoBehaviour
{
    private Image image;
    private bool fade = true;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void FixedUpdate()
    {
        if (fade)
        {
            if (image.color.a >= 0.2f)
            {
                Color temp = image.color;
                temp.a -= 0.02f;
                image.color = temp;
            }
            else
            {
                fade = false;
            }
        }
        else
        {
            if (image.color.a < 1f)
            {
                Color temp = image.color;
                temp.a += 0.02f;
                image.color = temp;
            }
            else
            {
                fade = true;
            }
        }
    }
}