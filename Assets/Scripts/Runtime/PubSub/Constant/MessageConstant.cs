namespace Runtime.PubSub.Constant
{
    public struct MessageConstant
    {
        public const int DEFAULT_ORDER = 0;
        public const int LAST_ORDER = int.MaxValue;
        public const int FIRST_ORDER = int.MinValue;
        public const int AFTER_DEFAULT_ORDER = 1;
        public const int BEFORE_DEFAULT_ORDER = -1;
    }
}
