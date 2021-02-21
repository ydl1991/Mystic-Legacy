using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToChangeColor : MonoBehaviour
{
    [SerializeField]
    Color m_color = Color.red;

    // Start is called before the first frame update
    void Start()
    {

    }

    private bool GetTouchRay(out Ray tapRay)
    {
        //find the number of touches (store the result in a variable)
        int numTouches = Input.touchCount;

        if (numTouches == 1)
        {
            //get info about the first of those touches
            Touch theTouch = Input.GetTouch(0); // a regular tap is one touch, a touch object

            // use raycasting to determine whether that touch was on this object

            //3D
            Vector3 touchPoint = theTouch.position; // get finger position in pixel coords
            Ray touchRay = Camera.main.ScreenPointToRay(touchPoint); //create a ray passing it through in world coords
            tapRay = touchRay;
            return true;
        }

        tapRay = new Ray();
        return false;
    }
    private bool GetMouseRay(out Ray tapRay)
    {
        if (Input.GetMouseButtonDown(0))
        {
            tapRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            return true;
        }
        tapRay = new Ray();
        return false; //no input
    }

    // Update is called once per frame
    void Update()
    {
        bool inputFound = false;

        Ray inputRay;


        //allowing us to test this stuff in editor as well, without having to build to device every time

#if UNITY_EDITOR             // if this code is running in the unity editor, do this stuff 

        if (GetTouchRay(out inputRay))
        {
            inputFound = true;
        }
        else if (GetMouseRay(out inputRay))
        {
            inputFound = true;
        }

        if (inputFound)
        {
            RaycastHit hitInfo; // will hold the results
            if (Physics.Raycast(inputRay, out hitInfo)) 
            {
                //determine whether that RaycastHit ever touched this object
                if (hitInfo.transform == this.transform) // if what we hit is the same as this object
                {
                    this.GetComponent<MeshRenderer>().material.color = m_color;

                }

            }
        }

#elif UNITY_ANDROID          // else if tis running on an android device natively, do this stuff (does not include unity remote)

       if (GetTouchRay(out inputRay))
        {
            inputFound = true;
        }
        
#endif                      // end if goes at the end of anyy such use of #if /#elif/ #else

        if (inputFound)
        {
            RaycastHit hitInfo; // will hold the results
            if (Physics.Raycast(inputRay, out hitInfo))
            {
                //determine whether that RaycastHit ever touched this object
                if (hitInfo.transform == this.transform) // if what we hit is the same as this object
                {
                    this.GetComponent<MeshRenderer>().material.color = m_color;

                }

            }
        }

        



    }

    
}
