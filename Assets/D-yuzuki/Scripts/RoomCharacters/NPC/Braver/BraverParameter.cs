using System;
using Unity.Mathematics;
using UnityEngine;

namespace D_yuzuki.Scripts.RoomCharacters.NPC.Braver
{
    public class BraverParameter : MonoBehaviour
    {
        public enum Parameter
        {
            Hp,
            Mp,
            Strength,
            Defense,
            MagicPower,
            MagicDefence,
            Speed
        }
        
        // _upValue��Parameter�ɑΉ�����7�v�f
        [Serializable]
        public struct RoomEffect
        {
            public RoomType _roomType;
            public float[] _upValue;
        }

        [Header("�����̃X�e�[�^�X�㏸�l")] [SerializeField] private RoomEffect[] _roomEffects;
        public RoomEffect[] RoomEffects => _roomEffects;
        // ���ネ�[�h�\��
        private int _braverCount = 1;
        public float[,] Parameters { get; private set; }
        
        private void Start()
        {
            Parameters = new float[_braverCount, Enum.GetValues(typeof(Parameter)).Length];
        }

        public void UpdateStatus(int braverNum, Parameter targetParam, float newValue)
        {
            Parameters[braverNum, (int)targetParam] = newValue;
        }
        
    }
}
