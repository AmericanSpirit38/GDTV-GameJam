using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Victory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Dance());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Dance()
    {
        yield return new WaitForSeconds(0.5f);
        transform.Rotate(new Vector3(0, 0, -30), Space.Self);
        yield return new WaitForSeconds(0.5f);
        transform.Rotate(new Vector3(0, 0, 30), Space.Self);
        StartCoroutine(Dance());
    }
}
