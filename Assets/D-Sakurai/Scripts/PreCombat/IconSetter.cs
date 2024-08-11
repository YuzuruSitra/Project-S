using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

namespace D_Sakurai.Scripts.PreCombat
{
    public class IconSetter : MonoBehaviour
    {
        [SerializeField] private Transform SetterParent;
        [SerializeField] private Transform ButtonParent;

        [SerializeField] private GameObject ButtonPrefab;

        [SerializeField] private InfoPanel InfoPanel;

        [SerializeField] private Camera MainCam;

        private (Transform, RectTransform)[] _btnData;

        public void SetIcons(DutyLoader loaderInstance)
        {
            var nSetters = SetterParent.childCount;
            _btnData = new (Transform, RectTransform)[nSetters];

            for (int i = 0; i < nSetters; i++)
            {
                // var btns = _btnData[i];

                var setter = SetterParent.GetChild(i);

                // btn.Item1: icon position in 3d space
                // btn.Item2: icon position in screen space

                var holder = setter.gameObject.GetComponent<IconDataHolder>();

                // UI�̃{�^����\������
                var uiBtn = Instantiate(ButtonPrefab, ButtonParent);

                uiBtn.transform.position = MainCam.WorldToScreenPoint(setter.position);

                // UI�̃{�^���̃C�x���g��ݒ肷��
                var btnScript = uiBtn.GetComponent<DutyButton>();
                btnScript.SetEvent(loaderInstance, InfoPanel, holder.GetDutyIdx());

                // �v���r���[�p�̃L���[�u������
                setter.GetChild(0).gameObject.SetActive(false);

                // UI�{�^����Transform��ێ�
                _btnData[i] = (setter, uiBtn.GetComponent<RectTransform>());
            }
        }

        public void RepositionIcons()
        {
            foreach (var data in _btnData)
            {
                if (!data.Item2) continue;

                data.Item2.position = MainCam.WorldToScreenPoint(data.Item1.position);
            }
        }
    }
}