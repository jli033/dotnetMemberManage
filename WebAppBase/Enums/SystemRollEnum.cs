namespace WebAppBase.Enums
{
    /// <summary>
    /// アクセス権限
    /// </summary>
    public enum SystemRollEnum
    {
        //0:閲覧のみ　1:利用者　2:管理者 3:システム管理者        
        /// <summary>
        /// 閲覧のみ（ログインなし）
        /// </summary>
        Visitor = 0,
        /// <summary>
        /// 一般ユーザー
        /// </summary>
        User=1,
        /// <summary>
        /// 事業所管理者
        /// </summary>
        Administrator = 2,
        /// <summary>
        /// システム管理者
        /// </summary>
        SysAdmin=3
    }
}
