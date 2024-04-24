using UnityEngine;

public class LeaveRoomState: IRoomAIState
{
    private InnNPCMover _innNpcMover;
    private Vector3 _targetPos;
    private bool _isWalk;

    public bool IsWalk => _isWalk;
    private bool _launchState;
    public bool IsStateFin => _innNpcMover.IsAchieved && _launchState;

    private int _targetRoomNum;

    public LeaveRoomState(InnNPCMover mover)
    {
        _innNpcMover = mover;
    }

    // �X�e�[�g�ɓ��������̏���
    public void EnterState(Vector3 pos, int targetRoom)
    {
        _targetPos = pos;
        _innNpcMover.SetTarGetPos(_targetPos);
        _targetRoomNum = targetRoom;
        _isWalk = true;
        _launchState = true;
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _innNpcMover.Moving();
    }
    
    public void ExitState()
    {
        _launchState = false;
    }
}
