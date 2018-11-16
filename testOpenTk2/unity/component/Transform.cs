using System;
using System.Collections.Generic;

using System.Text;
using UnityEngine;

/// <summary>
/// 坐标系与unity保持一致 为左手坐标系
/// </summary>

public class Transform : Component
{
    bool m_needParentUpdate = true;

    Vector3 m_localPostion;
    Quaternion m_localRotation = Quaternion.identity;
    Vector3 m_localScale = Vector3.one;

    Quaternion m_DerivedOrientation;
    Vector3 m_DerivedScale;
    Vector3 m_DerivedPosition;

    Transform m_parent = null;
    List<Transform> m_childList = new List<Transform>();

    public static void Test()
    {
        {
            GameObject t1 = new GameObject();
            GameObject t2 = new GameObject();
            t2.transform.parent = t1.transform;
            t2.transform.position = new Vector3(1, 0, 0);
            Debug.Log("LocalToWorld " + t2.transform.localToWorldMatrix);
            Debug.Log("worldToLocal " + t2.transform.worldToLocalMatrix);

            //Quaternion delta = Quaternion.FromToRotation(t2.transform.forward, new Vector3(1, 1, 1));
            t2.transform.forward = new Vector3(1, 1, 1);
            //t2.transform.rotation = t2.transform.rotation * delta;
            Debug.Log("forward rotation " + t2.transform.rotation);
        }


        {
            Transform t1 = new Transform();
            Debug.Log(t1.position.ToString());
            Debug.Log(t1.rotation.ToString());
            Debug.Log(t1.lossyScale.ToString());

            Transform t2 = new Transform();
            t2.parent = t1;

            t1.position = new Vector3(1, 0, 0);

            Debug.Log(t2.position);
            Debug.Log(t2.rotation);
            Debug.Log(t2.lossyScale);

            Debug.Log("LocalToWorldL " + t2.localToWorldMatrix);
            Debug.Log("worldToLocalL " + t2.worldToLocalMatrix);

            t2.forward = new Vector3(1, 1, 1);
            Debug.Log("forward rotationL " + t2.rotation);
        }

    }

    public Vector3 position
    {
        get
        {
            return convertLocalToWorldPosition(m_localPostion);
        }

        set
        {
            m_localPostion = convertWorldToLocalPosition(value);
            SetChildNeedUpdate();
        }
    }

    public Vector3 localPosition
    {
        get
        {
            return m_localPostion;
        }
        set
        {
            m_localPostion = value;
            SetChildNeedUpdate();
        }
    }

    Vector3 convertWorldToLocalPosition(Vector3 worldPos)
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return Quaternion.Inverse(m_DerivedOrientation) * Vector3.Divide(worldPos - m_DerivedPosition, m_DerivedScale);
    }

    Vector3 convertLocalToWorldPosition(Vector3 localPos)
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        //Matrix4x4L matrix = Matrix4x4L.TRS(_getDerivedPosition(), _getDerivedOrientation(), _getDerivedScale());
        Matrix4x4 matrix = GetFullTransform();
        return matrix.MultiplyPoint(localPos);
    }

    void SetChildNeedUpdate()
    {
        //if(m_parent == null)
        {
            m_needParentUpdate = true;
        }

        foreach (var iter in m_childList)
        {
            iter.SetNeedUpdateParent();
        }
    }

    void SetNeedUpdateParent()
    {
        m_needParentUpdate = true;
        SetChildNeedUpdate();
    }

    void _updateFromParent()
    {
        if (m_parent != null)
        {
            Quaternion parentOrientation = m_parent._getDerivedOrientation();
            m_DerivedOrientation = parentOrientation * m_localRotation;
            Vector3 parentScale = m_parent._getDerivedScale();
            m_DerivedScale = Vector3.Scale(parentScale, m_localScale);
            // Change position vector based on parent's orientation & scale
            m_DerivedPosition = parentOrientation * Vector3.Scale(parentScale, m_localPostion);
            // Add altered position vector to parents
            m_DerivedPosition += m_parent._getDerivedPosition();
        }
        else
        {
            m_DerivedOrientation = m_localRotation;
            m_DerivedPosition = m_localPostion;
            m_DerivedScale = m_localScale;
        }

        m_needParentUpdate = false;
    }

    Quaternion _getDerivedOrientation()
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return m_DerivedOrientation;
    }

    Vector3 _getDerivedScale()
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return m_DerivedScale;
    }

    Vector3 _getDerivedPosition()
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return m_DerivedPosition;
    }

    public Quaternion rotation
    {
        get
        {
            return convertLocalToWorldOrientation(m_localRotation);
        }

        set
        {
            m_localRotation = convertWorldToLocalOrientation(value);
            SetChildNeedUpdate();
        }
    }

    public Vector3 localEulerAngles
    {
        get
        {
            return m_localRotation.eulerAngles;
        }
        set
        {
            m_localRotation = Quaternion.Euler(value);
            SetChildNeedUpdate();
        }
    }

    public Vector3 eulerAngles
    {
        get
        {
            return rotation.eulerAngles;
        }
        set
        {
            rotation = Quaternion.Euler(value);
        }
    }

    Quaternion convertLocalToWorldOrientation(Quaternion localOrientation)
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return m_DerivedOrientation * localOrientation;
    }

    Quaternion convertWorldToLocalOrientation(Quaternion worldOrientation)
    {
        if (m_needParentUpdate)
        {
            _updateFromParent();
        }
        return Quaternion.Inverse(m_DerivedOrientation) * worldOrientation;
    }

    public Transform parent
    {
        get
        {
            return m_parent;
        }

        set
        {
            if (m_parent != null)
            {
                m_parent.m_childList.Remove(this);
            }
            m_parent = value;
            if (m_parent != null)
            {
                m_parent.m_childList.Add(this);
            }
            SetChildNeedUpdate();
        }
    }

    public Quaternion localRotation
    {
        get
        {
            return m_localRotation;
        }
        set
        {
            m_localRotation = value;
            SetChildNeedUpdate();
        }
    }

    public Vector3 localScale
    {
        get
        {
            return m_localScale;
        }
        set
        {
            m_localScale = value;
            SetChildNeedUpdate();
        }
    }

    public Vector3 lossyScale
    {
        get
        {
            return Vector3.Scale(m_DerivedScale, m_localScale);
        }
        set
        {
            m_localScale = Vector3.Divide(lossyScale, m_DerivedScale);
            SetChildNeedUpdate();
        }
    }

    Matrix4x4 GetFullTransform()
    {
        Matrix4x4 matrix = Matrix4x4.TRS(_getDerivedPosition(), _getDerivedOrientation(), _getDerivedScale());
        return matrix;
    }

    public Matrix4x4 localToWorldMatrix
    {
        get
        {

            return GetFullTransform();
        }
    }

    public Matrix4x4 worldToLocalMatrix
    {
        get
        {
            //Matrix4x4L matrix = Matrix4x4L.TRS(_getDerivedPosition(), _getDerivedOrientation(), _getDerivedScale());
            return GetFullTransform().inverse;
        }
    }

    public Vector3 forward
    {
        get
        {
            return rotation * Vector3.forward;
        }

        set
        {
            //QuaternionL forwardQua = QuaternionL.identity;
            //QuaternionL newForwardQua = RotateHelper.DirectionToRotation(value);
            //QuaternionL delta = QuaternionL.FromToRotation(forward, value);
            //rotation = rotation * delta;
            //SetChildNeedUpdate();
            rotation = Quaternion.LookRotation(value);
        }
    }

    public Vector3 right
    {
        get
        {
            return rotation * Vector3.right;
        }
        set
        {
            rotation = Quaternion.FromToRotation(Vector3.right, value);
        }
    }

    public Vector3 up
    {
        get
        {
            return rotation * Vector3.up;
        }
        set
        {
            rotation = Quaternion.FromToRotation(Vector3.up, value);
        }
    }
}

