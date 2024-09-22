using UnityEngine;

public class GoToRoomState : IRoomAIState
{
    // ����p�^�[��
    protected enum AvoidPatterns
    {
        Wait,
        Move
    }
    private InnNPCMover _innNpcMover;
    protected GameObject _npc;

    // ���݂̉���p�^�[��
    protected AvoidPatterns _currentAvoid = AvoidPatterns.Move;
    // �^�[�Q�b�g�̈ʒu
    protected Vector3 _targetPos;
    // ���s�t���O
    private bool _isWalk;
    // ���s�t���O��getter
    public bool IsWalk => _isWalk;
    private bool _launchState;
    // �X�e�[�g�I���t���O��getter
    public bool IsStateFin => _innNpcMover.IsAchieved && _launchState;
    private bool _currentDirection = true;

    public GoToRoomState(InnNPCMover mover)
    {
        _innNpcMover = mover;
        _npc = _innNpcMover.Character;
    }

    public void EnterState(Vector3 pos, int targetRoom)
    {
        _targetPos = pos;
        _innNpcMover.SetTarGetPos(_targetPos);
        _launchState = true;
    }

    public void UpdateState()
    {
        if (IsAvoidingObstacle())
            if (_currentAvoid == AvoidPatterns.Wait) 
                _isWalk = false;
            else 
                AvoidMoving();
        else
            DefaultMoving();
    }

    public void ExitState()
    {
        _launchState = false;
    }

    // �f�t�H���g�̈ړ�
    private void DefaultMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos);
        _innNpcMover.Moving();
    }

    // ������̈ړ�
    private void AvoidMoving()
    {
        _isWalk = true;
        MoveTowardsTarget(_targetPos, reverseDirection: true);
        _innNpcMover.Moving();
    }

    // �^�[�Q�b�g�Ɍ������Ĉړ�
    private void MoveTowardsTarget(Vector3 target, bool reverseDirection = false)
    {
        if (_currentDirection == reverseDirection) return;
        _currentDirection = reverseDirection;
        _innNpcMover.SetTarGetPos((reverseDirection ? -target : target));
    }

    // ��Q����������邩�ǂ����𔻒�
    protected virtual bool IsAvoidingObstacle()
    {
        return false;
    }

}
