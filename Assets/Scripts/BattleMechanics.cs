using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMechanics : MonoBehaviour
{
    private Animator anim;
    private bool firstAttack;
    private bool secondAttack;
    public string activeWeapon;
    float _time = 0f;
    public float maxComboDelay = 1.3f;
    public float firstAttackTimer;
    private playerMove _playerMove;
    CharacterController _characterController;
    private BoxCollider[] allColliders;
    public BoxCollider swordCollider;
    public GameObject Arrow;

    private void Start()
    {
        anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        _playerMove = GameObject.FindWithTag("Player").GetComponent<playerMove>();
    }

    void Update()
    {
        if (activeWeapon == "Sword")
        {
            _time += Time.deltaTime; // sayac mantigi
            if (Input.GetMouseButtonDown(0) && !_playerMove.canRoll && !_playerMove.canDash) // eger ki sol mouse'a basildiysa
                OnClickSword();
            if (_time >= maxComboDelay) // zaman max combo suresini gecti ise attacklar sifirlaniyor.
            {
                firstAttack = false;
                secondAttack = false;
                _playerMove._characterSpeed = 4;
                _time = 0;
            }
            // eger ki ilk vurus yapildiysa ve ikinci vurus daha yapilmadiysa normal hiza geri doner.
            if (!secondAttack && firstAttack && _time >= firstAttackTimer + 0.1f)
                _playerMove._characterSpeed = 4;
        }
        else if (activeWeapon == "Spear")
        {
            _time += Time.deltaTime;
            if (Input.GetMouseButtonDown(0) && !_playerMove.canRoll && !_playerMove.canDash)
                OnClickSpear();
            if (_time >= maxComboDelay)
            {
                firstAttack = false;
                secondAttack = false;
                _playerMove._characterSpeed = 4;
                _time = 0;
            }
            if (!secondAttack && firstAttack && _time >= firstAttackTimer + 0.1f)
                _playerMove._characterSpeed = 4;
        }
        else if (activeWeapon == "Shield")
        {
            _time += Time.deltaTime;
            if (Input.GetMouseButtonDown(1) && !_playerMove.canRoll && !_playerMove.canDash)
            {
                OnClickShield();
                _playerMove.isGuard = true;
            }
            if (Input.GetMouseButtonUp(1))
            {
                anim.SetBool("ShieldGuard", false);
                _playerMove.isGuard = false;
            }
        }
        else if (activeWeapon == "Bow")
        {
            if (Input.GetMouseButtonDown(0))
            {
                anim.SetBool("BowAim", true);
                _playerMove.isGuard = true;
            }
            if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("BowAim", false);
                anim.SetTrigger("BowRelease");
                OnClickBow();
                StartCoroutine(BowDelay());
            }
        }

    }
    void OnClickSword()
    {
        // ilk atack yapilmadiysa ilk attack'i yapar ve sureyi sifirlar ayriyeten playerin hizini yariya dusurur.
        if (!firstAttack)
        {
            anim.SetTrigger("Attack1");
            firstAttack = true;
            if (_characterController.isGrounded)
                _playerMove._characterSpeed = 1;
            _time = 0;
        }
        // ilk atack yapildiysa ve ilk attack yapildiktan sonra 0.7 saniye gectiyse ikinci attagi yap
        // sureyi 0.8 e cek bu sayede 2. attack son kombo saniyesinde yapilsa dahi direk olarak _time sifirlanmasin.
        if (firstAttack && !secondAttack && _time >= firstAttackTimer)
        {
            anim.SetTrigger("Attack2");
            if (_characterController.isGrounded)
                GameObject.FindWithTag("Player").GetComponent<playerMove>()._characterSpeed = 1;
            secondAttack = true;
            _time = firstAttackTimer + 0.1f;
        }
    }
    void OnClickSpear()
    {
        if (!firstAttack)
        {
            anim.SetTrigger("sAttack1");
            firstAttack = true;
            if (_characterController.isGrounded)
                _playerMove._characterSpeed = 1;
            _time = 0;
        }
        if (firstAttack && !secondAttack && _time >= firstAttackTimer)
        {
            anim.SetTrigger("sAttack2");
            if (_characterController.isGrounded)
                _playerMove._characterSpeed = 1;
            secondAttack = true;
            _time = firstAttackTimer + 0.1f;
        }
    }
    void OnClickShield()
    {
        anim.SetBool("ShieldGuard", true);
    }
    void OnClickBow()
    {
        GameObject _arrow;
        if (_playerMove._lookRight)
        {
            _arrow = Instantiate(Arrow, new Vector3(transform.position.x + 0.2f, transform.position.y + 1.5f), Quaternion.identity);
            _arrow.GetComponent<Rigidbody>().AddForce(100, 0, 0);
        }
        else
        {
            _arrow = Instantiate(Arrow, new Vector3(transform.position.x - 0.2f, transform.position.y + 1.5f), Quaternion.identity);
            _arrow.GetComponent<Rigidbody>().AddForce(-100, 0, 0);
        }
    }

    void collidersetActive() // kilicin collider'ini bulur ve onu aktif eder.
    {
        allColliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider item in allColliders)
        {
            if (item.CompareTag("Weapon") || item.CompareTag("Spear") || item.CompareTag("Shield"))
            {
                swordCollider = item;
                break;
            }
        }
        if (swordCollider != null)
            swordCollider.enabled = true;
    }

    void colliderSetDeactive()
    {
        allColliders = gameObject.GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider item in allColliders)
        {
            if (item.CompareTag("Weapon") || item.CompareTag("Spear") || item.CompareTag("Shield"))
            {
                swordCollider = item;
                break;
            }
        }
        if (swordCollider != null)
            swordCollider.enabled = false;

    }

    IEnumerator BowDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _playerMove.isGuard = false;
    }
}
