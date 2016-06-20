using System.Runtime.InteropServices;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Handy
{
    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("Handy", "Auto populate TFS check-in commentsTFS", "1.0", IconResourceID = 400)]
    [Guid(PackageGuidString)]
    [ProvideAutoLoad(UIContextGuids.SolutionExists)]
    [ProvideOptionPage(typeof (PackageOptions),
        "Handy", "Configure Options", 0, 0, true)]
    public sealed class HandyPackage : Package
    {
        /// <summary>
        ///     Handy GUID string.
        /// </summary>
        public const string PackageGuidString = "77ac61ff-903b-4b1b-b0f8-abde3e0668d5";

        private HandyService handyService;

        #region Package Members

        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            var teamExplorer = this.GetService(typeof (ITeamExplorer)) as ITeamExplorer;
            var packageOptions = this.GetDialogPage(typeof (PackageOptions)) as PackageOptions;
            this.handyService = new HandyService(teamExplorer, new CheckinCommentService(packageOptions));
            this.handyService.Start();
        }

        #endregion
    }
}