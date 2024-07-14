using System;
using System.Collections.Generic;
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
            Speed,
        }
        
        // _upValue��Parameter�ɑΉ�����7�v�f
        [Serializable]
        public struct RoomEffect
        {
            public RoomType _roomType;
            public float[] _upValue;
            public float _friendPoint;
        }

        [Header("�����̃X�e�[�^�X�㏸�l")] [SerializeField] private RoomEffect[] _roomEffects;
        public RoomEffect[] RoomEffects => _roomEffects;
        // ���ネ�[�h�\��
        private int _braverCount = 2;
        public float[,] Parameters { get; private set; }
        public List<List<float>> Friendship { get; private set; }
        private void Start()
        {
            Parameters = new float[_braverCount, Enum.GetValues(typeof(Parameter)).Length];
            InitializeFriendship(_braverCount);
        }

        private void InitializeFriendship(int braverCount)
        {
            Friendship = new List<List<float>>(braverCount);
            for (var i = 0; i < braverCount; i++)
            {
                Friendship.Add(new List<float>(braverCount));
                for (var j = 0; j < braverCount; j++)
                {
                    Friendship[i].Add(0f); // �����l�Ƃ���0��ݒ�
                }
            }
        }

        public void UpdateStatus(int braverNum, Parameter targetParam, float newValue)
        {
            Parameters[braverNum, (int)targetParam] = newValue;
        }
        
        public void UpdateFriendship(int braverNum, int targetNum, float newValue)
        {
            if (braverNum >= Friendship.Count || targetNum >= Friendship[braverNum].Count) return;
            Friendship[braverNum][targetNum] = newValue;
        }
        
        // �u���[�o�[�̐l�����ς�����Ƃ��p�̏�����ǋL�\��
        
    }
}
