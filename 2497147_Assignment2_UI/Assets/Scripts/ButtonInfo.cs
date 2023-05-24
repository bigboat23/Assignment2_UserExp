using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{
    public int ItemID;
    public Text priceTXT;
    public Text QuantityTXT;
    public string ItemType;
    public GameObject shopManager;
    public Text QuantityChestTXT;

    // public Text QuantityMaxTXT;

    void Update()
    {
        priceTXT.text = "Coins $" + shopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();
        QuantityTXT.text = "Quantity " + shopManager.GetComponent<ShopManager>().shopItems[3, ItemID].ToString();
        QuantityChestTXT.text = shopManager.GetComponent<ShopManager>().shopItems[4, ItemID].ToString();
    }

    public void OnButtonClick()
    {
        shopManager.GetComponent<ShopManager>().Chest();
    }

}
