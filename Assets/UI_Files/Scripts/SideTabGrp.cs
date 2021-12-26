using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SideTabGrp : MonoBehaviour
{
    //create a list of gameobject to store for the list of side tab button
    public List<TabButton> tabButtons;
    //the button is being selected
    public TabButton selectedTab;
    //list of gameobject going to swap 
    public List<GameObject> objectToswap;
    public GameObject linkData;

    //this method could take in type of button

    private void Start()
    {
        
    }
   

    public void OnTabSelected(TabButton btn)
    {
        Debug.Log("selectbtnpls");
        selectedTab = btn;
        //get sibling index from the transform btn
        
        int index = btn.transform.GetSiblingIndex();
        print(index);
        //usage of for loop to loop inside of the object gameobject panel
        for (int i = 0; i < objectToswap.Count; i++)
        {
            for (int j = 0; j < tabButtons.Count; j++)
            {
                //check if the index is eqal to the particular 
                if (i == index && tabButtons[i])
                {
                    objectToswap[i].SetActive(true);

                    if (index == 1)
                    {
                        linkData.GetComponent<LinkData>().Profile();
                    }

                    if (index == 3)
                    {
                        linkData.GetComponent<LinkData>().Leaderboard();
                    }

                }
                else
                {
                    objectToswap[i].SetActive(false);
                }
            }

        }
    }

}
