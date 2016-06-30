using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GunController : MonoBehaviour {

    public Transform weaponHold;
    public GameObject equippedGun;
    public int gunID = 0;
    public int FireModeID = 0;
    public GameObject[] guns;


    void Start()
    {
        GameObject tempAk = Instantiate(guns[0], weaponHold.position, weaponHold.rotation) as GameObject;
        guns[0] = tempAk;
		tempAk.GetComponent<Gun>().GunMesh[0].enabled = false;
        tempAk.GetComponent<MeshCollider>().enabled = false;
        tempAk.GetComponent<Gun>().isPickable = false;
        tempAk.transform.parent = weaponHold;
        GameObject tempPistol = Instantiate(guns[1], weaponHold.position, weaponHold.rotation) as GameObject;
        guns[1] = tempPistol;
		tempPistol.GetComponent<Gun>().GunMesh[0].enabled = false;
        tempPistol.GetComponent<MeshCollider>().enabled = false;
        tempPistol.GetComponent<Gun>().isPickable = false;
        tempPistol.transform.parent = weaponHold;
        GameObject tempMp5 = Instantiate(guns[2], weaponHold.position, weaponHold.rotation) as GameObject;
        guns[2] = tempMp5;
        tempMp5.GetComponent<Gun>().GunMesh[0].enabled = false;
        tempMp5.GetComponent<MeshCollider>().enabled = false;
        tempMp5.GetComponent<Gun>().isPickable = false;
        tempMp5.transform.parent = weaponHold;
        GameObject tempXm1014 = Instantiate(guns[3], weaponHold.position, weaponHold.rotation) as GameObject;
        guns[3] = tempXm1014;
        tempXm1014.GetComponent<Gun>().GunMesh[0].enabled = false;
        tempXm1014.GetComponent<MeshCollider>().enabled = false;
        tempXm1014.GetComponent<Gun>().isPickable = false;
        tempXm1014.transform.parent = weaponHold;
        GameObject tempMiniGun = Instantiate(guns[4], weaponHold.position, weaponHold.rotation) as GameObject;
        guns[4] = tempMiniGun;
        tempMiniGun.GetComponent<Gun>().GunMesh[0].enabled = false;
        tempMiniGun.GetComponent<Gun>().GunMesh[1].enabled = false;
        tempMiniGun.GetComponent<BoxCollider>().enabled = false;
        tempMiniGun.GetComponent<Gun>().isPickable = false;
        tempMiniGun.transform.parent = weaponHold;
        EquipGun();
    }

    public void EquipGun()
    {
        equippedGun = guns[gunID];
		for (int i = 0; i < equippedGun.GetComponent<Gun>().GunMesh.Length; i++) {
			equippedGun.GetComponent<Gun>().GunMesh[i].enabled = true;
		}
        equippedGun.GetComponent<Gun>().isActive = true;
        equippedGun.GetComponent<Gun>().isPickable = false;
        equippedGun.transform.parent = weaponHold;
    }

    public void OnTriggerHold()
    {
        if (equippedGun != null)
        {
            equippedGun.GetComponent<Gun>().OnTriggerHold();
        }
    }

    public void OnTriggerRelease()
    {
        if (equippedGun != null)
        {
            equippedGun.GetComponent<Gun>().OnTriggerRelease();
        }
    }

    void checkGunID()
    {
        if (gunID == guns.Length)
        {
            gunID = 0;
        }
    }

    public void ChangeGun()
    {
        gunID++;
        
		checkGunID();
        
        if (guns[gunID].GetComponent<Gun>().ammo == 0)
        {
			gunID++;
        }
		checkGunID();

        EquipGun();
        for (int i = 0; i < guns.Length; i++)
        {
            if (i != gunID)
            {
                guns[i].GetComponentInChildren<MeshRenderer>().enabled = false;
				for (int k = 0; k < guns[i].GetComponent<Gun>().GunMesh.Length; k++) {
					guns[i].GetComponent<Gun>().GunMesh[k].enabled = false;
				}
                guns[i].GetComponent<Gun>().isActive = false;
            }
        }
    }

    public void ChangeFireMode()
    {

        FireModeID++;
        if (FireModeID >= equippedGun.GetComponent<Gun>().fireModeMaxCount)
        {
            FireModeID = 0;
        }
        if (!equippedGun.GetComponent<Gun>().haveSingle)
        {
            if (FireModeID == 0)
            {
                FireModeID = 1;
            }
        }
        if (!equippedGun.GetComponent<Gun>().haveAuto)
        {
            if (FireModeID == 1)
            {
                FireModeID = 2;
            }
        }
        if (!equippedGun.GetComponent<Gun>().haveBurst)
        {
            if (FireModeID == 2)
            {
                FireModeID = 0;
            }
        }
        equippedGun.GetComponent<Gun>().fireMode = (Gun.FireMode)FireModeID;
    }

    public void AddAmmo(string name, int ammout)
    {
        if (equippedGun.name == name)
        {
            equippedGun.GetComponent<Gun>().AddAmmo(ammout);    
        }

    }
}
