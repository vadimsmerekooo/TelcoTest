using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TelcoTestAsp.Models;

namespace TelcoTestAsp.Generate_file
{
    public class GenerateXlsx
    {
        private string EmptyRow { get; } = "<Row><Cell></Cell></Row>";
        private TemplateFilesModel TemplateFilesModel { get; set; }
        private List<Task> Tasks { get; set; }

        public GenerateXlsx(TemplateFilesModel templateFilesModel, List<Task> tasks)
        {
            TemplateFilesModel = templateFilesModel;
            Tasks = tasks;
        }

        public T GenerateXlsxFile<T>()
        {
            if (!TemplateFilesModel.GetIsFileExists())
            {
                return (T)Convert.ChangeType(new Exception("TemplateFilesModel return false, on check exists file template!"), typeof(T));
            }

            return (T)Convert.ChangeType(GenerateRowsTask(), typeof(T));
        }

        private byte[] GenerateRowsTask()
        {
            string templateRow = File.ReadAllText(TemplateFilesModel.PathTemplateRowTaskFile);
            StringBuilder taskRows = new StringBuilder();

            foreach (Task taskItem in Tasks)
            {
                taskRows.AppendLine(templateRow.ToString()
                    .Replace("Id", taskItem.Id.ToString())
                    .Replace("Name", taskItem.Name)
                    .Replace("Description", taskItem.Description)
                    .Replace("Start_date", taskItem.Start_date.ToShortDateString())
                    .Replace("End_date", taskItem.End_date.ToShortDateString())
                    .Replace("Status", taskItem.Status.ToString()));
                if(taskItem.TaskElements != null && taskItem.TaskElements.Count() != 0)
                {
                    taskRows.AppendLine(File.ReadAllText(TemplateFilesModel.PathTemplateRowHeaderTaskElementFile));

                    string templateTaskElementRow = File.ReadAllText(TemplateFilesModel.PathTemplateRowTaskElementFile);

                    foreach (TaskElement elementItem in taskItem.TaskElements)
                    {
                        taskRows.AppendLine(templateTaskElementRow.ToString()
                        .Replace("Id", elementItem.Id.ToString())
                        .Replace("Name", elementItem.Name)
                        .Replace("Description", elementItem.Description)
                        .Replace("Start_date", elementItem.Value));
                    }
                }
                taskRows.AppendLine(EmptyRow);
            }

            StringBuilder structureFileTemplate = new StringBuilder(File.ReadAllText(TemplateFilesModel.PathTemplateStructureFile));
            structureFileTemplate.Replace("//RowTableTaskWithElements", taskRows.ToString());


            return Encoding.UTF8.GetBytes(structureFileTemplate.ToString());
        }

    }
}
