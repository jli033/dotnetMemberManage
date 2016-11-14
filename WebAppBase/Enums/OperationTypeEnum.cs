using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebAppBase.Enums
{
    public enum OperationTypeEnum
    {
        //0.準備中 1.稼働中 2.分割済 3.出荷済み


        /// <summary>
        /// 準備中
        /// </summary>
        Preparing = 0,

        /// <summary>
        /// 稼働中
        /// </summary>
        Running = 1,

        /// <summary>
        /// 分割済
        /// </summary>
        Divided = 2,

        /// <summary>
        /// 出荷済み
        /// </summary>
        Shipped = 3
    }
}
