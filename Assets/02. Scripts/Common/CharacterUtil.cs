using UnityEngine;

public static class CharacterUtil
{
    /// <summary>
    /// 특정 Position에서 layerMask까지의 거리를 반환하는 메서드(함수)
    /// </summary>
    /// <param name="position">시작 위치</param>
    /// <param name="layerMask">대상 오브젝트의 Layer Mask</param>
    /// <param name="maxDistance">최대 거리</param>
    /// <returns>시작 위치에서 부터 대상 오브젝트 사이의 거리</returns>
    public static float GetDistanceFromGround(Vector3 position, LayerMask layerMask, float maxDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(position, Vector3.down, out hit, 
            maxDistance, layerMask))
        {
            return hit.distance;
        }

        return maxDistance;
    }
}
