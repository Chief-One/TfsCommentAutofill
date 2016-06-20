using System;

namespace Handy
{
    public class PackageOptionsChangedEventArgs : EventArgs
    {
        public PackageOptionsChangedEventArgs(PackageOptions packageOptions)
        {
            this.PackageOptions = packageOptions;
        }

        public PackageOptions PackageOptions { get; private set; }
    }
}