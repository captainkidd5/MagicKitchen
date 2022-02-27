using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IOEngine.Classes
{
    public class SaveFile 
    {

        public string Name { get; private set; }

        public string FolderPath { get; private set; }

        //The main save file, where all game data is saved to
        public string MainSaveFilePath { get; private set; }


        //METADATA - the file where the main menu reads about each save file
        //without having to actually read the entirety of each save file bc that would be v slow
        public string MetaDataPath { get; private set; }

        //The date the new save was created on
        public DateTime DateCreated { get; private set; }

        public string StageFilePath { get; private set; }


        /// <summary>
        /// Creates new save file which includes: New folder, new metadata, and new save data
        /// </summary>
        /// <param name="name"></param>
        internal void CreateNew(string name)
        {
            Name = name;
            FolderPath = SaveLoadManager.BasePath + @"\" + Name;
            MainSaveFilePath = FolderPath + @"\" + Name + ".dat";
            StageFilePath = FolderPath + @"\" + "Stages";
            Directory.CreateDirectory(FolderPath);
            Directory.CreateDirectory(StageFilePath);

            File.WriteAllText(MainSaveFilePath, string.Empty);

           CreateNewMetadata(FolderPath);

        }

        public void Delete()
        {
            Directory.Delete(FolderPath, true);
        }
        /// <summary>
        /// Creates the metadata file 
        /// </summary>
        private void CreateNewMetadata(string folderPath)
        {

            string fileName = "MetaData_" + Name;
            DateCreated = DateTime.Now;
            MetaDataPath = folderPath + @"\" + fileName + ".dat";
            File.WriteAllText(MetaDataPath, string.Empty);
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Name);
            writer.Write(DateCreated.ToBinary());
            writer.Write(FolderPath);
            writer.Write(MainSaveFilePath);
            writer.Write(StageFilePath);
            
        }

        public void LoadSave(BinaryReader reader)
        {
            Name = reader.ReadString();
            DateCreated = DateTime.FromBinary(reader.ReadInt64());
            FolderPath = reader.ReadString();
            MainSaveFilePath = reader.ReadString();
            StageFilePath = reader.ReadString();
        }
    }
}
