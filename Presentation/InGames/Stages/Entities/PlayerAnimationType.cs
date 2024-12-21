namespace Kiro.Presentation
{
    /// <summary>
    ///     アニメーションの文字列のラッパー
    /// </summary>
    public readonly record struct PlayerAnimationType
    {
        public string Name { get; }

        PlayerAnimationType(string name)
        {
            Name = name;
        }

        public static PlayerAnimationType Idle => new("player-idle");
        public static PlayerAnimationType Up => new("player-up");
        public static PlayerAnimationType Down => new("player-down");
        public static PlayerAnimationType Left => new("player-left");
        public static PlayerAnimationType Right => new("player-right");
        public static PlayerAnimationType Clear => new("player-clear");
    }
}