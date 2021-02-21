using UnityEngine;

static class Helpers
{
    public static Vector3 ScreenToWorldPosition(Vector3 screenPos)
    {
        Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;
        
        if(Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return Vector3.zero;
    }

}
