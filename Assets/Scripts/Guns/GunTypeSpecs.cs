using UnityEngine;
using System.Collections;

public class GunTypeSpecs : MonoBehaviour {
	
    //Gun Type for shooting
	public enum GunType{ Pistol, SubMachine, Rifle, Shotgun, Minigun };
	public GunType gunType;

	public enum FireMode { Auto = 1, Burst = 2, Single = 0 };
	public FireMode fireMode;

	public bool haveBurst;
	public int burstCount;
	public bool haveAuto;
	public bool haveSingle;

	public float msBetweenShootsBurst = 100; //Miliseconds Between each shoot//
	public float armorPenetration;
	public float bulletDamage;
	public float reloadTime; //In Seconds
	public int ammo;
	public int MaxAmmo;
	public int cartridgeAmmo;
	public int cartidgeMaxAmmo;
	public float msBetweenShootsAuto = 100; //Miliseconds Between each shoot//
	public float muzzleVelocity = 35;   //Speed for bullet//

    //Recoil Vars//
    public Vector2 kickMinMax = new Vector2(.05f, .2f);
    public float recoilMoveSettleTime = .1f;

	void Awake(){
        if (!haveBurst)
        {
            burstCount = 0;
            msBetweenShootsBurst = 0;
        }
        if (!haveAuto)
        {
            msBetweenShootsAuto = 50;
        }

	}


}
