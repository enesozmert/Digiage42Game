using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    private KeyCode pickupKey = KeyCode.E;
    public GameObject currentWeapon;
    private BattleMechanics _battleMechanics;
    public Transform hand;

    private void Start()
    {
        _battleMechanics = GameObject.FindGameObjectWithTag("Player").GetComponent<BattleMechanics>();
    }
    void Update()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        
        if (Physics.Raycast(ray, out hit, 1))
        {
            if ((hit.transform.CompareTag("Weapon") || hit.transform.CompareTag("Spear")
                || hit.transform.CompareTag("Shield") || hit.transform.CompareTag("Bow")) && Input.GetKeyDown(pickupKey))
            {
                if (currentWeapon != null)
                    Destroy(currentWeapon);
                currentWeapon = Instantiate(hit.transform.gameObject, hand.transform.position, hand.transform.rotation, hand);
                currentWeapon.GetComponent<BoxCollider>().enabled = false;
                Handle(currentWeapon.tag, hit.transform.gameObject);
            }
        }
    }
    void Handle(string tag, GameObject weapon)
    {
        if (tag == "Spear")
        {
            _battleMechanics.firstAttackTimer = 0.8f;
            _battleMechanics.maxComboDelay = 1.6f;
            _battleMechanics.activeWeapon = "Spear";
            PositionChanger(new Vector3(0.21f, -0.11f, 0.074f), new Vector3(-36.594f, -110.574f, -92.972f)); // kilica gore elin transformunu ayarlar
        }
        else if (tag == "Weapon")
        {
            _battleMechanics.firstAttackTimer = 0.7f;
            _battleMechanics.maxComboDelay = 1.3f;
            _battleMechanics.activeWeapon = "Sword";
            PositionChanger(new Vector3(-0.193f, 0.054f, 0.004f), new Vector3(0f, -90f, -90f));
        }
        else if (tag == "Shield")
        {
            _battleMechanics.activeWeapon = "Shield";
            PositionChanger(new Vector3(-0.035f, 0.074f, -0.016f), new Vector3(0f, -90f, -90f));
        }
        else if (tag == "Bow")
        {
            _battleMechanics.activeWeapon = "Bow";
            PositionChanger(new Vector3(-0.017f, 0.079f, 0.017f), new Vector3(-2.607f, -13.807f, -105.978f));
        }
    }

    void PositionChanger(Vector3 localPos, Vector3 localAngle)
    {
        hand.transform.localPosition = localPos;
        hand.transform.localEulerAngles = localAngle;
    }
}
