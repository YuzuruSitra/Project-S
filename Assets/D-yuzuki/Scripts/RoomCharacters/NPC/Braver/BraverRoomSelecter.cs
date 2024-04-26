using System.Collections.Generic;
using UnityEngine;

// �ړI�����̑I��
public class BraverRoomSelecter
{
    // �V���O���g��
    private static BraverRoomSelecter instance;
    public static BraverRoomSelecter Instance => instance ?? (instance = new BraverRoomSelecter());

    private RoomBunker _roomBunker;

    private BraverRoomSelecter()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public int SelectNextRoomNum(int npcRoom, int currentRoomNum)
    {
        List<int> contenderRoom = CreateContenderRoomList(npcRoom);
        contenderRoom = SearchStairs(contenderRoom, npcRoom);
        contenderRoom = SearchLift(contenderRoom, npcRoom);
        if (contenderRoom.Count == 0) return RoomBunker.ERROR_ROOM_NUM;
        List<int> alternativeRooms = SelectAlternativeRooms(contenderRoom, currentRoomNum);
        if (alternativeRooms.Count == 0) return RoomBunker.ERROR_ROOM_NUM;
        int nextRoomNum = SelectNextRoom(alternativeRooms);
        return nextRoomNum;
    }

    // �N���\�ȕ����̑I�������쐬
    private List<int> CreateContenderRoomList(int npcRoom)
    {
        List<int> contenderRoom;
        int calcPos = npcRoom % _roomBunker.FloorRoomCount;

        if (calcPos == 0)
            contenderRoom = new List<int>() { npcRoom, npcRoom + 1 };
        else
            contenderRoom = new List<int>() { npcRoom, npcRoom - 1, npcRoom + 1 };

        List<int> outRooms = new List<int>(contenderRoom);
        foreach (int room in contenderRoom)
            if (!_roomBunker.RoomDetails[room].IsRoomAcceptance(npcRoom)) 
                outRooms.Remove(room);
        return outRooms;
    }

    // �K�i���l�������T��
    private List<int> SearchStairs(List<int> rooms, int npcRoom)
    {
        List<int> updatedRooms = new List<int>(rooms);
        foreach (int roomNum in rooms)
        {
            RoomType roomType = _roomBunker.RoomDetails[roomNum].RoomType;
            if (roomType != RoomType.Stair) continue;
            bool isDel = true;
            int floor = roomNum / _roomBunker.FloorRoomCount;

            // ��K�̕�����ǉ�
            if (floor != _roomBunker.TopFloor)
            {
                int upperStairRoom = roomNum + _roomBunker.FloorRoomCount;
                if (_roomBunker.RoomDetails[upperStairRoom - 1].IsRoomAcceptance(npcRoom))
                {
                    updatedRooms.Add(upperStairRoom);
                    updatedRooms.Add(upperStairRoom - 1);
                    isDel = false;
                }
            }
            if (floor != 0)
            {
                // ���K�̕�����ǉ�
                int lowerStairRoom = roomNum - _roomBunker.FloorRoomCount;
                if (_roomBunker.RoomDetails[lowerStairRoom].IsRoomAcceptance(npcRoom))
                {
                    updatedRooms.Add(lowerStairRoom);
                    updatedRooms.Add(lowerStairRoom - 1);
                    isDel = false;
                }
            }
            
            // �㉺�K�̕��������ɕs�Ȃ�Ό��݂̕������폜
            if (isDel) updatedRooms.Remove(roomNum);
        }
        return updatedRooms;
    }

    // ���t�g���l�������T��
    private List<int> SearchLift(List<int> rooms, int npcRoom)
    {
        List<int> updatedRooms = new List<int>(rooms);
        foreach (int roomNum in rooms)
        {
            RoomType roomType = _roomBunker.RoomDetails[roomNum].RoomType;
            bool isAdd = false;
            if (roomType != RoomType.Lift) continue;
            GameObject room = _roomBunker.RoomDetails[roomNum].gameObject;
            Lift lift = room.GetComponent<Lift>();
            // �K�w������
            int targetLiftNum;
            if (lift.Info == Lift.LiftInfo.UPPER)
                targetLiftNum = roomNum - _roomBunker.FloorRoomCount;
            else
                targetLiftNum = roomNum + _roomBunker.FloorRoomCount;
            // �[���ǂ����̔���
            int outerNum = targetLiftNum % _roomBunker.FloorRoomCount; 
            // ���t�g�̍��E�����ɒǉ�
            if (outerNum != 0 && _roomBunker.RoomDetails[targetLiftNum - 1].IsRoomAcceptance(npcRoom))
            {
                isAdd = true;
                updatedRooms.Add(targetLiftNum - 1);
            }
            if (outerNum != _roomBunker.FloorRoomCount - 1 && _roomBunker.RoomDetails[targetLiftNum + 1].IsRoomAcceptance(npcRoom))
            {
                isAdd = true;
                updatedRooms.Add(targetLiftNum + 1);
            }
            if (isAdd) updatedRooms.Add(targetLiftNum);
            else updatedRooms.Remove(roomNum);
        }
        return updatedRooms;
    }

    // �ŏI�I�ȑI������I��
    private List<int> SelectAlternativeRooms(List<int> rooms, int currentRoomNum)
    {
        List<int> alternativeRooms = new List<int>();
        int currentFloor = currentRoomNum / _roomBunker.FloorRoomCount;

        foreach (int roomNum in rooms)
        {
            if (roomNum == currentRoomNum) continue;
            int calcFloor = roomNum / _roomBunker.FloorRoomCount;
            if (currentFloor == calcFloor) alternativeRooms.Add(roomNum);
        }
        
        return alternativeRooms;
    }

    // �����_���ŕ�����I��
    private int SelectNextRoom(List<int> rooms)
    {
        int randomIndex = Random.Range(0, rooms.Count);
        int nextRoomNum = rooms[randomIndex];

        return nextRoomNum;
    }

}
