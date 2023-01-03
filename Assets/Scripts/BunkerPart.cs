using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BunkerPart : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.layer == LayerMask.NameToLayer("Missile") || other.gameObject.layer == LayerMask.NameToLayer("Invader"))
        {
            this.gameObject.SetActive(false);
        }
    }
}
