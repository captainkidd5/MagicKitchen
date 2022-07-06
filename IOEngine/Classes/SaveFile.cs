using DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace IOEngine.Classes
{
    public class SaveFile 
    {
        public MetadataSaveData MetadataSaveData { get; set; } = new MetadataSaveData();



        /// <summary>
        /// Creates new save file which includes: New folder, new metadata, and new save data
        /// </summary>
        /// <param name="name"></param>
        internal void CreateNew(string name)
        {
            MetadataSaveData.Name = name;
            MetadataSaveData.FolderPath = SaveLoadManager.BasePath + @"\" + MetadataSaveData.Name;
            MetadataSaveData.MainSaveFilePath = MetadataSaveData.FolderPath + @"\" + MetadataSaveData.Name + ".dat";
            MetadataSaveData.StageFilePath = MetadataSaveData.FolderPath + @"\" + "Stages";
            Directory.CreateDirectory(MetadataSaveData.FolderPath);
            Directory.CreateDirectory(MetadataSaveData.StageFilePath);

            File.WriteAllText(MetadataSaveData.MainSaveFilePath, string.Empty);

           CreateNewMetadata(MetadataSaveData.FolderPath);

        }

        public void Delete()
        {
            Directory.Delete(MetadataSaveData.FolderPath, true);
        }
        /// <summary>
        /// Creates the metadata file 
        /// </summary>
        private void CreateNewMetadata(string folderPath)
        {
            string fileName = "MetaData_" + MetadataSaveData. Name;
            MetadataSaveData.DateCreated = DateTime.Now;
            MetadataSaveData.MetaDataPath = folderPath + @"\" + fileName + ".json";
           // File.WriteAllText(MetaDataPath, string.Empty);
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(MetadataSaveData);
            File.WriteAllText(MetadataSaveData.MetaDataPath, jsonString);

            
        }

        public void LoadSave(string fullFilePath)
        {
            string jsonString = File.ReadAllText(fullFilePath);
            MetadataSaveData = JsonSerializer.Deserialize<MetadataSaveData>(jsonString);
    
        }
    }
}
