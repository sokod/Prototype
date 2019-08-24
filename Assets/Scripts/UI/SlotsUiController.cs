using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsUiController : MonoBehaviour
{
    private SlotUpdate[] slots;
    public int unlockedSkins { get; private set; }
    public int positionOfCurrentObject { get; private set; }
    public GameObject buy_Confirmation_Panel;
    public GameObject price_Tag;

    void Start()
    {
        slots = GetComponentsInChildren<SlotUpdate>();
    }
    private void Awake()
    {
        Debug.Log("Getting value of unlocked items");
        //unlockedSkins=PlayerPrefs.GetInt(gameObject.name, 1);
        PlayerPrefs.GetInt(gameObject.name, 1);
    }
    public void UnlockSkin(int number)
    {
        if(unlockedSkins==number) return;
        unlockedSkins = number;
        PlayerPrefs.SetInt(gameObject.name, unlockedSkins);
        slots[number + 1].SetSlot(number + 1);
        Debug.Log("Updating unlocked skins. Skin group: "+ gameObject.name +"; skin number " +number);
    }

    /// <summary>
    /// По запросу проверяем каждый настроенный объект инвентаря, обновляем выбранный элемент.
    /// </summary>
    public void UpdateSet()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].HasObject)
            {
                slots[i].SetButton();
            }
        }
    }
    
    public void ChangeCurrentObject(int value)
    {
        
        int previousPos = positionOfCurrentObject;
        if (previousPos == value) return;
        else
        {
            slots[previousPos].slot_button.interactable = true;
            positionOfCurrentObject = value;
        }
    }
}
