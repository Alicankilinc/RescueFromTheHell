using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DamageHeal : MonoBehaviour
{
    public float totalHealth=100;
    public float currentHealth=100;
    public Image healthImage;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthImage.fillAmount=currentHealth/100;
        
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            currentHealth = currentHealth - 0.1f;
        }
        if (collision.gameObject.tag == "Healer")
        {
            currentHealth = currentHealth + 35f;
            
            Destroy(collision.gameObject);
        }   
    }



}
