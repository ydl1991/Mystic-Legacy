using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorePageManager : MonoBehaviour
{
    public IAPManager IAPmanager;

    public Button shieldPurchaseButton;
    public Button rosePurchaseButton;

    public Text shieldButtonText;
    public Text roseButtonText;

    // Start is called before the first frame update
    void Start()
    {
        //check entitlements
        CheckingStoreEntitlements();
    }

    public void CheckingStoreEntitlements()
    {
        //if it is true that there is a shield
        if (PlayerPrefs.GetInt("hasShield") == 1)
        {
            SoldOutShield();
        }

        //if it is true that there is a rose
        if (PlayerPrefs.GetInt("hasRose") == 1)
        {
            SoldOutRose();
        }

    }


    public void SoldOutShield()
    {
        //set the button to uninteractable
        shieldPurchaseButton.interactable = false;

        //change the text to "sold out"
        shieldButtonText.text = "SOLD OUT";
    }

    public void SoldOutRose()
    {
        //set the button to uninteractable
        rosePurchaseButton.interactable = false;

        //change the text to "sold out"
        roseButtonText.text = "SOLD OUT";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
