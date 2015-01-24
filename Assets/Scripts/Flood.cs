using UnityEngine;
using System.Collections;

public class Flood : MonoBehaviour {

    public float speed = 1.0f;
    float height = 0.0f;
    
    enum State
    {
        FLOODING,
        IDLE
    }

    State m_state;

	void Start () 
    {
        Vector3 curScale = transform.localScale;
        curScale.y = height;
        transform.localScale = curScale;

        m_state = State.IDLE;
	}

    public bool IsCleared()
    {
        return m_state == State.IDLE;
    }

    public void Pump()
    {
        if (m_state == State.IDLE)
            return;

        height -= 2.0f * Time.deltaTime;
        height = Mathf.Clamp01(height);

        if (height < 0.0001f)
        {
            height = 0.0f;
            Vector3 curScale = transform.localScale;
            curScale.y = height;
            transform.localScale = curScale;
            m_state = State.IDLE;

            RemoteCallNotifyRoomCleared();
        }
    }

    public void RemoteCallNotifyRoomCleared()
    {
        networkView.RPC("NotifyRoomIsClear", RPCMode.OthersBuffered);
    }

    [RPC]
    public void NotifyRoomIsClear()
    {
        ClearFlood();
    }

    public void StartFlood()
    {
        m_state = State.FLOODING;
    }

    public void ClearFlood()
    {
        height = 0.0f;
        Vector3 curScale = transform.localScale;
        curScale.y = height;
        transform.localScale = curScale;
        m_state = State.IDLE;
    }

	void Update () 
    {
        if (m_state == State.FLOODING)
        {
            Flooding();
        }
	}

    void Flooding()
    {
       if (!Network.isServer)
            return;

        height += Time.deltaTime * speed * 0.05f;
        height = Mathf.Clamp01(height);

        Vector3 curScale = transform.localScale;
        Vector3 curPos = transform.localPosition;

        curScale.y = height;
        curPos.y = (height) * 0.5f - 0.5f;
        transform.localScale = curScale;
        transform.localPosition = curPos;
    }
}
