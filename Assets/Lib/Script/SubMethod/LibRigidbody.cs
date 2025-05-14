using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LibRigidbody
{

    static public void SetPosition(this Rigidbody rigidbody, Vector3 goalPos)
    {
        rigidbody.MovePosition(goalPos - rigidbody.position);
    }

    static public bool MoveFourceTime(this Rigidbody rigidbody, Vector3 goalPos, ref float time)
    {
        if (time <= Time.deltaTime)
        {
            time = 0;
            rigidbody.SetPosition(goalPos);
            return true;
        }

        float rate = Time.deltaTime / time;
        rigidbody.SetPosition(Vector3.Lerp(rigidbody.position, goalPos, rate));
        time -= Time.deltaTime;
        return false;
    }

    static public bool MoveFocusSpeed(this Rigidbody rigidbody, Vector3 goalPos, float speed)
    {
        bool isGoal = (rigidbody.position - goalPos).magnitude < speed;

        rigidbody.position = Vector3.MoveTowards(rigidbody.position, goalPos, speed);

        return isGoal;
    }
}
