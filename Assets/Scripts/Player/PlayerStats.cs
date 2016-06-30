using UnityEngine;
using System.Collections;

public class PlayerStats : LivingEntity
{

    [SerializeField]
    protected Rigidbody rigbody;
    protected int _goldAmount;
    protected int _currentGunLevel;

    public int currentGunLevel { get { return _currentGunLevel + 1; } }
    public int goldAmount { get { return _goldAmount; } }

    public void addGunLevel()
    {
        _currentGunLevel++;
    }

    public void addGold(int x)
    {
        _goldAmount += x;
    }

    public void Heal(float x)
    {
        health += x;
        if (health >= 100)
        {
            health = 100;
        }
    }

}
