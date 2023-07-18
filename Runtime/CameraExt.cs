using UnityEngine;

namespace OT.Extensions
{
    public static class CameraExt
    {
        public static void WorldRectToScreenSpace( this Camera cam, Vector3 worldPos, RectTransform area, out Vector2 res)
        {
            Vector3 screenPoint = cam.WorldToScreenPoint(worldPos);
            screenPoint.z = 0;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(area, screenPoint, cam, out res);
        }
    }
}