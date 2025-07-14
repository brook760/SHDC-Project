using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] footSounds;
    public float stepInterval = 0.5f;
    public float volume = 1.0f;
    public Vector2 input = Vector2.zero;

    private float stepTimer;
    public Character character;

    public void FootSound(int index)
    {
       AudioClip footStepClip = footSounds[index];
        source.PlayOneShot(footStepClip, volume);
    }
    private void Update()
    {
        input = character.playerInput.actions["Move"].ReadValue<Vector2>();
        if (input.x > 0 || input.x <0 || input.y >0 || input.y <0)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                if (character.playerInput.actions["Sprint"].triggered)
                    FootSound(1);
                else
                    FootSound(0);
                stepTimer = stepInterval;
            }
        }
    }
}
