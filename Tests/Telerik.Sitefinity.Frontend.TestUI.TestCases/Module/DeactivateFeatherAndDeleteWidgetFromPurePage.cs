﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.Sitefinity.Frontend.TestUI.Framework;
using Telerik.Sitefinity.Frontend.TestUtilities;
using Telerik.Sitefinity.TestUI.Framework.Utilities;

namespace Telerik.Sitefinity.Frontend.TestUI.TestCases.Module
{
    /// <summary>
    /// Deactivates Feather and checks if widget on pure page is visible in the frontend and if it can be deleted in the backend.
    /// </summary>
    [TestClass]
    public class DeactivateFeatherAndDeleteWidgetFromPurePage_ : FeatherTestCase
    {
        /// <summary>
        /// Deactivates the feather and delete widget from pure page.
        /// </summary>
        [TestMethod]
        [Owner(FeatherTeams.FeatherTeam)]
        [TestCategory(FeatherTestCategories.PagesAndContent)]
        public void DeactivateFeatherAndDeleteWidgetFromPurePage()
        {
            var featherDeactivated = false;

            try
            {
                // Add widget to page
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().User().EnsureAdminLoggedIn());
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(PagesPageUrl, false));
                BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
                BATFrontend.Wrappers().Backend().Pages().PageZoneEditorWrapper().DragAndDropWidgetToPlaceholder(WidgetName, Placeholder);
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().CheckWidgetContent(WidgetName, WidgetContent);
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

                // Verify on frontend
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(PageUrl, false));
                Assert.IsTrue(BAT.Wrappers().Frontend().Pages().PagesWrapperFrontend().IsHtmlControlPresent(WidgetContent));

                // Deactivate Feather
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(ModulesAndServicesPageUrl, false));
                BAT.Wrappers().Backend().ModulesAndServices().ModulesAndServicesWrapper().DeactivateModule(FeatherModuleName);
                featherDeactivated = true;

                // Verify on frontend
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(PageUrl, false));
                Assert.IsFalse(BAT.Wrappers().Frontend().Pages().PagesWrapperFrontend().IsHtmlControlPresent(WidgetContent));

                // Remove widget from page
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().User().EnsureAdminLoggedIn());
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(PagesPageUrl, false));
                BAT.Wrappers().Backend().Pages().PagesWrapper().OpenPageZoneEditor(PageName);
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().DeleteWidget(WidgetName);
                Assert.IsFalse(BAT.Wrappers().Frontend().Pages().PagesWrapperFrontend().IsHtmlControlPresent(WidgetContent));
                BAT.Wrappers().Backend().Pages().PageZoneEditorWrapper().PublishPage();

                // Verify on frontend
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(PageUrl, false));
                Assert.IsFalse(BAT.Wrappers().Frontend().Pages().PagesWrapperFrontend().IsHtmlControlPresent(WidgetContent));

                // Activate Feather
                RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(ModulesAndServicesPageUrl, false));
                BAT.Wrappers().Backend().ModulesAndServices().ModulesAndServicesWrapper().ActivateModule(FeatherModuleName);
                featherDeactivated = false;
            }
            finally 
            {
                if (featherDeactivated)
                {
                    // Activate Feather if Test Failed
                    RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().User().EnsureAdminLoggedIn());
                    RuntimeSettingsModificator.ExecuteWithClientTimeout(ClientTimeoutInterval, () => BAT.Macros().NavigateTo().CustomPage(ModulesAndServicesPageUrl, false));
                    BAT.Wrappers().Backend().ModulesAndServices().ModulesAndServicesWrapper().ActivateModule(FeatherModuleName);
                }
            }
        }
        
        /// <summary>
        /// Performs Server Setup and prepare the system with needed data.
        /// </summary>
        protected override void ServerSetup()
        {
            BAT.Arrange(this.TestName).ExecuteSetUp();
        }

        /// <summary>
        /// Performs clean up and clears all data created by the test.
        /// </summary>
        protected override void ServerCleanup()
        {
            BAT.Arrange(this.TestName).ExecuteTearDown();
        }

        private readonly string PageUrl = "~/" + PageName.ToLower();

        private const string PageName = "Page_DeactivateFeatherAndDeleteWidgetFromPurePage";

        private const int ClientTimeoutInterval = 80000;

        private const string ModulesAndServicesPageUrl = "~/Sitefinity/Administration/ModulesAndServices";
        private const string PagesPageUrl = "~/sitefinity/pages";

        private const string FeatherModuleName = "Feather";
        private const string Placeholder = "Body";

        private const string WidgetName = "ModuleTestsWidget";
        private const string WidgetContent = "ca9af596-eaa3-44ed-a654-0e9170266a36";
    }
}
