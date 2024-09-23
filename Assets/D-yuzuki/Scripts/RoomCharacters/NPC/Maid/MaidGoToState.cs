using UnityEngine;

public class MaidGoToState : GoToRoomState
{
     // ���C�̋���
    private float _rayDistance = 1.0f;
    // �������
    private const float AVOID_DISTANCE = 0.75f;

    public MaidGoToState(InnNPCMover mover) : base(mover)
    {
        _currentAvoid = AvoidPatterns.Move;
    }

    // ���݂̏Փ˃I�u�W�F�N�g
    private GameObject _currentHitObj;

    protected override bool IsAvoidingObstacle()
    {
        Debug.DrawRay(_npc.transform.position, -_npc.transform.forward * _rayDistance, Color.red);
        if (Physics.Raycast(_npc.transform.position, -_npc.transform.forward, out RaycastHit hit, _rayDistance) &&
            hit.collider.CompareTag("RoomBraver") && _currentHitObj == null)
        {
            _currentHitObj = hit.collider.gameObject;
        }
        return CheckObstacleTooFar();
    }

    // ��Q�������ꂷ���Ă��Ȃ������m�F
    private bool CheckObstacleTooFar()
    {
        if (_currentHitObj == null) return false;
        float distanceX = Vector3.Distance(_npc.transform.position, _currentHitObj.transform.position);
        if (distanceX > AVOID_DISTANCE)
        {
            _currentHitObj = null;
            return false;
        }

        return true;
    }
}

