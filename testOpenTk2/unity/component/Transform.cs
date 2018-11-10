using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

/// <summary>
/// 坐标系与unity保持一致 为左手坐标系
/// </summary>

public class Transform : Component
{
    public Vector3 position = Vector3.zero;

    //Vector3 m_localScale;

    public Quaternion rotation = Quaternion.identity;

    public Vector3 eulerAngles = Vector3.zero;

    public Vector3 scale = Vector3.one;

    Transform m_parent = null;

    public Transform parent
    {
        get
        {
            return m_parent;
        }
    }

    public Matrix4x4 localToWorldMatrix
    {
        get 
        {
            return Matrix4x4.TRS(position, rotation, scale);
        }
    }

    // 旋转转朝向
//     public static Vector3 GetForward(Quaternion rotation)
//     {
//         return rotation * Vector3.forward;
//     }

    // 朝向转旋转
//     public static Quaternion LookAt(Vector3 dir)
//     {
//         return Quaternion.LookRotation(dir, Vector3.up);
//     }
}

