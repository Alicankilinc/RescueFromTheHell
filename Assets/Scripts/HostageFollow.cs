using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class HostageFollow : MonoBehaviour
{
    public GameObject playerObject;
    public float TargetDistance;
    public float AllowedDistance = 5;
    public GameObject theHostage;
    public float followSpeed;
    public RaycastHit Shot;
    public Animator hostageAnim;
    public TextMeshProUGUI hostageText;
    public Transform playerr;
    string followtrigger;
    // Start is called before the first frame update
    void Start()
    {
        hostageText.enabled = false;
        hostageAnim=GetComponent<Animator>();
        PlayerPrefs.DeleteKey("hostagefollowing");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(playerr.transform.position, this.transform.position) <= 5)
        {
            hostageText.enabled = true;
        }
        if (hostageText.enabled==true && Input.GetKeyDown(KeyCode.E))
        {
            SoundManager.Instance.hostageSound.Play();
            hostageAnim.SetBool("Stand", true);
            PlayerPrefs.SetString("hostagefollowing", followtrigger);
            
        }
        if (PlayerPrefs.HasKey("hostagefollowing"))
        {
            
            hostageText.enabled = false;
            transform.LookAt(playerObject.transform);
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out Shot))
            {
                TargetDistance = Shot.distance;
                if (TargetDistance >= AllowedDistance)
                {
                    followSpeed = 0.1f;
                    hostageAnim.SetBool("IdleHostage", false);
                    hostageAnim.SetBool("RunHostage", true);
                    transform.position = Vector3.MoveTowards(transform.position, playerObject.transform.position, followSpeed);
                }
                else
                {
                    followSpeed = 0;
                    hostageAnim.SetBool("RunHostage", false);
                    hostageAnim.SetBool("IdleHostage", true);

                }
            }
        }
    }
}
