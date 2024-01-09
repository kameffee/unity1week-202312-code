namespace Unity1week202312.InGame
{
    public enum InGameUserStateType
    {
        // 何もしていない状態
        Ready,

        // 掴んだ状態
        Grabbing,

        // パッキング可能
        Packable,

        // パッキング中
        Packing,
    }
}