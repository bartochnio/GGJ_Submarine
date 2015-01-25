using UnityEngine;
using System.Collections;

public class AlarmLight : MonoBehaviour {

    public Room room;
    public Color alarmColor;
    public Color idleColor;
    public float blinkSpeed;
    public float idleIntensity;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (room == null) return;

        switch (room.Status)
        {
            //case Room.RoomEmergency.BROKEN:
            //    light.color = alarmColor;
            //    light.intensity *= Mathf.Sin(Time.deltaTime * blinkSpeed) * 0.5f + 0.5f;
            //    break;
            case Room.RoomEmergency.NONE:
                light.color = idleColor;
                light.intensity = idleIntensity;
                break;
            default:
                light.color = alarmColor;
                light.intensity = (Mathf.Sin(Time.time * blinkSpeed)*0.5f+0.5f) * 4.0f;
                break;
        }
	}
}
