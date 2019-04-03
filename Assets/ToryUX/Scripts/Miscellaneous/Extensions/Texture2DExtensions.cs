using UnityEngine;
using System.Collections;

public static class Texture2DExtensions
{
    public static void DrawLine(this Texture2D tex, Vector2 p1, Vector2 p2, Color col)
    {
        Vector2 t = p1;
        float frac = 1f / Mathf.Sqrt (Mathf.Pow (p2.x - p1.x, 2f) + Mathf.Pow (p2.y - p1.y, 2f));
        float ctr = 0;

        while ((int)t.x != (int)p2.x || (int)t.y != (int)p2.y) {
            t = Vector2.Lerp(p1, p2, ctr);
            ctr += frac;
            tex.SetPixel((int)t.x, (int)t.y, col);
        }
    }

    public enum RotationDirection { CW, CCW };
    public static void Rotate90(this Texture2D self, RotationDirection direction)
    {
        Color[] original = self.GetPixels();
        self.Resize(self.height, self.width);
        for (var y = 0; y < self.height; y++)
        {
            for (var x = 0; x < self.width; x++)
            {
                if (direction == RotationDirection.CW)
                {
                    self.SetPixel(x, y, original[x * self.height + self.height - y - 1]);
                }
                else
                {
                    self.SetPixel(x, y, original[self.height * (self.width - x - 1) + y]);
                }
            }
        }
    }

    public enum FlipDirection { Horizontal, Vertical };
    public static void Flip(this Texture2D self, FlipDirection direction)
    {
        Color[] original = self.GetPixels();
        for (var y = 0; y < self.height; y++)
        {
            for (var x = 0; x < self.width; x++) {
                if (direction == FlipDirection.Horizontal)
                {
                    self.SetPixel(x, y, original[y * self.width + self.width - x - 1]);
                }
                else
                {
                    self.SetPixel(x, y, original[(self.height - y - 1) * self.width + x]);
                }
            }
        }
    }
}