using UnityEngine;

public class StayRoomState : IRoomAIState
{
    protected GameObject _npc;
    protected InnNPCMover _innNpcMover;
    private RoomBunker _roomBunker;
    private Vector3 _entryTargetPos;
    private Vector3 _errorVector;
    private bool _isEntry;
    private float _remainStateTime;
    protected bool _isWalk;

    public bool IsWalk => _isWalk;

    private bool _launchState;
    public bool IsStateFin => (_remainStateTime <= 0) && _launchState;

    public StayRoomState(InnNPCMover mover, Vector3 errorVector)
    {
        _innNpcMover = mover;
        _npc = _innNpcMover.Character;
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
        _errorVector = errorVector;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState(Vector3 pos, int targetRoom)
    {
        _entryTargetPos = pos;
        _innNpcMover.SetTarGetPos(_entryTargetPos);
        _remainStateTime = _roomBunker.RoomDetails[targetRoom].RemainTime;
        _isEntry = false;

        if (_entryTargetPos == _errorVector) _isEntry = true;
        _launchState = true;
    }

    // �X�e�[�g�̍X�V

    public void UpdateState()
    {
        // ����
        if (!_isEntry)
        {
            _isWalk = true;
            // �����ɓ���܂ł̈ړ�
            _innNpcMover.Moving();
            if (_innNpcMover.IsAchieved) _isEntry = true;
            
            return;
        }

        DoAction();

        // �X�e�[�g�J�E���g�_�E��
        _remainStateTime -= Time.deltaTime;
    }

    protected virtual void DoAction()
    {
    }

    public void ExitState()
    {
        _launchState = false;
    }
}
