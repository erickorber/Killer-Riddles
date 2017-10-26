using UnityEngine;

public class Movement : MonoBehaviour {

    /*
     * Checks to see if the distance between GameObjects a and b are less than distance.
     * Returns the result as a bool.
     */
    public static bool isCloserThan(GameObject a, GameObject b, float distance)
    {
        if (Vector3.Distance(a.transform.position, b.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /*
     * Checks to see if the distance between GameObjects a and b are greater than distance.
     * Returns the result as a bool.
     */
    public static bool isFartherThan(GameObject a, GameObject b, float distance)
    {
        if (Vector3.Distance(a.transform.position, b.transform.position) > distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool V3Equal(Vector3 a, Vector3 b)
    {
        return Vector3.SqrMagnitude(a - b) < 0.0001;
    }
}
