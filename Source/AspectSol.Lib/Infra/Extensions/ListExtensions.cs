namespace AspectSol.Lib.Infra.Extensions;

public static class ListExtensions
{
    public static List<T> SafeConcat<T>(this List<T> listA, List<T> listB)
    {
        if (listA == null || listA.Count == 0) return listB;
        if (listB == null || listB.Count == 0) return listA;

        var list = listA.ToList();

        foreach (var item in listB.Where(item => !list.Contains(item)))
        {
            list.Add(item);
        }

        return list;
    }

    public static List<T> SafetIntersect<T>(this List<T> listA, List<T> listB)
    {
        if (listA == null || listA.Count == 0) return listB;
        if (listB == null || listB.Count == 0) return listA;

        var list = listA.Intersect(listB).ToList();
        return list;
    }

    public static List<T> SafeUnion<T>(this List<T> listA, List<T> listB)
    {
        if (listA == null || listA.Count == 0) return listB;
        if (listB == null || listB.Count == 0) return listA;

        var list = listA.Union(listB).ToList();
        return list;
    }
}