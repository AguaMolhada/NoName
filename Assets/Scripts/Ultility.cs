using System.Text;
using System.Collections;

public static class Ultility {

    public static T[] ShuffleArray<T>(T[] array, string seed)
    {
        System.Random rnd = new System.Random(seed.GetHashCode());

        for (int i = 0; i < array.Length-1; i++)
        {
            int randomIndex = rnd.Next(i, array.Length);
            T tempItem = array[randomIndex];
            array[randomIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }

    public static string GetRandomString(System.Random rnd, int length)
    {
        int x = length;
        string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890!@#$%&*()[]{}<>,.;:/?";
        StringBuilder rs = new StringBuilder();

        while (x != 0)
        {
            rs.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);
            x--;
        }
        return rs.ToString();
    }

    public static float GetPercent(float total, float percent)
    {
        return (percent / 100) * total;
    }
}

[System.Serializable]
public struct Coord
{
    public int x;
    public int y;

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public static bool operator == (Coord c1, Coord c2)
    {
        return c1.x == c2.x && c1.y == c2.y;
    }

    public static bool operator != (Coord c1, Coord c2)
    {
        return !(c1 == c2);
    }

}