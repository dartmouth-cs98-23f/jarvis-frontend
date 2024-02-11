using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageDotManager : MonoBehaviour
{
    public GameObject dot1;
    public GameObject dot2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleDot()
    {
        dot1.GetComponent<Outline>().enabled = true;
        dot1.GetComponent<Outline>().effectColor = new Color32(115, 123, 125, 255);
        dot1.GetComponent<Image>().color = new Color32(222, 207, 191, 255);
        dot2.GetComponent<Image>().color = new Color32(108, 98, 86, 255);
        dot2.GetComponent<Outline>().enabled = false;
    }
    public void ToggleDot2()
    {
        dot1.GetComponent<Image>().color = new Color32(108, 98, 86, 255);
        dot1.GetComponent<Outline>().enabled = false;
        dot2.GetComponent<Image>().color = new Color32(222, 207, 191, 255);
        dot2.GetComponent<Outline>().enabled = true;
        dot2.GetComponent<Outline>().effectColor = new Color32(115, 123, 125, 255);
    }
}
