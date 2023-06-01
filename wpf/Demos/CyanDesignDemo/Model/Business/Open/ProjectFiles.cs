using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyanDesignDemo.Model.Business.Open
{
    public class ProjectFiles : BaseModel
    {
        public string Title { get; set; } = "添加项目启动配置";

        public int ProjectFilesId { get; set; }
        public long ProjectFilesGuid { get; set; }
        public long CustomerGuid { get; set; }
        public long ProjectGuid { get; set; }
        public string ProjectFilesTitle { get; set; }
        public int ProjectFilesOpenMethodType { get; set; } = 2;
        public string ProjectFilesOpenMethodPath { get; set; }
        public int ProjectFilesFileOpenType { get; set; } = 1;
        public string ProjectFilesFileOpenPath { get; set; }
        public int ProjectFilesIsGit { get; set; }
        public int ProjectFilesIsOpen { get; set; } = 1;

        public bool IsProjectFilesOpen
        {
            get { return ProjectFilesIsOpen == 1; }
            set { ProjectFilesIsOpen = value == true ? 1 : 2; }
        }

        public bool IsProjectFilesIsGit
        {
            get { return ProjectFilesIsGit == 1; }
            set { ProjectFilesIsGit = value == true ? 1 : 2; }
        }

        public string ProjectFilesGitPath { get; set; }

        public List<ProjectFiles> ProjectFilesList { get; set; }



        public bool IsDirectOpen
        {
            get { return ProjectFilesOpenMethodType == 1; }
            set
            {
                if (value)
                    ProjectFilesOpenMethodType = 1;
                else if (ProjectFilesOpenMethodType == 1)
                    ProjectFilesOpenMethodType = 2;
            }
        }

        public bool IsChooseOpen
        {
            get { return ProjectFilesOpenMethodType == 2; }
            set
            {
                if (value)
                    ProjectFilesOpenMethodType = 2;
                else if (ProjectFilesOpenMethodType == 2)
                    ProjectFilesOpenMethodType = 1;
            }
        }

        public bool IsFolderSelected
        {
            get { return ProjectFilesFileOpenType == 1; }
            set
            {
                if (value)
                    ProjectFilesFileOpenType = 1;
                else if (ProjectFilesFileOpenType == 1)
                    ProjectFilesFileOpenType = 2;
            }
        }

        public bool IsFileSelected
        {
            get { return ProjectFilesFileOpenType == 2; }
            set
            {
                if (value)
                    ProjectFilesFileOpenType = 2;
                else if (ProjectFilesFileOpenType == 2)
                    ProjectFilesFileOpenType = 1;
            }
        }

    }

}
