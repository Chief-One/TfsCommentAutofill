using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Shell;

namespace Handy
{
    public class PackageOptions : DialogPage
    {
        public PackageOptions()
        {
            this.SprintIndex = 1;
            this.SprintStart = DateTime.Today;
            this.SprintLength = 14;
        }

        [Category("Handy")]
        [DisplayName("Current Sprint Index")]
        [Description("Current Sprint Index")]
        public int SprintIndex { get; set; }

        [Category("Handy")]
        [DisplayName("Current Sprint Start Date")]
        [Description("Current Sprint Start Date")]
        public DateTime SprintStart { get; set; }

        [Category("Handy")]
        [DisplayName("Sprint Length in Days")]
        [Description("Sprint Length in Days")]
        public int SprintLength { get; set; }

        public event EventHandler<PackageOptionsChangedEventArgs> OnPackageOptionsChanged;

        protected override void OnApply(PageApplyEventArgs e)
        {
            base.OnApply(e);
            this.SanitizeData();
            var handler = this.OnPackageOptionsChanged;
            if (handler != null)
            {
                handler(null, new PackageOptionsChangedEventArgs(this));
            }
        }

        private void SanitizeData()
        {
            this.SprintIndex = Math.Abs(this.SprintIndex);

            if (this.SprintLength < 1)
            {
                this.SprintLength = 1;
            }
        }
    }
}