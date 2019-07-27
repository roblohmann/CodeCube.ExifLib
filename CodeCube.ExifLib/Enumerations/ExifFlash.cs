using System;

namespace CodeCube.ExifLib.Enumerations
{
    [Flags]
    public enum ExifFlash
    {
        No = 0,
        Fired = 1,
        StrobeReturnLightDetected = 6,
        On = 8,
        Off = 16, // 0x00000010
        Auto = Off | On, // 0x00000018
        FlashFunctionPresent = 32, // 0x00000020
        RedEyeReduction = 64, // 0x00000040
    }
}
