using System;
using D_Sakurai.Scripts.CombatSystem.Units;
using D_Sakurai.Scripts.PreCombat;
using Resources.Duty;
using UnityEngine;

namespace D_Sakurai.Scripts.CombatSystem
{
    public class CombatSequencer : MonoBehaviour
    {
        private CombatManager _manager;
        
        [SerializeField] private bool DontUseSingleton;

        [SerializeField] private int DutyId;
        [SerializeField] private Tester.TestBraver[] Allies;

        private int _id;
        private UnitAlly[] _allies;
        
        public void Start()
        {
            if (DontUseSingleton)
            {
                // [DEBUG] �C���X�y�N�^�Őݒ肵���l�ň˗����J�n����ۂ̏���
                _id = DutyId;
                _allies = Tester.GetInstancedBravers(Allies);
            }
            else
            {
                // Singleton���瓾�����ň˗����J�n����ۂ̏���
                var inst = DutyDispatcher.SingletonInstance;
                
                if (!inst)
                {
                    Debug.LogError(
                        "DutyDispatcher doesn't exist!" +
                        "Enable 'DontUseSingleton' if you want to use parameters set in inspector."
                        );
                }
                
                _id = DutyDispatcher.SingletonInstance.DutyId;
                _allies = DutyDispatcher.SingletonInstance.RegisteredAllies;
            }
         
            // �}�l�[�W���[���擾
            _manager = GetComponent<CombatManager>();
            
            if (_manager == null)
            {
                Debug.LogError("Reference to Combat Manager is null! Make sure you attached combatManager.cs to this GameObject.\nTrying to get instance...");
            }
            
            // �˗����Z�b�g�A�b�v
            _manager.Setup(_id, _allies);
            
            // �˗��J�n
            _manager.Commence();
        }
    }
}