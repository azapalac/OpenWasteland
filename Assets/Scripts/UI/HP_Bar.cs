using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HP_Bar : MonoBehaviour {

    [Range(0, 1)]
    public float percentageRemaining;


    public Image hpBar;
    private float origWidth;
    RectTransform rectTransform;
    public void SetPercentage(float currHP, float maxHP)
    {
        percentageRemaining = (currHP / maxHP);
    }
	// Use this for initialization

    void Awake()
    {
        hpBar = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        origWidth = rectTransform.rect.width;

    }
	void Start () {
       

    }

    // Update is called once per frame
    void Update () {

        if(percentageRemaining > 1)
        {
            percentageRemaining = 1;
        }

        if(percentageRemaining < 0)
        {
            percentageRemaining = 0;
        }

        float redValue = (1 - percentageRemaining) * 2;
        hpBar.color = new Color((1 - percentageRemaining)*2, (percentageRemaining), percentageRemaining/3f);

        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, origWidth*percentageRemaining);

    }
}
