using UnityEngine;

public class BraverGoToState : GoToRoomState
{
     // ���C�̋���
    private float _rayDistance = 1.5f;
    // �������
    private const float AVOID_DISTANCE = 1.5f;
    // ����X�e�[�g����臒l
    private const float AVOID_THRESHOLD = 0.0f;

    public BraverGoToState(InnNPCMover mover) : base(mover)
    {

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
            
            if (ShouldCancelAvoidance(_currentHitObj)) return false;
            DecideAvoidancePattern(_currentHitObj);
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

    // ������L�����Z�����ׂ����𔻒f
    private bool ShouldCancelAvoidance(GameObject otherObj)
    {
        var isCancel = false;
        var otherNPCController = _currentHitObj.GetComponent<BraverController>();
        var otherDistance = otherNPCController.GetDistanceToTarget();
        var thisDistance = Vector3.Distance(_npc.transform.position, _targetPos);
        if (thisDistance < otherDistance)
        {
            _currentHitObj = null;
            isCancel = true;
        }
        
        return isCancel;
    }

    // ����p�^�[��������
    private void DecideAvoidancePattern(GameObject otherObj)
    {
        var otherNPCController = _currentHitObj.GetComponent<BraverController>();
        var otherDistance = otherNPCController.GetDistanceToTarget();
        var targetDistance = Vector3.Distance(_npc.transform.position, otherObj.transform.position);
        
        // 臒l�Ɋ�Â��ăp�^�[�������肷�郍�W�b�N�𒲐�
        _currentAvoid = (targetDistance - AVOID_THRESHOLD >= otherDistance) ? AvoidPatterns.Wait : AvoidPatterns.Move;
    }
}

