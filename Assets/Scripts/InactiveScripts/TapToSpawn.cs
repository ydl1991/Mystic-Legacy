using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToSpawn : MonoBehaviour
{

    [SerializeField]
    Color selectedColor = Color.red;

    [SerializeField]
    Color unselectedColor = Color.blue;

    bool isSelected = false;

    // Start is called before the first frame update
    void Start()
    {
        //get the object's mesh renderer and change the color to our color variable
        this.GetComponent<MeshRenderer>().material.color = unselectedColor;
    }


    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnMouseDown()
    {
        if(isSelected == false)
        {
            //get the object's mesh renderer and change the color to our color variable
            this.GetComponent<MeshRenderer>().material.color = selectedColor;
            isSelected = true;
        }
        else if (isSelected)
        {
            //get the object's mesh renderer and change the color to our color variable
            this.GetComponent<MeshRenderer>().material.color = unselectedColor;
            isSelected = false;

        }



    }

    
}
