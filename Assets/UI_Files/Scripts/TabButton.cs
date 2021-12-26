using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabButton : MonoBehaviour , IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    //setting a reference to the tabgrpscript
    public SideTabGrp tabGrp;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGrp.OnTabSelected(this);
        Debug.Log(name + " Game Object Clicked!");
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

}
