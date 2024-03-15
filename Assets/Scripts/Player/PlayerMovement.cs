using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class PlayerMovement : MonoBehaviour
{
    [Header("���s���x")]
    [SerializeField]
    private float _walkSpeed;
    [Header("���s���x")]
    [SerializeField]
    private float _runSpeed;
    [Header("�d�͌W��")]
    [SerializeField]
    private float _gravity;
    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private bool _inRoom = false;
    public event Action<bool> ActionInRoom;
    private int _currentRoomNum;
    public int CurentRoomNum => _currentRoomNum;
    private bool _inStair = false;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // ����
        _moveDirection.x = Input.GetAxis("Horizontal");
        if (_controller.isGrounded) _moveDirection.y = 0;
        _moveDirection.z = Input.GetAxis("Vertical");

        // ��]
        if (_moveDirection.x > 0)
            _spriteRenderer.flipX = true;
        if (_moveDirection.x < 0)
            _spriteRenderer.flipX = false;
        
        float speed = _walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed = _runSpeed;

        _moveDirection.y -= _gravity * Time.deltaTime;
        _controller.Move(_moveDirection * speed * Time.deltaTime);   
    }

    private void AnimHandler(Vector3 moveDirection)
    {
        // _animator.SetBool("IsIdole", true);
    }

    public void ChangeInRoom(bool state)
    {
        if (_inRoom == state) return;
        _inRoom = state;
        ActionInRoom?.Invoke(_inRoom);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            _currentRoomNum = other.gameObject.GetComponent<RoomDetails>().RoomNum;
            ChangeInRoom(true);
        }
        if (other.CompareTag("OutLineCol") && !_inStair)
        {
            _inStair = true;
            gameObject.layer = LayerMask.NameToLayer("Outline");
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
            ChangeInRoom(false);

        if (other.CompareTag("OutLineCol") && _inStair)
        {
            _inStair = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
        }
    }
}
