using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUpdate : MonoBehaviour
{
    public bool HasObject { get; private set;} //настроен ли объект
    private Image slot_image;
    private GameObject slot_prefab;
    private Button slot_button;

    private void Start()
    {
        int pos = int.Parse(gameObject.name);
        slot_button = gameObject.GetComponent<Button>();
        SetSlot(pos);
    }
    /// <summary>
    /// Установить нужный объект из листа объектов Game_Loader
    /// </summary>
    /// <param name="pos"></param>
    private void SetSlot(int pos)
    {
        if (gameObject.transform.parent.name == "Player_Holder")
        {
            if (Game_Loader.Instance.bodyPrefabs.Count > pos)
            {
                slot_prefab = Game_Loader.Instance.bodyPrefabs[pos];
                SetImage();
                SetButton();
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
        }
        else if (gameObject.transform.parent.name == "Collision_Effect_Holder")
        {
            if (Game_Loader.Instance.collisionEffectPrefabs.Length>pos)
            {
                slot_prefab = Game_Loader.Instance.collisionEffectPrefabs[pos].particleEffect;
                SetImage(Game_Loader.Instance.collisionEffectPrefabs[pos].particlePicture);
                SetButton();
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
        }
        else
        {
            Debug.LogError("Didn't Find Prefab");
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
        slot_button.onClick.AddListener(UpdateLoader); // при нажатии выполняем UpdateLoader
        HasObject = true;
    }
    private void SetImage(Sprite picture)
    {
        slot_image = gameObject.GetComponentsInChildren<Image>()[1];
        slot_image.sprite = picture;
        slot_image.enabled = true;
        slot_button.onClick.AddListener(UpdateLoader); // при нажатии выполняем UpdateLoader
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
        }
        else slot_button.interactable = true;
    }

    /// <summary>
    /// Обновляем текущий объект GameList'а и посылаем обновление инвентаря родителю.
    /// </summary>
    private void UpdateLoader()
    {
        Game_Loader.Instance.ChangeSelectedObject(slot_prefab);
        GetComponentInParent<SlotsUiController>().UpdateSet();
    }
}
