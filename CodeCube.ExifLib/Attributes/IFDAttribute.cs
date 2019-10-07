using System;
using CodeCube.ExifLib.Enumerations;

namespace CodeCube.ExifLib.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    internal class IFDAttribute : Attribute
    {
        public readonly IFD IFD;

        public IFDAttribute(IFD ifd)
        {
            IFD = ifd;
        }
    }
}