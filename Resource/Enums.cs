namespace Nomadicooer.rimworld.prf
{
    public enum MatchMode
    {
        Any,
        All
    }
    public enum FilterMode
    {
        OneInMillion,
        ChosenOne
    }
    public enum HealthState
    {
        Any,
        Nothing,
        Exsit
    }
    public enum DisableWorkState
    {
        Any,
        Nothing,
        Exsit
    }
    public enum RelationshipState
    {
        Any,
        Nothing,
        Exsit
    }
    public enum StopRandomReason
    {
        //未知
        None,
        //用户停止
        User,
        //查找到
        Find,
        //达到随机最大次数
        MaxTimes,
        //角色为空
        PawnNull
    }
}
