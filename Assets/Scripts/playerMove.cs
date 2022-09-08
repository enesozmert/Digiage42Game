using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;

public class playerMove : MonoBehaviour
{
    public InputActionReference _inputs;
    public InputActionReference _rollInput;
    float PlayerZ;
    public bool canRoll = false;
    private bool rollController = true;
    private bool _rollDelay = true;
    public bool canDash;
    public bool isGuard;
    private bool _dashDelay = true;
    private bool dashController = true;
    public float _characterSpeed = 4;
    public bool _lookRight = true;
    private Animator _anim;
    private Vector2 _axsis;
    Vector3 playerVelocity;
    private float gravityValue = -9.81f;
    CharacterController _controller;

    private void OnEnable()
    {
        _inputs.action.Enable();
        _rollInput.action.Enable();
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        PlayerZ = transform.position.z;
    }

    private void Update()
    {
        if (_controller.isGrounded && playerVelocity.y < 0) // karakter fazla baskilanmasin diye velocity'i 0 a esitliyor.
            playerVelocity.y = 0f;
        _axsis = _inputs.action.ReadValue<Vector2>(); // input'u okur ve _axis'e yazar.
        if (Convert.ToBoolean(_rollInput.action.ReadValue<float>()) == true) // ctrl tusuna basildigini algiliyor.
            canRoll = true;
        if ((!canRoll || !_rollDelay) && !isGuard) // roll atmiyor ise
        {
            if (Input.GetButton("Jump") && _controller.isGrounded) // space tusuna basildiysa
                playerVelocity.y += Mathf.Sqrt(1.0f * -3.0f * gravityValue); // karakterin velocity'sini arttiriyor.
            _controller.Move(new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * _characterSpeed); // yatay duzlemde hareket
            _controller.Move(new Vector3(0, playerVelocity.y, 0.0f) * Time.deltaTime); // dikey duzlemde hareket
            if (_axsis.x >= 0.1f) // karakterin en son hangi yone baktigini bulur.
                _lookRight = true;
            else if (_axsis.x <= -0.1f)
                _lookRight = false;
            if (_axsis != Vector2.zero) // hareket var ise karakteri hareket yonune rotate'ler.
                transform.forward = _axsis;
            _anim.SetFloat("Speed", Mathf.Abs(_axsis.x)); // hareket animasyonunu calistirir.
            if (Input.GetKeyDown(KeyCode.K) /* && !_controller.isGrounded*/)
                canDash = true;
            if (Input.GetKeyDown(KeyCode.U))
                _anim.SetTrigger("Dance");
        }
        // ------ ROLL ------- //
        if (canRoll && _rollDelay && !isGuard)
        {
            Rolling();
            if (rollController)
            {
                _anim.SetTrigger("Roll");
                StartCoroutine(rollDelay()); // roll islemini 1 saniye sonra bitirir.
                rollController = false;
            }
        }
        // ------ DASH ------- //
        if (canDash && _dashDelay && !canRoll && !isGuard)
        {
            Dash();
            if (dashController)
            {
                _anim.SetTrigger("Dash");
                StartCoroutine(dashDelay());
                dashController = false;
            }
        }
        // -------------------//
        playerVelocity.y += gravityValue * Time.deltaTime; // yer cekimi
    }

    private void FixedUpdate()
    {
        if (transform.position.z != PlayerZ) // karakterin Z eksenini kilitlemek icin.
            transform.position = new Vector3(transform.position.x, transform.position.y, PlayerZ);
    }

    void Rolling()
    {
        if (_lookRight) // eger ki saga bakiyor ise saga dogru hareket et
            _controller.Move(new Vector3(1, playerVelocity.y - 1 , 0) * Time.deltaTime * _characterSpeed);
        else
            _controller.Move(new Vector3(-1, playerVelocity.y - 1, 0) * Time.deltaTime * _characterSpeed);
    }

    void Dash()
    {
        if (_lookRight)
            _controller.Move(new Vector3(8, playerVelocity.y, 0) * Time.deltaTime * _characterSpeed);
        else
            _controller.Move(new Vector3(-8, playerVelocity.y, 0) * Time.deltaTime * _characterSpeed);
    }

    // 1 saniye sonra yuvarlanma ozelligini kaldirir.
    IEnumerator rollDelay()
    {
        _controller.center = new Vector3(0, 0.43f, 0); // karakterin collider'ini kucultur.
        _controller.height = 0.82f;
        yield return new WaitForSeconds(1);
        _rollDelay = false;
        _controller.center = new Vector3(0, 0.89f, 0);
        _controller.height = 1.8f;
        _characterSpeed = 4;
        canRoll = false;
        rollController = true;
        yield return new WaitForSeconds(1); // karakter 1 saniye ardindan tekrar roll atabilir.
        _rollDelay = true;
        canRoll = false;
    }

    // 1 saniye sonra dash ozelligini kaldirir.
    IEnumerator dashDelay()
    {
        yield return new WaitForSeconds(0.05f);
        _dashDelay = false;
        canDash = false;
        dashController = true;
        yield return new WaitForSeconds(1f);
        _dashDelay = true;
        canDash = false;
    }
}
