using System;
using System.ComponentModel;

namespace RSoft.Framework.Cross.Enums
{

    /// <summary>
    /// User type
    /// </summary>
    public enum UserType
    {

        [Description("A human user")]
        User = 1,

        [Description("An application or service")]
        Service = 2

    }
}
