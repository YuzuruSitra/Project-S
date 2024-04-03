using System;
using UnityEngine;

// ����̎��Ԃ�ێ�����N���X
public class DayTimeKeeper : MonoBehaviour
{
    [Header("���ۂ�1���̎��� (��)")]
    [SerializeField]
    private float _dayMinute;
    private int MAX_DAY_TIME = 24;
    // ���Ԃ̌o�ߑ��x/��
    private float _elapsedTimeSpeed;
    private TimeSpan _currentTime = TimeSpan.Zero;
    // ���݂̎���
    public TimeSpan CurrentTime => _currentTime;
    private float _currentHourRatio;
    // ���݂̊���
    public float CurrentHourRatio => _currentHourRatio;

    // Start is called before the first frame update
    void Start()
    {
        _elapsedTimeSpeed = MAX_DAY_TIME / _dayMinute * 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = _currentTime.Add(TimeSpan.FromSeconds(_elapsedTimeSpeed * Time.deltaTime));
        // �o�ߎ��Ԃ̊����v�Z
        float currentHourInDay = _currentTime.Days * MAX_DAY_TIME + (float)_currentTime.TotalHours % MAX_DAY_TIME;
        _currentHourRatio = currentHourInDay / MAX_DAY_TIME;
    }
}
