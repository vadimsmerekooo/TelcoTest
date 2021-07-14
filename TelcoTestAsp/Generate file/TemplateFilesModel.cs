using System.IO;

namespace TelcoTestAsp.Generate_file
{
    public class TemplateFilesModel
    {
        public string PathTemplateStructureFile { get; set; }
        public string PathTemplateRowTaskFile { get; set; }
        public string PathTemplateRowHeaderTaskElementFile { get; set; }
        public string PathTemplateRowTaskElementFile { get; set; }
        private bool IsFileExists { get; set; }
        public TemplateFilesModel(string pathTemplateStructureFile, string pathTemplateRowTaskFile, string pathTemplateRowHeaderTaskElementFile, string pathTemplateRowTaskElementFile)
        {
            PathTemplateStructureFile = pathTemplateStructureFile;
            PathTemplateRowTaskFile = pathTemplateRowTaskFile;
            PathTemplateRowHeaderTaskElementFile = pathTemplateRowHeaderTaskElementFile;
            PathTemplateRowTaskElementFile = pathTemplateRowTaskElementFile;
            IsFileExists = IsFileTemplateExists();
        }
        public bool GetIsFileExists() => IsFileExists;

        private bool IsFileTemplateExists()
        {
            return File.Exists(PathTemplateStructureFile) && File.Exists(PathTemplateRowTaskFile) && File.Exists(PathTemplateRowHeaderTaskElementFile) && File.Exists(PathTemplateRowTaskElementFile);
        }

    }
}
