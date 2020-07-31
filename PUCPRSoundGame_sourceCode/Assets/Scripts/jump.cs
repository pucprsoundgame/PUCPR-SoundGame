using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jump : MonoBehaviour
{
    [SerializeField] private AudioSource _jumpAudioSource;
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump")) {
            this._jumpAudioSource.Play();
        }        
    }
}
