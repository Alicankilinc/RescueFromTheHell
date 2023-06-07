using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIandmanager : MonoBehaviour
{
    public TextMeshProUGUI doorLocked, gameOver, youShouldSave;
    public TextMeshProUGUI switchInfo;
    public UnityEngine.UI.Image gameOverImage;
    
    // Start is called before the first frame update
    void Start()
    {
        
        Destroy(switchInfo, 5);
        doorLocked.enabled = false;
        gameOverImage.enabled = false;
        gameOver.enabled = false;
        youShouldSave.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "GameOver")
        {
            if (PlayerPrefs.HasKey("hostagefollowing"))
            {
                
                gameOver.enabled = true;
                gameOverImage.enabled = true;
                Time.timeScale = 0;
            }
            else
            {
                youShouldSave.enabled = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "GameOver")
        {
            youShouldSave.enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LockedDoorTrigger")
        {
            doorLocked.enabled = true;
            SoundManager.Instance.lockedDoor.Play();
        }
        if (other.gameObject.tag == "LevelOneShifter")
        {
            SceneManager.LoadScene(1);
            SoundManager.Instance.radioTalk.Play();
        }
        if (other.gameObject.tag == "LevelTwoShift")
        {
            SceneManager.LoadScene(2);
        }
        if (other.gameObject.tag == "LevelThreeShift")
        {
            SceneManager.LoadScene(3);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        doorLocked.enabled = false;
    }
}
