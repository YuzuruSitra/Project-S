using System;
using Unity.Mathematics;
using UnityEngine;

namespace D_Sakurai.Scripts.Utility
{
    public class DragCameraControl : MonoBehaviour
    {
        [SerializeField] private Transform MainCam;

        [SerializeField] private PreCombat.IconSetter IconSetter;
        
        [SerializeField] private float DragThresh;
        [SerializeField] private float MaxSpeed;
        [SerializeField] private float DragScale;
        [SerializeField] private float FrictionRate;

        [SerializeField] private int2 CamRangeX;
        [SerializeField] private int2 CamRangeZ;

        private Vector3 _prevMousePos;

        private Vector2 _velocity;

        private void Start()
        {
            _prevMousePos = Input.mousePosition;
        }

        private void Update()
        {
            // ����
            if (_velocity.magnitude > .001)
            {
                _velocity *= FrictionRate;
            }
            else
            {
                _velocity = Vector2.zero;
            }

            
            var diff = Vector2.zero;
            
            var mousePos = Input.mousePosition;
            
            // �h���b�O�������
            if (Input.GetMouseButton(0))
            {
                var diff3 = mousePos - _prevMousePos;
                diff = new Vector2(diff3.x, diff3.y);
            }
            
            _prevMousePos = mousePos;

            // �h���b�O�ʂ�臒l�ȏ�ł����
            if (diff.magnitude > DragThresh)
            {
                _velocity += diff * (DragScale * -1);

                // ��������ꍇ
                if (_velocity.magnitude > MaxSpeed)
                {
                    _velocity = _velocity.normalized * MaxSpeed;
                }
            }
            
            // ���ɒx���ꍇ�͍Ĕz�u�����ɗ��E
            if (_velocity.magnitude < .001) return;
            
            // �K�p
            var cp = new Vector2(MainCam.position.x, MainCam.position.z);
            cp += _velocity;

            // �͈͊O�Ȃ�~�߂�
            var clampedX = Mathf.Clamp(cp.x, CamRangeX.x, CamRangeX.y);
            var clampedY = Mathf.Clamp(cp.y, CamRangeZ.x, CamRangeZ.y);

            MainCam.position = new Vector3(clampedX, MainCam.position.y, clampedY);
            
            IconSetter.RepositionIcons();
        }
    }
}