using UnityEngine;
using System.Collections;

public class Gun : GunTypeSpecs
{

    [HideInInspector]
    public int fireModeMaxCount;

    public string weaponName;
    public bool isActive;
    public Transform[] muzzle;
    public Projectile projectile;
    public Transform shellEjection;
    public Transform shell;
    public Transform Barrel;
    public MeshRenderer[] GunMesh;

    float nextShotTime;
    bool triggerReleasedSinceLastShot;
    int burstShotsRemaining;
    float originalNsBetweenShoots;
    float timeTriggerHold;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;

    public bool isReloading = false;
    public bool isPickable = true;

    MuzzleFlash muzzleflash;
    float rotationSpeed = 90;

    void Start()
    {
        fireModeMaxCount = System.Enum.GetValues(typeof(FireMode)).Length;
        muzzleflash = GetComponent<MuzzleFlash>();
        burstShotsRemaining = burstCount;
    }

    void Shoot()
    {
        if (!isPickable && isActive)
        {
            if (Time.time > nextShotTime)
            {
                if (!isReloading && Time.time > nextShotTime && cartridgeAmmo > 0)
                {
                    if (fireMode == FireMode.Burst)
                    {
                        if (burstShotsRemaining == 0)
                        {
                            return;
                        }
                        burstShotsRemaining--;
                        recoilMoveSettleTime = msBetweenShootsBurst / 1000;
                        originalNsBetweenShoots = msBetweenShootsBurst;
                    }

                    else if (fireMode == FireMode.Single)
                    {
                        if (!triggerReleasedSinceLastShot)
                        {
                            return;
                        }
                    }

                    if (fireMode != FireMode.Burst)
                    {
                        recoilMoveSettleTime = msBetweenShootsAuto / 1000;
                        originalNsBetweenShoots = msBetweenShootsAuto;
                    }

                    TypeShoot();

                    Instantiate(shell, shellEjection.position, shellEjection.rotation);
                    muzzleflash.GetComponent<MuzzleFlash>().Activate();

                    transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
                }
            }
        }
    }

    void TypeShoot()
    {

        if (gunType == GunType.Shotgun)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                if (cartridgeAmmo == 0)
                {
                    break;
                }
                nextShotTime = Time.time + originalNsBetweenShoots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
                newProjectile.SetDamage(bulletDamage + (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentGunLevel - 1) * 5);
                newProjectile.SetSpeed(muzzleVelocity);
            }
            cartridgeAmmo--;
        }
        if (gunType == GunType.Minigun)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                if (cartridgeAmmo == 0)
                {
                    break;
                }
                Barrel.transform.Rotate(0, 0, msBetweenShootsAuto * 1000 * Time.deltaTime, Space.Self);
                nextShotTime = Time.time + originalNsBetweenShoots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation * Quaternion.Euler(new Vector3(0, Random.Range(-timeTriggerHold, timeTriggerHold + 1)))) as Projectile;
                cartridgeAmmo--;
                newProjectile.SetDamage(bulletDamage + (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentGunLevel - 1) * 5);
                newProjectile.SetSpeed(muzzleVelocity);

                Instantiate(shell, shellEjection.position, shellEjection.rotation);
                muzzleflash.GetComponent<MuzzleFlash>().Activate();

            }
        }
        if (gunType == GunType.Pistol)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                if (cartridgeAmmo == 0)
                {
                    break;
                }
                nextShotTime = Time.time + originalNsBetweenShoots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
                cartridgeAmmo--;
                newProjectile.SetDamage(bulletDamage + (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentGunLevel - 1) * 5);
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }
        if (gunType == GunType.Rifle)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                if (cartridgeAmmo == 0)
                {
                    break;
                }
                nextShotTime = Time.time + originalNsBetweenShoots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
                cartridgeAmmo--;
                newProjectile.SetDamage(bulletDamage + (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentGunLevel - 1) * 5);
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }
        if (gunType == GunType.SubMachine)
        {
            for (int i = 0; i < muzzle.Length; i++)
            {
                if (cartridgeAmmo == 0)
                {
                    break;
                }
                nextShotTime = Time.time + originalNsBetweenShoots / 1000;
                Projectile newProjectile = Instantiate(projectile, muzzle[i].position, muzzle[i].rotation) as Projectile;
                cartridgeAmmo--;
                newProjectile.SetDamage(bulletDamage + (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().currentGunLevel - 1) * 5);
                newProjectile.SetSpeed(muzzleVelocity);
            }
        }
    }

    public void OnTriggerHold()
    {
        timeTriggerHold += Time.deltaTime;
        if (timeTriggerHold >= 7)
        {
            timeTriggerHold = 7;
        }
        Shoot();
        triggerReleasedSinceLastShot = false;
    }

    public void OnTriggerRelease()
    {
        triggerReleasedSinceLastShot = true;
        burstShotsRemaining = burstCount;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.name == "Player" && isPickable)
        {
            foreach (var x in col.gameObject.GetComponentsInChildren<Gun>())
            {
                if (x.weaponName == weaponName)
                {
                    x.AddAmmo(ammo);
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }

    void Update()
    {
        if (!isPickable && isActive)
        {
            if (cartridgeAmmo == 0 && !isReloading && ammo > 0)
            {
                StartCoroutine(Reload());
            }
        }

        if (ammo >= MaxAmmo)
        {
            ammo = MaxAmmo;
        }
    }

    void LateUpdate()
    {
        // animate recoil
        if (!isPickable && isActive)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);

        }
        if (isPickable && !isActive)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.World);
        }
    }

    public void ForceReload()
    {
        StartCoroutine(Reload());
    }


    IEnumerator Reload()
    {
        isReloading = true;
        float reloadSpeed = 1f / reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        if (ammo > 0)
        {

            ammo -= cartidgeMaxAmmo - cartridgeAmmo;
            if (ammo < 0)
            {
                cartridgeAmmo = cartidgeMaxAmmo + ammo;
                ammo = 0;
            }
            else
            {
                cartridgeAmmo = cartidgeMaxAmmo;
            }
            yield return new WaitForSeconds(reloadTime);
        }
        burstShotsRemaining = burstCount;
        transform.localEulerAngles = Vector3.zero;
        isReloading = false;
    }

    public void AddAmmo(int x)
    {
        ammo += x;
    }
}
