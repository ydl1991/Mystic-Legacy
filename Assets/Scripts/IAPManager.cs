using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

public class IAPManager : MonoBehaviour
{
    public StorePageManager storePageManager;

    public int hasShield; // 0 = false, 0 = true
    public int hasRose; // 0 = false, 0 = true
    public int explosionCount; // count of how many explosions they have


    // Start is called before the first frame update
    void Start()
    {
        


    }

    public void CheckEntitlements()
    {
        //check if certain entitlements are already purchased (??)

        //if it is true that there is a shield
        if (PlayerPrefs.GetInt("hasShield") == 1)
        {
            Debug.Log("Yeah I have a shield");
        }
        else
        {
            Debug.Log("No shield sadge");

        }

        //if it is true that there is a rose
        if (PlayerPrefs.GetInt("hasRose") == 1)
        {
            Debug.Log("Yeah I have a rose");
        }
        else
        {
            Debug.Log("No rose sadge");

        }

        Debug.Log("Explosion count: " + PlayerPrefs.GetInt("explosionCount"));

    }

    public void OnSuccessfulPurchase(Product purchasedProduct)
    {
        switch(purchasedProduct.definition.id)
        {
            case "Champion's Shield":
                OnSuccessfullyPurchasedShield(purchasedProduct);
                break;
            case "Mystical Rose":
                OnSuccessfullyPurchasedRose(purchasedProduct);
                break;
            case "Dark Explosion":
                OnSuccessfullyPurchasedExplosion(purchasedProduct);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSuccessfullyPurchasedRose(Product purchasedProduct)
    {
        //give the rose to the player
        Debug.Log("Purchased " + purchasedProduct.definition.id);

        PlayerPrefs.SetInt("hasRose", 1);

        if (SceneManager.GetActiveScene().name == "Store")
        {
            storePageManager.SoldOutRose();
        }
    }

    public void OnSuccessfullyPurchasedShield(Product purchasedProduct)
    {
        //give the shield to the player
        Debug.Log("Purchased " + purchasedProduct.definition.id);

        PlayerPrefs.SetInt("hasShield", 1);

        if (SceneManager.GetActiveScene().name == "Store")
        {
            storePageManager.SoldOutShield();
        }
    }

    public void OnSuccessfullyPurchasedExplosion(Product purchasedProduct)
    {
        //give the player the explosion
        Debug.Log("Purchased " + purchasedProduct.definition.id);

        //add to the int count
        PlayerPrefs.SetInt("explosionCount", PlayerPrefs.GetInt("explosionCount") + 1);

    }

    public void OnPurchaseFailed(Product attemptedProduct, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + attemptedProduct.definition.id + " failed: " + reason );
    }


   
}
