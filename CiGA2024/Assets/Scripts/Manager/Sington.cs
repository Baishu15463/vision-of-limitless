using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sington<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// ��̬�����ֶ�
    /// </summary>
    private static T instance;
    /// <summary>
    /// ��̬�������ԣ�ʹ��ʱֱ�ӻ�ȡ��ֵ����ʹ�õ���
    /// </summary>
    public static T Instance { get { return instance; } }
    /// <summary>
    /// ��ʼ���������Ե����ֶν��г�ʼ����ֵ
    /// </summary>
    protected virtual void Awake()
    {
        instance = this as T;
    }
    /// <summary>
    /// ���ٷ������������ֶ������ֵ
    /// </summary>
    protected virtual void OnDestroy()
    {
        instance = null;
    }
}
