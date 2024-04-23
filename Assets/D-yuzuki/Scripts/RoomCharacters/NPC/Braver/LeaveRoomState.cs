using UnityEngine;

public class LeaveRoomState: IRoomAIState
{
    private InnNPCMover _innNpcMover;
    private Vector3 _targetPos;
    private bool _isWalk;

    public bool IsWalk => _isWalk;
    public bool IsStateFin => _innNpcMover.IsAchieved;

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
    }

    // �X�e�[�g�̍X�V
    public void UpdateState()
    {
        _innNpcMover.Moving();
    }

}
