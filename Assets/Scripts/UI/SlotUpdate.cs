using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUpdate : MonoBehaviour
{
    public bool HasObject { get; private set;} //настроен ли объект
    private Image slot_image;
    private GameObject slot_prefab;
    public Button slot_button;
    private GameObject slot_blocker;
    private int price=10;

    private void Start()
    {
        int pos = int.Parse(gameObject.name);
        slot_button = gameObject.GetComponent<Button>();
        SetSlot(pos);
        if(HasObject)
            slot_button.onClick.AddListener(UpdateLoader); // при нажатии выполняем UpdateLoader
    }
    /// <summary>
    /// Установить нужный объект из листа объектов Game_Loader, если объектов в листе больше чем номер текущего слота
    /// </summary>
    /// <param name="pos"></param>
    public void SetSlot(int pos)
    {
        if (gameObject.transform.parent.name == "Player_Holder")
        {
            if (Game_Loader.Instance.bodyPrefabs.Count > pos)
            {
                slot_prefab = Game_Loader.Instance.bodyPrefabs[pos];
                SetImage();
                SetButton();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.transform.parent.name == "Jump_Effect_Holder")
        {
            if (Game_Loader.Instance.jumpEffectsPrefabs.Length>pos)
            {
                slot_prefab = Game_Loader.Instance.jumpEffectsPrefabs[pos].particleEffect;
                SetImage(Game_Loader.Instance.jumpEffectsPrefabs[pos].particlePicture);
                SetButton();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.transform.parent.name == "Collision_Effect_Holder")
        {
            if (Game_Loader.Instance.collisionEffectPrefabs.Length>pos)
            {
                slot_prefab = Game_Loader.Instance.collisionEffectPrefabs[pos].particleEffect;
                SetImage(Game_Loader.Instance.collisionEffectPrefabs[pos].particlePicture);
                SetButton();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.transform.parent.name == "Arrow_Holder")
        {
            if (Game_Loader.Instance.arrowHeadPrefabs.Count > pos)
            {
                slot_prefab = Game_Loader.Instance.arrowHeadPrefabs[pos];
                SetImage();
                SetButton();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Didn't Find Prefab");
        }

        if (gameObject.transform.Find("Blocker") != null)
        {
            slot_blocker = gameObject.transform.Find("Blocker").gameObject;
            if (pos == gameObject.transform.parent.GetComponent<SlotsUiController>().unlockedSkins + 1)
            {
                slot_button.onClick.AddListener(UpdateBlocker);
                slot_blocker.SetActive(true);
            }
        }

        if (slot_blocker != null && HasObject && pos > gameObject.transform.parent.GetComponent<SlotsUiController>().unlockedSkins+1)
        {
            slot_image.enabled = false;
            slot_button.interactable = false;
            slot_blocker.SetActive(true);
        }
    }

    /// <summary>
    /// настроить картинку, цвет, кнопку
    /// </summary>
    private void SetImage()
    {
        slot_image = gameObject.GetComponentsInChildren<Image>()[1];
        slot_image.sprite = slot_prefab.GetComponentInChildren<SpriteRenderer>().sprite;
        if (gameObject.transform.parent.name != "Arrow_Holder")
            slot_image.color = slot_prefab.GetComponentInChildren<SpriteRenderer>().color;
        slot_image.enabled = true;
        HasObject = true;
    }
    private void SetImage(Sprite picture)
    {
        slot_image = gameObject.GetComponentsInChildren<Image>()[1];
        slot_image.sprite = picture;
        slot_image.enabled = true;
        HasObject = true;
    }

    /// <summary>
    /// Если имя объекта совпадает с настроенным сейчас объектом, то делаем кнопку неактивной
    /// </summary>
    public void SetButton()
    {
        if (slot_prefab.name == Game_Loader.Instance.GetPrefabInLoader(slot_prefab).name)
        {
            /*
            ColorBlock color_block = slot_button.colors;
            color_block.normalColor = color_block.pressedColor;
            slot_button.colors = color_block;
            */
            slot_button.interactable = false;

            gameObject.transform.parent.GetComponent<SlotsUiController>().ChangeCurrentObject(int.Parse(gameObject.name));
        }
        //if (slot_prefab.name)
        else slot_button.interactable = true;
    }

    private void UpdateBlocker()
    {
        Debug.Log("TEST");
        GameObject buy_Confirmation_Panel = gameObject.transform.parent.GetComponent<SlotsUiController>().buy_Confirmation_Panel;
        Text buy_panel_text = buy_Confirmation_Panel.GetComponentsInChildren<Text>()[0];
        buy_panel_text.text = $"Do you want to buy this item for {price}?";

        if(!buy_Confirmation_Panel.activeSelf)
            buy_Confirmation_Panel.SetActive(true);
        Button buy_confirmation = buy_Confirmation_Panel.GetComponentInChildren<Button>();
        buy_confirmation.onClick.AddListener(BuyComponent);
    }

    /// <summary>
    /// Обновляем текущий объект GameList'а и посылаем обновление инвентаря родителю.
    /// </summary>
    private void UpdateLoader()
    {
        if (!slot_blocker.activeSelf)
        {
            Game_Loader.Instance.ChangeSelectedObject(slot_prefab);
            SetButton();
        }
        //GetComponentInParent<SlotsUiController>().UpdateSet();
    }

    private void BuyComponent()
    {
        GameObject buy_Confirmation_Panel = gameObject.transform.parent.GetComponent<SlotsUiController>().buy_Confirmation_Panel;
        Button buy_confirmation = buy_Confirmation_Panel.GetComponentInChildren<Button>();
        if (Game_Loader.Instance.gems<price)
        {
            Text buy_panel_text = buy_Confirmation_Panel.GetComponentsInChildren<Text>()[0];
            buy_panel_text.text = $"Not enough {price-Game_Loader.Instance.gems} gems.";
        }
        else
        {
            Game_Loader.Instance.UpdateGems(-price);
            slot_blocker.SetActive(false);
            buy_Confirmation_Panel.SetActive(false);
            gameObject.GetComponentInParent<SlotsUiController>().UnlockSkin(int.Parse(gameObject.name));
            slot_button.onClick.RemoveListener(UpdateBlocker);
            UpdateLoader();
        }
        // do it anyway
        buy_confirmation.onClick.RemoveAllListeners();
    }
}
