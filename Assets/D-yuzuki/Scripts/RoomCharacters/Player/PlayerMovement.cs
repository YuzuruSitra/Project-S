using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("���s���x")]
    [SerializeField] private float _walkSpeed;
    [Header("���s���x")]
    [SerializeField] private float _runSpeed;
    [Header("�d�͌W��")]
    [SerializeField] private float _gravity;

    private CharacterController _controller;
    private Vector3 _moveDirection;
    private Vector3 _currentDirection;
    public Vector3 CurrentDirection => _currentDirection;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        HandleMovement();
        UpdateCurrentDirection();
    }

    private void HandleMovement()
    {
        // X���̈ړ�����
        _moveDirection.x = Input.GetAxis("Horizontal");
        // Z���̈ړ����͂́AX�����[���̂Ƃ��̂ݎ󂯕t����
        _moveDirection.z = Input.GetAxis("Vertical");
        // �n�ʂɂ���Ȃ�Y�������̈ړ������Z�b�g
        if (_controller.isGrounded) _moveDirection.y = 0;

        _moveDirection.y -= _gravity * Time.deltaTime;
        // ���s�����s���̑��x������
        float speed = Input.GetKey(KeyCode.LeftShift) ? _runSpeed : _walkSpeed;
        // �v���C���[���ړ�
        _controller.Move(_moveDirection * speed * Time.deltaTime);
    }

    private void UpdateCurrentDirection()
    {
        // ���݂̈ړ��������X�V (Y���͖���)
        Vector3 normalizedDirection = _moveDirection;
        normalizedDirection.y = 0;
        
        if (normalizedDirection != Vector3.zero)
        {
            _currentDirection = normalizedDirection.normalized;
            var x = _currentDirection.x;
            if (x != 0 && x != 0)
            {
                _currentDirection.x = x > 0 ? 1 : -1;
                _currentDirection.z = 0;
            }
        }
    }
}
