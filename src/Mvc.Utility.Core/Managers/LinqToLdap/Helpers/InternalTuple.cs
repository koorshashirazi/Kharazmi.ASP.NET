namespace Mvc.Utility.Core.Managers.LinqToLdap.Helpers
{
    internal class ThreeTuple<T1, T2, T3>
    {
        public ThreeTuple(T1 t1, T2 t2, T3 t3)
        {
            Item1 = t1;
            Item2 = t2;
            Item3 = t3;
        }

        public T1 Item1 { get; }
        public T2 Item2 { get; }
        public T3 Item3 { get; }
    }

    internal class TwoTuple<T1, T2>
    {
        public TwoTuple(T1 t1, T2 t2)
        {
            Item1 = t1;
            Item2 = t2;
        }

        public T1 Item1 { get; }
        public T2 Item2 { get; }
    }
}