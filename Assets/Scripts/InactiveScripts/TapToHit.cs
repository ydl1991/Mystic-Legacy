using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapToHit : MonoBehaviour
{
    [SerializeField]
    Color hitColor = Color.red;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        StartCoroutine("DealDamage");
    }

    IEnumerator DealDamage()
    {
        //get the object's mesh renderer and change the color to our color variable
        this.GetComponent<MeshRenderer>().material.color = hitColor;

        //delay 
        yield return new WaitForSeconds(1.5f); 

        //set the object to inactive ("destroy" it)
        gameObject.SetActive(false);

    }
}
