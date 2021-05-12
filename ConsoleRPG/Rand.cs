using System;

namespace ConsoleRPG
{
    [Serializable]
    public class Rand : Random
    {
        private static Rand _instant;
        public static Rand Instant => _instant ?? (_instant = new Rand());
        private Rand(){}
    }
}
