using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Update_Freeze : MonoBehaviour
{
    public RawImage[] FreezeCounters;
    public RawImage FreezedBorder;
    private Vector3 positionChange, positionChange2;
    public bool show;
    public bool hide;
    // Start is called before the first frame update
    void Start()
    {
        show = false;
        hide = false;
        // to show the freeze counter like crash bandicoot boxes/lives
        positionChange = new Vector3(0.0f, 0.01f, 0.0f);
        positionChange2 = new Vector3(0.0f, -0.01f, 0.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        print(FreezeCounters[0].transform.position.y);
        
        if (show == true)
        {

            // transform.localScale += scaleChange;
            FreezeCounters[0].transform.position += positionChange2;
            FreezeCounters[1].transform.position += positionChange2;
            FreezeCounters[2].transform.position += positionChange2;
            StartCoroutine(WaitAndPrint(1.7f));
        }
        else if (hide == true)
        {
            //  transform.localScale += scaleChange;
            FreezeCounters[0].transform.position += positionChange;
            FreezeCounters[1].transform.position += positionChange;
            FreezeCounters[2].transform.position += positionChange;
            StartCoroutine(WaitAndPrint2(1.7f));
        }

    }

    public void HideUpdate()
    {

    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        show = false;
        hide = true;

    }
    private IEnumerator WaitAndPrint2(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        show = false;
        hide = false;
    }
}
