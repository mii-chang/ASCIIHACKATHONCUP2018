public static partial class Const {
    public enum Team {
        team1 = 1,
        team2
    }

    public static int ToInt(this Team teamType) {
        return (int)teamType;
    }
}