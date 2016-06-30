using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Gun))]
public class TransformInspector : Editor {
	
	bool gunFireMode;
	bool gunStats;
    bool prefabs;
    bool recoil;

	public override void OnInspectorGUI() {
        serializedObject.Update();
        Gun t = (Gun)target;
        t.weaponName = EditorGUILayout.TextField("Gun ID", t.weaponName);
		t.gunType = (GunTypeSpecs.GunType)EditorGUILayout.EnumPopup("Gun Type", t.gunType);
		gunFireMode = EditorGUILayout.Foldout(gunFireMode, "Fire Mode");
		if(gunFireMode) {
			t.fireMode = (GunTypeSpecs.FireMode)EditorGUILayout.EnumPopup("Fire Mode", t.fireMode);
			t.haveBurst = EditorGUILayout.Toggle("Have Bust Mode:", t.haveBurst);
			t.haveAuto = EditorGUILayout.Toggle("Have Auto Mode:", t.haveAuto);
			t.haveSingle = EditorGUILayout.Toggle("Have Single Mode:", t.haveSingle);
		}
		gunStats = EditorGUILayout.Foldout(gunStats, "Gun Stats");
		if (gunStats) {
			if(t.haveAuto || t.haveBurst){
				EditorGUILayout.LabelField("Miliseconds Between Shoots", GUILayout.Width(300));
				if(t.haveAuto){
					t.msBetweenShootsAuto = EditorGUILayout.FloatField("(Automatic Mode)",t.msBetweenShootsAuto);
				}
				if(t.haveBurst){
					t.msBetweenShootsBurst = EditorGUILayout.FloatField("(Burst Mode)",t.msBetweenShootsBurst);
				}
				EditorGUILayout.LabelField("______________________________________________________");
			}

			t.armorPenetration = EditorGUILayout.FloatField("Armor Penetration",t.armorPenetration);
			t.bulletDamage = EditorGUILayout.FloatField("Bullet Damage",t.bulletDamage);
			t.reloadTime = EditorGUILayout.FloatField("Reload Time",t.reloadTime);
			t.ammo = EditorGUILayout.IntField("Gun Ammo", t.ammo);
			t.MaxAmmo = EditorGUILayout.IntField("Gun Max Ammo", t.MaxAmmo);
            t.cartridgeAmmo = EditorGUILayout.IntField("Cartidge Ammo", t.cartridgeAmmo);
            t.cartidgeMaxAmmo = EditorGUILayout.IntField("Cartidge Max Ammo", t.cartidgeMaxAmmo);
			t.muzzleVelocity = EditorGUILayout.FloatField("Muzzle Exit Velocity", t.muzzleVelocity);
		}

        prefabs = EditorGUILayout.Foldout(prefabs, "Prefabs to attach");
        if (prefabs)
        {  
            EditorGUILayout.PropertyField(serializedObject.FindProperty("muzzle"), true);
            t.projectile = (Projectile)EditorGUILayout.ObjectField("Weapon Projectile", t.projectile, typeof(Projectile), true) as Projectile;
            t.shellEjection = (Transform)EditorGUILayout.ObjectField("Weapon Shell Ejection", t.shellEjection, typeof(Transform), true) as Transform;
            t.Barrel = (Transform)EditorGUILayout.ObjectField("WeaponBarrel", t.Barrel,typeof(Transform),true) as Transform;
            t.shell = (Transform)EditorGUILayout.ObjectField("Weapon Shell", t.shell, typeof(Transform), true) as Transform;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("GunMesh"), true);
            EditorGUILayout.LabelField("______________________________________________________");
        }

        recoil = EditorGUILayout.Foldout(recoil, " Recoil Var");
        if (recoil)
        {
            t.kickMinMax = EditorGUILayout.Vector2Field("Kick Min Max", t.kickMinMax);
            t.recoilMoveSettleTime = EditorGUILayout.FloatField("Move Settle Time", t.recoilMoveSettleTime);
            EditorGUILayout.LabelField("______________________________________________________");
        }
        serializedObject.ApplyModifiedProperties();
    }

}