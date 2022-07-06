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
        public MetadataSaveData MetaData { get; set; } = new MetadataSaveData();


        public PlayerAvatarData PlayerAvatarData { get; set; }

        /// <summary>
        /// Creates new save file which includes: New folder, new metadata, and new save data
        /// </summary>
        /// <param name="name"></param>
        internal void CreateNew(string name, PlayerAvatarData playerAvatarData)
        {
            MetaData.Name = name;
            MetaData.FolderPath = SaveLoadManager.BasePath + @"\" + MetaData.Name;
            MetaData.MainSaveFilePath = MetaData.FolderPath + @"\" + MetaData.Name + ".dat";
            MetaData.StageFilePath = MetaData.FolderPath + @"\" + "Stages";
            Directory.CreateDirectory(MetaData.FolderPath);
            Directory.CreateDirectory(MetaData.StageFilePath);

            File.WriteAllText(MetaData.MainSaveFilePath, string.Empty);

           CreateNewMetadata(MetaData.FolderPath);

            PlayerAvatarData = playerAvatarData;



        }

        public void Delete()
        {
            Directory.Delete(MetaData.FolderPath, true);
        }
        /// <summary>
        /// Creates the metadata file 
        /// </summary>
        private void CreateNewMetadata(string folderPath)
        {
            string fileName = "MetaData_" + MetaData.Name;
            MetaData.DateCreated = DateTime.Now;
            MetaData.MetaDataPath = folderPath + @"\" + fileName + ".json";
        }

        private void CreateNewAvatarPath()
        {
            string fileName = "Player_Avatar_" + MetaData.Name;
            PlayerAvatarData.Path = MetaData.FolderPath + @"\" + fileName + ".json";
        }

        public void Save()
        {
            string jsonString = JsonSerializer.Serialize(MetaData);
            File.WriteAllText(MetaData.MetaDataPath, jsonString);

            jsonString = JsonSerializer.Serialize(PlayerAvatarData);
            CreateNewAvatarPath();
            File.WriteAllText(PlayerAvatarData.Path, jsonString);


        }

        public void LoadMetaData(string fullFilePath)
        {
            string jsonString = File.ReadAllText(fullFilePath);
            MetaData = JsonSerializer.Deserialize<MetadataSaveData>(jsonString);
        }

        public void LoadPlayerAvatarData(string fullFilePath)
        {
            string jsonString = File.ReadAllText(fullFilePath);

            PlayerAvatarData = JsonSerializer.Deserialize<PlayerAvatarData>(jsonString);

        }
    }
}
