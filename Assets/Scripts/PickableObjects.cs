using UnityEngine;
using System.Collections;

public class PickableObjects : MonoBehaviour {

    public enum TypeItem
    {
        Gold,
        Health
    };
    public AudioSource soundFX;
    public TypeItem currentState;
    public float ammout;

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.gameObject.tag == "Player")
        {
            if (currentState == TypeItem.Health)
            {
                col.gameObject.GetComponent<Player>().Heal(ammout);
            }
            if (currentState == TypeItem.Gold)
            {
                col.gameObject.GetComponent<Player>().addGold((int)ammout);
            }
            Destroy(this.gameObject);
        }
    }

    void Update()
    {
        if (currentState != TypeItem.Gold)
        {
            transform.Rotate(0, 90 * Time.deltaTime, 0, Space.World);
        }
    }
}
