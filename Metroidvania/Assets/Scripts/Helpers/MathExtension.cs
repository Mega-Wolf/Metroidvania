using UnityEngine;

public static class MathExtension {

    public static (float x1, float x2) Midnight(float a, float b, float c) {
        if (a == 0) {
            return (float.NaN, float.NaN);
        }
        float underSqrt = b * b - 4 * a * c;
        if (underSqrt < 0) {
            return (float.NaN, float.NaN);
        }

        float sqrt = Mathf.Sqrt(underSqrt);

        return ((-b + sqrt) / 2, (-b - sqrt) / 2);
    }

    public static Vector2 Inters(Vector2 start1, Vector2 end1, Vector2 start2, Vector2 end2) {

        // This means they are parallel
        // I return the midpoint then
        if (Almost(Mathf.Abs((end1.x - start1.x) / (end2.x - start2.x)), 1) && Almost(Mathf.Abs((end1.y - start1.y) / (end2.y - start2.y)), 1)) {
            return (start1 + start2) / 2;
        }

        // I am no fan of this method since I don't get t
        // t would allow me to check if the result is actually between the points or not

        float divider = (end1.x - start1.x) * (end2.y - start2.y) - (end2.x - start2.x) * (end1.y - start1.y);

        Vector2 retVector = new Vector2(
            ((end1.x * start1.y - start1.x * end1.y) * (end2.x - start2.x) - (end2.x * start2.y - start2.x * end2.y) * (end1.x - start1.x)) / divider,
            ((end1.x * start1.y - start1.x * end1.y) * (end2.y - start2.y) - (end2.x * start2.y - start2.x * end2.y) * (end1.y - start1.y)) / divider
        );

        return retVector;
    }

    private static bool Almost(float a, float b) {
        return Mathf.Abs(a - b) < 0.05f;
    }

    // public static Vector2 Intersection((Vector2 start, Vector2 direction) line1, (Vector2 start, Vector2 direction) line2) {

    //     Vector2 a = line1.start;
    //     Vector2 b = line1.direction;

    //     Vector2 c = line2.start;
    //     Vector2 d = line2.direction;

    //     // This means they are parallel
    //     // I return the midpoint then
    //     if (Almost(Mathf.Abs(line1.direction.x / line2.direction.x), 1) && Almost(Mathf.Abs(line1.direction.y / line2.direction.y), 1)) {
    //         return (line1.start + line2.start) / 2;
    //     }

    //     if (line1.direction.x == 0) { // this is a special case, because I actually want to divide through this
    //         //therefore; I have to calculate s here instead of t
    //         float s = (line1.start.x - line2.start.y) / line1.direction.y;
    //         return line2.start + s * line2.direction;
    //     } else if (line2.direction.y == 0) { // I would love to divide through that as well
    //         float t = (line2.start.y - line1.start.y) / line2.direction.x;
    //         return line1.start + t * line1.direction;
    //     } else {
    //         float factor = line2.direction.x / line2.direction.y;
    //         float test = ((line2.start.x - line1.start.x) - factor * (line2.start.y - line1.start.y));
    //         float test2 = (line1.direction.x - factor * line2.direction.x);
    //         Debug.Log(test2);
    //         float t = ((line2.start.x - line1.start.x) - factor * (line2.start.y - line1.start.y)) / (line1.direction.x - factor * line2.direction.x);
    //         return line1.start + t * line1.direction;
    //     }
    // }

}