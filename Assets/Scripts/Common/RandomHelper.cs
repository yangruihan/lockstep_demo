using System;

public class RandomHelper : Singleton<RandomHelper>
{
    Random random = new Random(1);      // 给定默认的随机数种子

    public int GetRandomInt()
    {
        return random.Next();
    }

    public int GetRandomInt(Int32 maxValue)
    {
        return random.Next(maxValue);
    }

    public double GetRandomDouble()
    {
        return random.NextDouble();
    }
}
