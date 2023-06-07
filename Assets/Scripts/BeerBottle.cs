using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerBottle : MonoBehaviour
{
    public List <Rigidbody> allParts = new List<Rigidbody>();

    public void Shatter()
    {
        SoundManager.Instance.shatterSound.Play();
        foreach (var part in allParts)
        {
            part.isKinematic = false;
        }
        Destroy(this.gameObject, 5);
    }
}
