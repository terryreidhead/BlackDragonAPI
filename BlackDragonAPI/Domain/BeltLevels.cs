namespace BlackDragonAPI.Domain
{
    public static class BeltLevels
    {
        public static readonly string[] Allowed =
        {
            "White", "Yellow", "Orange", "Green", "Blue", "Purple", "Brown", "Black"
        };

        public static bool IsAllowed(string beltLevel)
            => Allowed.Contains(beltLevel);

        public static int Rank(string beltLevel)
        {
            var idx = Array.IndexOf(Allowed, beltLevel);
            return idx < 0 ? int.MaxValue : idx;
        }
    }
}
