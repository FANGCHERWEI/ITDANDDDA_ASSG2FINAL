using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject fire;
    public AudioSource fireSound;

    public GameObject task01CompletionCanvas;

    public bool fireSoundIsPlaying;

    private void Awake()
    {
        fireSoundIsPlaying = false;
    }

    public void ToggleFireEffects()
    {

        if (!fireSoundIsPlaying)
        {
            fire.SetActive(true);

            fireSound.Play();
            fireSoundIsPlaying = true;
        }

        else
        {
            fire.SetActive(false);

            fireSound.Stop();
            fireSoundIsPlaying = false;
        }
    }

    public void ToggleTask01CompletionCanvas()
    {
        task01CompletionCanvas.SetActive(false);
    }
}
