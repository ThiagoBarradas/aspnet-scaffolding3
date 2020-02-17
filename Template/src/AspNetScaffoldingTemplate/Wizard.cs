using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace AspNetScaffoldingTemplate
{
    public class Wizard : IWizard
    {
        private DTE2 Dte2 { get; set; }

        private Dictionary<string, string> Variables { get; set; }


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
            try
            {
                Solution2 solution = (Solution2) this.Dte2.Solution;

                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var templatePath = path + "\\ProjectTemplates\\AspNetScaffolding3";
                var solutionPath = this.Variables["$basedir$"];
                var mainProjectDir = this.Variables["$destinationdirectory$"] + "\\" + this.Variables["$safeprojectname$"] + ".Api";

                Projects projects = solution.Projects;
                var firstProject = projects.Cast<Project>().First();
                var solutionItems = (SolutionFolder)firstProject.Object;

                // git
                var git = solutionItems.AddSolutionFolder("git");
                git.ProjectItems.AddFromFileCopy(templatePath + "\\.gitignore");
                git.ProjectItems.AddFromFileCopy(templatePath + "\\.gitattributes");

                // github
                var github = solutionItems.AddSolutionFolder("github");
                github.ProjectItems.AddFromFileCopy(templatePath + "\\README.md");

                Directory.CreateDirectory(solutionPath + "\\.github");
                File.Copy(templatePath + "\\.github\\CONTRIBUTING.md", solutionPath + "\\.github\\CONTRIBUTING.md");
                File.Copy(templatePath + "\\.github\\ISSUE_TEMPLATE.md", solutionPath + "\\.github\\ISSUE_TEMPLATE.md");
                File.Copy(templatePath + "\\.github\\PULL_REQUEST_TEMPLATE.md", solutionPath + "\\.github\\PULL_REQUEST_TEMPLATE.md");

                github.ProjectItems.AddFromFile(solutionPath + "\\.github\\CONTRIBUTING.md");
                github.ProjectItems.AddFromFile(solutionPath + "\\.github\\ISSUE_TEMPLATE.md");
                github.ProjectItems.AddFromFile(solutionPath + "\\.github\\PULL_REQUEST_TEMPLATE.md");

                // devops
                var devops = solutionItems.AddSolutionFolder("devops");

                Directory.CreateDirectory(solutionPath + "\\devops");
                File.Copy(templatePath + "\\devops\\azure-pipelines.yml", solutionPath + "\\devops\\azure-pipelines.yml");
                File.Copy(templatePath + "\\devops\\docker-compose.yml", solutionPath + "\\devops\\docker-compose.yml");
                File.Copy(templatePath + "\\devops\\Dockerfile", solutionPath + "\\devops\\Dockerfile");

                devops.ProjectItems.AddFromFile(solutionPath + "\\devops\\azure-pipelines.yml");
                devops.ProjectItems.AddFromFile(solutionPath + "\\devops\\docker-compose.yml");
                devops.ProjectItems.AddFromFile(solutionPath + "\\devops\\Dockerfile");
                
                // docs
                var docs = solutionItems.AddSolutionFolder("docs");
                docs.ProjectItems.AddFromFile(mainProjectDir + "\\Docs.md");

                foreach (var openedDocument in this.Dte2.Documents.Cast<Document>())
                {
                    openedDocument.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocorreu um errinho aqui, mas tenta rodar como administrador pra ver se é permissão :)", "Ops!");
            }
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

                replacementsDictionary.Add("$basedir$", replacementsDictionary["$solutiondirectory$"]);
                
                this.Variables = replacementsDictionary;
                this.Dte2 = (DTE2)automationObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}
