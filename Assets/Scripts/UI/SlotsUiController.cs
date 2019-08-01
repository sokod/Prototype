using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotsUiController : MonoBehaviour
{
    private SlotUpdate[] slots;
    void Start()
    {
        slots = GetComponentsInChildren<SlotUpdate>();
    }

    /// <summary>
    /// По запросу проверяем каждый настроенный объект инвентаря, обновляем выбранный элемент.
    /// </summary>
    public void UpdateSet()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].HasObject) 
                slots[i].SetButton();
        }
    }

}
