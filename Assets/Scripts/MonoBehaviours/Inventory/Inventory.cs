using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;

    public const int numSlots = 5;

    Image[] itemImages = new Image[numSlots];

    Item[] items = new Item[numSlots];

    GameObject[] slots = new GameObject[numSlots];
    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateSlots()
    {
        if (slotPrefab != null)
        {
            for (int i=0; i<numSlots; i++)
            {
                GameObject newSlots = Instantiate(slotPrefab);
                newSlots.name = "ItemSlot_"+i;
                newSlots.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlots;
                itemImages[i] = newSlots.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                // Adding when not empty
                items[i].quantity = items[i].quantity + 1;
                Slot slotScript = slots[i].GetComponent<Slot>();
                Text quantityText = slotScript.qtyText;
                quantityText.enabled = true;
                quantityText.text = items[i].quantity.ToString();
                return true;
            }

            if (items[i] == null)
            {
                // Adding when empty
                // Copy item and add to inventory (so modify original Scriptable Object)
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }
}
