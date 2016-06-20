using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;
using Microsoft.TeamFoundation.Controls.WPF.TeamExplorer;
using Microsoft.TeamFoundation.VersionControl.Client;
using Microsoft.TeamFoundation.VersionControl.Controls.Extensibility;

namespace Handy
{
    internal class HandyService
    {
        private readonly CheckinCommentService checkinCommentService;
        private readonly Guid pendingChangesGuid = Guid.Parse(TeamExplorerPageIds.PendingChanges);
        private readonly ITeamExplorer teamExplorer;

        public HandyService(ITeamExplorer teamExplorer, CheckinCommentService checkinCommentService)
        {
            this.teamExplorer = teamExplorer;
            this.checkinCommentService = checkinCommentService;
        }

        private void RegisterEvents()
        {
            this.teamExplorer.PropertyChanged += this.TeamExplorerOnPropertyChanged;
        }

        private void TeamExplorerOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (!propertyChangedEventArgs.PropertyName.Equals("CurrentPage") &&
                !propertyChangedEventArgs.PropertyName.Equals("TeamProjectNames"))
            {
                return;
            }

            var pendingChangesPage = this.teamExplorer.CurrentPage as TeamExplorerPageBase;
            if (pendingChangesPage == null)
            {
                return;
            }

            if (pendingChangesPage.GetId() != this.pendingChangesGuid)
            {
                return;
            }

            var pendingCheckin = pendingChangesPage.Model as IPendingCheckin;
            var pendingChangesExtensibility =
                pendingChangesPage.GetExtensibilityService(typeof (IPendingChangesExt)) as IPendingChangesExt;
            if (pendingChangesExtensibility == null)
            {
                return;
            }

            try
            {
                this.checkinCommentService.SetCheckinComment(pendingCheckin,
                    pendingChangesExtensibility.IncludedChanges);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void Start()
        {
            this.RegisterEvents();
            Console.WriteLine("Handy Started");
        }
    }
}