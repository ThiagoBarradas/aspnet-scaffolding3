using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AspNetScaffoldingTemplate
{
    public class Wizard : IWizard
    {
        private DTE Dte { get; set; }

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
            //var github = this.GetSolutionFolderProject("github");
            //github.ProjectItems.AddFromFile("README.md");
            //github.ProjectItems.AddFromFile(".github\\CONTRIBUTING.md");
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var form = new AspNetScaffoldingAdditionalData();
                form.ShowDialog();

                replacementsDictionary.Add("$company$", AspNetScaffoldingAdditionalData.Company);
                replacementsDictionary.Add("$lower_company$", AspNetScaffoldingAdditionalData.Company.ToLower());
                replacementsDictionary.Add("$team$", AspNetScaffoldingAdditionalData.Team);
                replacementsDictionary.Add("$lower_team$", AspNetScaffoldingAdditionalData.Team.ToLower());
                replacementsDictionary.Add("$path$", AspNetScaffoldingAdditionalData.Path);
                replacementsDictionary.Add("$version$", AspNetScaffoldingAdditionalData.Version);
                replacementsDictionary.Add("$author_name$", AspNetScaffoldingAdditionalData.AuthorName);
                replacementsDictionary.Add("$author_email$", AspNetScaffoldingAdditionalData.AuthorEmail);
                replacementsDictionary.Add("$hc_token$", (Guid.NewGuid().ToString() + Guid.NewGuid().ToString()).Replace("-", "").ToLower());

                this.Dte = (DTE) automationObject;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        private Project GetSolutionFolderProject(string folder)
        {
            var project =
                this.Dte.Solution.Projects.Cast<Project>()
                    .FirstOrDefault(p => p.Name == folder);

            return project;
        }
    }
}
