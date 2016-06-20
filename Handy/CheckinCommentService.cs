using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Handy
{
    internal class CheckinCommentService : ICheckinCommentService
    {
        private const string MainTeamBranchPath = "$/HCHB/2.MAIN";
        private PackageOptions packageOptions;

        public CheckinCommentService(PackageOptions packageOptions)
        {
            this.packageOptions = packageOptions;
            this.RegisterEvents();
        }

        public void SetCheckinComment(IPendingCheckin pendingCheckin, IEnumerable<PendingChange> pendingChanges)
        {
            if (pendingCheckin == null)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(pendingCheckin.PendingChanges.Comment))
            {
                return;
            }

            pendingCheckin.PendingChanges.Comment = this.GetCommentText(pendingChanges);
        }

        private void RegisterEvents()
        {
            this.packageOptions.OnPackageOptionsChanged += this.OnPackageOptionsChanged;
        }

        private void OnPackageOptionsChanged(object sender, PackageOptionsChangedEventArgs e)
        {
            this.packageOptions = e.PackageOptions;
        }

        private string GetCommentText(IEnumerable<PendingChange> pendingChanges)
        {
            var changes = pendingChanges as IList<PendingChange> ?? pendingChanges.ToList();

            if (!changes.Any())
            {
                return string.Empty;
            }

            var isMainBranchCheckin = changes.All(x => x.ServerItem.ToUpper().Contains(MainTeamBranchPath));
            var sprintIndex = this.GetSprintIndex();

            return isMainBranchCheckin
                ? string.Format("WAG-S{0}-R2/PointCare-", sprintIndex)
                : string.Format("WAG-S{0}-R2/PointCare-US", sprintIndex);
        }

        private int GetSprintIndex()
        {
            var daysIntoSprint = (DateTime.Today - this.packageOptions.SprintStart).Days;
            return daysIntoSprint/this.packageOptions.SprintLength + this.packageOptions.SprintIndex;
        }
    }
}