using UnityEngine;
using UnityEngine.EventSystems;

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

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _animator = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        // ����
        _moveDirection.x = Input.GetAxis("Horizontal");
        if (_controller.isGrounded) _moveDirection.y = 0;
        _moveDirection.z = Input.GetAxis("Vertical");
        
        // ��]
        if (_moveDirection.x > 0)
            transform.eulerAngles = new Vector3(0, 180 ,0);
        if (_moveDirection.x < 0)
            transform.eulerAngles = new Vector3(0, 0 ,0);
        
        float speed = _walkSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) speed = _runSpeed;

        _moveDirection.y -= _gravity * Time.deltaTime;
        _controller.Move(_moveDirection * speed * Time.deltaTime);   
    }

    private void AnimHandler(Vector3 moveDirection)
    {
        // _animator.SetBool("IsIdole", true);
    }
}
