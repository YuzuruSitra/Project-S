using System;
using D_Sakurai.Scripts.CombatSystem.Units;
using UnityEngine;

namespace D_Sakurai.Scripts.PreCombat
{
    public sealed class DutyDispatcher : MonoBehaviour
    {
        //  DATA CONTAINER
        // -------------------------
        public static DutyDispatcher SingletonInstance { get; private set; }

        public UnitAlly[] RegisteredAllies { get; private set; }
        public int DutyId { get; private set; }

        private void Awake()
        {
            if (SingletonInstance && this != SingletonInstance)
            {
                Destroy(gameObject);
            }

            SingletonInstance = this;
            
            DontDestroyOnLoad(this);
        }
        
        
        //  CONFIGURE
        // --------------------
        
        // �v��Ȃ��C�����܂����A�o�����Ȃ��̂ŔO�̂���
        private static bool DispatcherExists()
        {
            if (SingletonInstance) return true;
            
            Debug.Log($"Duty dispatcher(Singleton MonoBehavior) doesn't exist!");
            return false;

        }

        /// <summary>
        /// �˗�ID��ݒ肷��B�˗�ID��ScriptableObject: Duties�̃C���f�b�N�X�B
        /// </summary>
        /// <param name="id">ID</param>
        public static void RegisterDutyId(int id)
        {
            if (DispatcherExists()) SingletonInstance.DutyId = id;
        }

        /// <summary>
        /// �o�����閡����ݒ肷��B
        /// </summary>
        /// <param name="allies">�������i�[�����z��B</param>
        public static void RegisterAllies(UnitAlly[] allies)
        {
            if (DispatcherExists()) SingletonInstance.RegisteredAllies = allies;
        }
    }
}