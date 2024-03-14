using System.Collections.Generic;
using UnityEngine;

// �ړI�����̑I��
public class RoomSelecter
{
    // �V���O���g��
    private static RoomSelecter instance;
    public static RoomSelecter Instance => instance ?? (instance = new RoomSelecter());

    private RoomBunker _roomBunker;
    public Vector3 ErrorVector => _roomBunker.ErrorVector;

    private RoomSelecter()
    {
        _roomBunker = GameObject.FindWithTag("RoomBunker").GetComponent<RoomBunker>();
    }

    public enum PointKind
    {
        IN_POINT,
        EXIT_POINT,
        OUT_POINT
    }

    // �w����W�̕����o��
    public Vector3 TargetPosSelection(int roomNum, PointKind pointKind, float npcPosY)
    {
        Vector3 outPos = Vector3.zero;
        // �s���ȃ��[���ԍ�
        if (roomNum >= _roomBunker.RoomDetails.Length || roomNum < 0) return ErrorVector;

        if (pointKind == PointKind.IN_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomInPoints.position;
        else if (pointKind == PointKind.EXIT_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomExitPoints.position;
        else if (pointKind == PointKind.OUT_POINT) outPos = _roomBunker.RoomDetails[roomNum].RoomOutPoints.position;
        outPos.y = npcPosY;

        return outPos;
    }

    /*-----�^�[�Q�b�g�̕�����I��-----*/
    public int SelectNextRoomNum(int NPCRoom, int currentRoomNum)
    {
        List<int> contenderRoom = CreateContenderRoomList(NPCRoom);
        contenderRoom = SearchStairs(contenderRoom);

        if (contenderRoom.Count == 0) return RoomBunker.ERROR_ROOM_NUM;

        List<int> alternativeRooms = SelectAlternativeRooms(contenderRoom, currentRoomNum);
        int nextRoomNum = SelectNextRoom(alternativeRooms);

        return nextRoomNum;
    }

    // �N���\�ȕ����̑I�������쐬
    private List<int> CreateContenderRoomList(int NPCRoom)
    {
        List<int> contenderRoom;
        int calcPos = NPCRoom % _roomBunker.FloorRoomCount;

        if (calcPos == 0)
            contenderRoom = new List<int>() { NPCRoom, NPCRoom + 1 };
        else
            contenderRoom = new List<int>() { NPCRoom, NPCRoom - 1, NPCRoom + 1 };

        List<int> outRooms = new List<int>(contenderRoom);
        foreach (int room in contenderRoom)
        {
            if (room == NPCRoom) continue;
            if (!_roomBunker.RoomDetails[room].IsRoomAcceptance) outRooms.Remove(room);
        }

        return outRooms;
    }

    // �K�i���l�������T��
    private List<int> SearchStairs(List<int> rooms)
    {
        List<int> updatedRooms = new List<int>(rooms);

        foreach (int roomNum in rooms)
        {
            // �K�i�łȂ��ꍇ�͎��̕�����
            if (!_roomBunker.RoomDetails[roomNum].IsStair) continue;
            bool isDel = true;
            int floor = roomNum / _roomBunker.FloorRoomCount;

            // ��K�̕�����ǉ�
            if (floor != _roomBunker.TopFloor)
            {
                int upperStairRoom = roomNum + _roomBunker.FloorRoomCount;
                if (_roomBunker.RoomDetails[upperStairRoom - 1].IsRoomAcceptance)
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
                if (_roomBunker.RoomDetails[lowerStairRoom].IsRoomAcceptance)
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

    /*------------------------*/
}
