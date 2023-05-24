using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[5, 5];
    public float coins;
    public Text coinsTXT;
    public GameObject panel;
    public GameObject panel1;
    public GameObject panel2;
    public Dropdown dropdown;
    private bool isIncreasingCoins = false;
    public GameObject UpgradeButton;

    void Start()
    {
        coinsTXT.text = "Money " + coins.ToString();


        shopItems[1, 1] = 1;//IDS
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;

        shopItems[2, 1] = 30;//Price
        shopItems[2, 2] = 25;
        shopItems[2, 3] = 15;
        shopItems[2, 4] = 50;
        shopItems[2, 5] = 200;

        //Quanitity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
        shopItems[3, 4] = 0;
        shopItems[3, 5] = 0;
        StartCoroutine(IncreaseCoinsOverTime());
    }

    IEnumerator IncreaseCoinsOverTime()
    {
        isIncreasingCoins = true;

        while (true)
        {
            yield return new WaitForSeconds(10f); // Wait for 10 seconds
            coins += 25; // Increase coins by 25
            coinsTXT.text = "Coins: " + coins.ToString();

            if (coins > 150)
            {
                // Activate the upgrade button when the player has 150 coin
                UpgradeButton.SetActive(true);
            }
        }
    }


    public void Buy()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int currentQuantity = shopItems[3, itemID];

        if (coins >= shopItems[2, itemID] && currentQuantity < 4)
        {
            coins -= shopItems[2, itemID];
            shopItems[3, itemID]++;
            coinsTXT.text = "Coins: " + coins.ToString();
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();
        }
        else if (currentQuantity >= 4)
        {
            Debug.Log("Maximum quantity reached. Cannot add more items.");
        }
        else
        {
            Debug.Log("Insufficient coins to buy the item.");
        }
    }

    public void Sell()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int currentQuantity = shopItems[3, itemID];

        if (currentQuantity > 0)
        {
            coins += shopItems[2, itemID]; // Increase coins
            shopItems[3, itemID]--; // Decrease quantity
            coinsTXT.text = "Coins: " + coins.ToString();
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();

            Debug.Log("Item sold successfully!");
        }
        else
        {
            Debug.Log("No items to sell.");
        }
    }
    public void Chest()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;

        if (shopItems[3, itemID] > 0)
        {
            // Decrease quantity in the backpack
            shopItems[3, itemID]--;

            // Increase quantity in the chest
            shopItems[4, itemID]++;

            // Update quantity in QuantityTXT
            buttonInfo.QuantityTXT.text = shopItems[3, itemID].ToString();

            // Update quantity in QuantityChestTXT
            buttonInfo.QuantityChestTXT.text = shopItems[4, itemID].ToString();

            Debug.Log("Item moved to chest successfully!");
        }
        else
        {
            Debug.Log("No items to move to chest.");
        }
    }
    public void ChestToBackpack()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;

        if (shopItems[4, itemID] > 0)
        {
            // Decrease quantity in the chest
            shopItems[4, itemID]--;

            // Increase quantity in the backpack
            shopItems[3, itemID]++;

            // Update quantity in quantityChestTXT
            buttonInfo.QuantityChestTXT.text = "Quantity " + shopItems[4, itemID].ToString();

            // Update quantity in quantityTXT
            buttonInfo.QuantityTXT.text = "Quantity " + shopItems[3, itemID].ToString();

            Debug.Log("Item moved from chest to backpack successfully!");
        }
        else
        {
            Debug.Log("No items to move from chest to backpack.");
        }
    }









    public void ShopAct()
    {
        if (panel.activeInHierarchy == true)
        {
            panel.SetActive(false);
            panel1.SetActive(false);
            panel2.SetActive(false);
        }
        else
        {
            panel.SetActive(true);
            panel1.SetActive(true);
            panel2.SetActive(false);
        }


    }

    public void BackpackAct()
    {
        if (panel1.activeInHierarchy == true)
        {
            panel1.SetActive(false);
            panel2.SetActive(false);
            panel.SetActive(false);
        }
        else
        {
            panel1.SetActive(true);
            panel2.SetActive(true);
            panel.SetActive(false);
        }


    }






    public void FilterTag(string tag)
    {
        GameObject[] objectsToDisable = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in objectsToDisable)
        {
            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    public void OnDropdownValueChanged(int optionIndex)
    {
        string tag = GetTagForDropdownOption(optionIndex);
        FilterTag(tag);
    }

    public string GetTagForDropdownOption(int optionIndex)
    {
        // Define the mapping of option index to tag name
        string[] tags = { "Health", "Power", "Upgrade" };

        // Check if the option index is within the valid range
        if (optionIndex >= 0 && optionIndex < tags.Length)
        {
            return tags[optionIndex];
        }

        // Return an empty string if the index is invalid
        return string.Empty;
    }

    public void UpgradeQuantityLimit()
    {
 

        // Check if there are enough coins to perform the upgrade
        if (coins >= 150)
        {
            // Subtract 150 coins from the total
            coins -= 150;
            coinsTXT.text = "Coins: " + coins.ToString();

            // Increase the quantity limit for all items by 1
            for (int i = 0; i < shopItems.GetLength(1); i++)
            {
                int currentQuantity = shopItems[3, i];

                if (currentQuantity == 4)
                {
                    shopItems[3, i] = 5;

                    // Update the quantity text of the button immediately
                    GameObject[] buttons = GameObject.FindGameObjectsWithTag("ShopItemButton");
                    foreach (GameObject button in buttons)
                    {
                        ButtonInfo buttonInfo = button.GetComponent<ButtonInfo>();
                        if (buttonInfo.ItemID == i)
                        {
                            buttonInfo.QuantityTXT.text = shopItems[3, i].ToString();
                            break;
                        }
                    }
                }
            }

            Debug.Log("Quantity limit upgraded to 5 for all items.");
        }
        else
        {
            Debug.Log("Insufficient coins to perform the upgrade.");
        }
    }








    public void CheckQuantityLimit()
    {
        GameObject buttonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        ButtonInfo buttonInfo = buttonRef.GetComponent<ButtonInfo>();

        int itemID = buttonInfo.ItemID;
        int currentQuantity = shopItems[3, itemID];

        if (currentQuantity >= 4)
        {
            Debug.Log("Maximum quantity reached for item with ID: " + itemID + ". Cannot add more items.");
        }
    }
}

