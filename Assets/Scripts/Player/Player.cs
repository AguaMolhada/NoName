using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GunController))]
public class Player : PlayerStats {

    Camera viewCamera;
    GunController gunController;

    protected override void Start()
    {
        base.Start();
        gunController = GetComponent<GunController>();
        viewCamera = Camera.main;
		if (rigbody == null) {
			GetComponent<Rigidbody>();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
        rigbody.MovePosition(rigbody.position + (movement * speed) * Time.fixedDeltaTime);
	}

    void LookAt(Vector3 lookPoint)
    {
        Vector3 point = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
        transform.LookAt(point);
    }

    void Update()
    {

        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            LookAt(point);
        }

        if (Input.GetButton("Shoot"))
        {
            gunController.OnTriggerHold();
        }
        if (Input.GetButtonUp("Shoot"))
        {
            gunController.OnTriggerRelease();
        }

        if (Input.GetButtonDown("Change Weapon"))
        {
            gunController.ChangeGun();
        }
        if (Input.GetButton("Reload"))
        {
            gunController.equippedGun.GetComponent<Gun>().ForceReload();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            gunController.ChangeFireMode();
        }
    }

}
