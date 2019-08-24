using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class SlotUpdate : MonoBehaviour
{
    public bool HasObject { get; private set;} //настроен ли объект
    private Image slot_image;
    private GameObject slot_prefab;
    private ParticleSystem slot_Particle;
    public Button slot_button { get; set; }
    private GameObject slot_blocker;
    private int price;
    private GameObject priceTagPrefab;
    private SlotsUiController parentController;

    private void Start()
    {
        int pos = int.Parse(gameObject.name);
        slot_button = gameObject.GetComponent<Button>();
        if (gameObject.GetComponentInParent<SlotsUiController>() != null)
        {
            parentController = gameObject.GetComponentInParent<SlotsUiController>();
        }
        else Debug.LogWarning("Couldn't find parent control script in " + gameObject.transform.parent.name);

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
                price = 100 + (pos + 1) * 10;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.transform.parent.name == "Jump_Effect_Holder")
        {
            if (Game_Loader.Instance.jumpEffectPrefabs.Count > pos)
            {
                slot_prefab = Game_Loader.Instance.jumpEffectPrefabs[pos];
                SetParticle(new Vector2(10,10));
                SetButton();
                price = (pos + 1) * 20;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else if (gameObject.transform.parent.name == "Collision_Effect_Holder")
        {
            if (Game_Loader.Instance.collisionEffectPrefabs.Count > pos)
            {
                slot_prefab = Game_Loader.Instance.collisionEffectPrefabs[pos];
                SetParticle(new Vector2(50,50));
                SetButton();
                price = (pos + 1) * 5;
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
                price = (pos+1)*10;
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
            if (pos == parentController.unlockedSkins + 1)
            {
                slot_button.onClick.AddListener(ShowBuyWindow);
                Debug.Log("Listener is added ShowBuyWindow " + gameObject.transform.parent.name + " slot №" + gameObject.name);
                slot_blocker.SetActive(true);
                SetPriceTag();
            }
        }
        // если слот заполенный но ещё заблокирован
        if (slot_blocker != null && HasObject && pos > parentController.unlockedSkins+1)
        {
            //если это система частиц, то блокируем её, нет - блокируем спрайт.
            if (slot_Particle == null)
            {
                slot_image.enabled = false;
            }
            else
            {
                slot_Particle.gameObject.SetActive(false);
            }
            slot_button.interactable = false;
            slot_blocker.SetActive(true);
        }
    }
    private void OnEnable()
    {
        if (slot_Particle != null)
        {
            slot_Particle.Play();
        }
    }
    private void SetPriceTag()
    {
        priceTagPrefab = Instantiate(parentController.price_Tag, gameObject.transform);
        Text price_tag = priceTagPrefab.GetComponentInChildren<Text>();
        price_tag.text = price.ToString();
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
    private void SetParticle(Vector2 scale)
    {
        GameObject particleObj = Instantiate(slot_prefab, transform.position, Quaternion.identity);
        slot_Particle = particleObj.GetComponent<ParticleSystem>();
        UnityEngine.UI.Extensions.UIParticleSystem UIParticle;
        Material previousMaterial;
        if (slot_prefab.transform.childCount>0)
        {
            foreach (Transform child in particleObj.transform)
            {
                previousMaterial = child.gameObject.GetComponent<ParticleSystemRenderer>().material;
                UIParticle = child.gameObject.AddComponent<UnityEngine.UI.Extensions.UIParticleSystem>();
                UIParticle.material = previousMaterial;
                UIParticle.rectTransform.sizeDelta = Vector2.one;
            }
        }
        previousMaterial = particleObj.GetComponent<ParticleSystemRenderer>().material;
        UIParticle = particleObj.AddComponent<UnityEngine.UI.Extensions.UIParticleSystem>();
        UIParticle.material = previousMaterial;
        particleObj.transform.SetParent(gameObject.transform);
        particleObj.transform.localPosition = Vector3.zero;
        particleObj.transform.localScale = scale;
        UIParticle.rectTransform.sizeDelta = gameObject.GetComponent<RectTransform>().sizeDelta / scale;
        ParticleSystem.MainModule main = slot_Particle.main;
        main.loop = true;
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
            parentController.ChangeCurrentObject(int.Parse(gameObject.name));
        }
        //if (slot_prefab.name)
        else slot_button.interactable = true;
    }

    private void ShowBuyWindow()
    {
        Debug.Log("Blocked item selected");
        GameObject buy_Confirmation_Panel = parentController.buy_Confirmation_Panel;
        Text buy_panel_text = buy_Confirmation_Panel.GetComponentsInChildren<Text>()[0];
        buy_panel_text.text = $"Do you want to buy this item for {price}?";

        if(!buy_Confirmation_Panel.activeSelf)
            buy_Confirmation_Panel.SetActive(true);
        Button buy_confirmation = buy_Confirmation_Panel.GetComponentInChildren<Button>();
        buy_confirmation.interactable = true;
            buy_confirmation.onClick.AddListener(BuyButtonPressed);
            Debug.Log("Listener is added BuyButtonPressed");
        Button buy_cancel = buy_Confirmation_Panel.GetComponentsInChildren<Button>()[1];
        buy_cancel.onClick.AddListener(CancelButtonPressed);
        Debug.Log("Listener added CancelButtonPressed");
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
    }

    private void BuyButtonPressed()
    {
        GameObject buy_Confirmation_Panel = parentController.buy_Confirmation_Panel;
        Button buy_confirmation = buy_Confirmation_Panel.GetComponentInChildren<Button>();
        if (Game_Loader.Instance.gems<price)
        {
            Text buy_panel_text = buy_Confirmation_Panel.GetComponentsInChildren<Text>()[0];
            buy_panel_text.text = $"Not enough {price-Game_Loader.Instance.gems} gems.";
            buy_confirmation.interactable = false;
        }
        else
        {
            Game_Loader.Instance.UpdateGems(-price);
            slot_blocker.SetActive(false);
            buy_Confirmation_Panel.SetActive(false);
            parentController.UnlockSkin(int.Parse(gameObject.name));
            Destroy(priceTagPrefab);
            slot_button.onClick.RemoveListener(ShowBuyWindow);
            Debug.Log("Listener is removed ShowBuyWindow");
            UpdateLoader();
        }
        buy_confirmation.onClick.RemoveListener(BuyButtonPressed);
        Debug.Log("Listener is removed BuyButtonPressed");
    }
    private void CancelButtonPressed()
    {
        GameObject buy_Confirmation_Panel = parentController.buy_Confirmation_Panel;
        Button buy_button = buy_Confirmation_Panel.GetComponentsInChildren<Button>()[0];
        Button cancel_button = buy_Confirmation_Panel.GetComponentsInChildren<Button>()[1];
        buy_button.onClick.RemoveListener(BuyButtonPressed);
        cancel_button.onClick.RemoveListener(CancelButtonPressed);
        Debug.Log("Listener CancelButtonPressed and BuyButtonPressed is removed");

    }

}
