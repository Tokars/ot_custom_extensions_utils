using UnityEngine;
using UnityEngine.UI;

namespace OT.Extensions
{
    public static class GraphicExtension
    {
        public static void RandomColor(this Graphic graphic, (float min, float max) range)
        {
            graphic.color = new Color(
                UnityEngine.Random.Range(range.min, range.max),
                UnityEngine.Random.Range(range.min, range.max),
                UnityEngine.Random.Range(range.min, range.max)
            );
        }

        /// <summary>
        /// Change Color brightness ONLY.
        /// </summary>
        /// <param name="graphic"></param>
        /// <param name="V">Brightness value [0..1].</param>
        public static void Brightness(this Graphic graphic, float v)
        {
            float H, S, V;
            Color.RGBToHSV(graphic.color, out H, out S, out V);
            graphic.color = Color.HSVToRGB(H, S, v);
        }

        public static void SetAlpha(this Graphic graphic, float alpha)
        {
            var color = graphic.color;
            graphic.color = new Color(color.r, color.g, color.b, alpha);
        }

        public static string ToRGBHex(Color c)
        {
            return $"#{ToByte(c.r):X2}{ToByte(c.g):X2}{ToByte(c.b):X2}";

            byte ToByte(float f)
            {
                f = Mathf.Clamp01(f);
                return (byte) (f * 255);
            }
        }
    }
}