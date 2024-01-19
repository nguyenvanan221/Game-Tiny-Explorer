using System.Collections;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;

    public Inventory inventoryPrefab;
    Inventory inventory;

    public HealthBar healthBarPrefab;
    HealthBar healthBar;

    private void OnEnable()
    {
        ResetCharacter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    { 
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitObject = collision.gameObject.GetComponent<Consumable>().item;

            if (hitObject != null)
            {
                bool shouldDisapper = false;

                print("Hit: " + hitObject.name);

                switch (hitObject.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisapper = inventory.AddItem(hitObject);
                        break;
                    case Item.ItemType.HEALTH:
                         shouldDisapper = AdjustHitPoints(hitObject.quantity);
                        break;
                    default:
                        break;
                }

                if (shouldDisapper) 
                {
                    collision.gameObject.SetActive(false);
                }
                    
            }

         
        }
    }

    public bool AdjustHitPoints(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjust HP by: " + amount + ". New value: " + hitPoints.value);
            return true;
        }

        print("didnt adjust hitpoints");
        return false;
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value <= float.Epsilon)
            {
                KillCharacter();
                break;
            }

            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCharacter()
    {
        base.KillCharacter();
        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }
}
