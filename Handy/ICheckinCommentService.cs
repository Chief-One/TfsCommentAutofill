using System.Collections.Generic;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace Handy
{
    internal interface ICheckinCommentService
    {
        void SetCheckinComment(IPendingCheckin pendingCheckin, IEnumerable<PendingChange> pendingChanges);
    }
}