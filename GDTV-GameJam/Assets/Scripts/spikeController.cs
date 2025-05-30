using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeController : MonoBehaviour
{
    public CheckPointManager checkPointMan;
    private void Start()
    {
        checkPointMan = FindFirstObjectByType<CheckPointManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SoundEffectSource.Instance.PlaySoundEffect(1);
            if (checkPointMan == null)
            {
                playerController.instance.gameObject.transform.position = new Vector3(-2, -2, 0);
            }
            if (checkPointMan.isCheckpointActive)
            {
                playerController.instance.gameObject.transform.position = checkPointMan.checkPoint.position;
            }
            else
            {
                playerController.instance.gameObject.transform.position = new Vector3(-2, -2, 0);
            }
            
        }
    }
}
