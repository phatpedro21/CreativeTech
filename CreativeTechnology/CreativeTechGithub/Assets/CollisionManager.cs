using UnityEngine;
using System.Collections;

public class CollisionManager : MonoBehaviour {

    public float turnSpeed;
    public float angleR, angleL;
    public bool turningRight, turningLeft;
 

    // Use this for initialization
    void Start () {
        angleL = 0f;
        angleR = 0f;
	
	}
	
	// Update is called once per frame
	void Update () {

		if (turningRight  && angleR < 90)
        {        
            angleR += turnSpeed;
            updateAngle((Quaternion.AngleAxis(angleR, new Vector3(0f, 1f, 0f))));
        }
        else
        {
            if (angleR > 0f)
            {
                angleR -= turnSpeed;
                updateAngle((Quaternion.AngleAxis(angleR, new Vector3(0f, 1f, 0f))));
            }
            else
            {
                angleR = 0f;
                updateAngle((Quaternion.AngleAxis(0, new Vector3(0f, 1f, 0f))));
            }            

        }
		if (turningLeft && angleL < 90)
        {
            angleL += turnSpeed;
            updateAngle((Quaternion.AngleAxis(- angleL, new Vector3(0f, 1f, 0f))));
        }
        else
        {
            if (angleL > 0f)
            {
                angleL -= turnSpeed;
                updateAngle((Quaternion.AngleAxis(-angleL, new Vector3(0f, 1f, 0f))));
            }
            else
            {
                angleL = 0f;
                updateAngle((Quaternion.AngleAxis(0, new Vector3(0f, 1f, 0f))));
            }
          }

    }

    public void updateAngle(Quaternion angle)
    {
        transform.parent.GetComponent<AgentShowcase>().setSwerve(angle);
    }
}
