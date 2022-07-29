using Globals.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiledEngine.Classes.TileAddons.LightStuff
{
    public class TileLightManager : ISaveable
    {
        private Dictionary<int, TileLightDataDTO> _lightDictionary;

        public TileLightManager()
        {
            _lightDictionary = new Dictionary<int,TileLightDataDTO>();
        }
        public void AddNewItem(TileLightDataDTO tileLightDTO)
        {
            if (!_lightDictionary.ContainsKey(tileLightDTO.Key))
                _lightDictionary.Add(tileLightDTO.Key, tileLightDTO);
            else
                throw new Exception($"Duplicate light key");

        }

        public void Update(TileLightDataDTO dto)
        {
            _lightDictionary[dto.Key] = dto;
        }


        public void RemoveLightFromTile(TileObject tile)
        {
            if (_lightDictionary.ContainsKey(tile.TileData.GetKey()))
                _lightDictionary.Remove(tile.TileData.GetKey());

        }
        public TileLightDataDTO? GetLightFromTile(TileObject tile)
        {
            if (_lightDictionary.ContainsKey(tile.TileData.GetKey()))
            {
                return _lightDictionary[tile.TileData.GetKey()];
            }
            return null;
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(_lightDictionary.Count);

            foreach (KeyValuePair<int, TileLightDataDTO> kvp in _lightDictionary)
            {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value.TimeCreated);

                writer.Write(kvp.Value.CurrentCharge);


            }

        }

        public void LoadSave(BinaryReader reader)
        {
            int count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                TileLightDataDTO dto = new TileLightDataDTO(reader.ReadInt32(), reader.ReadSingle(), reader.ReadByte());

            }
        }

        public void CleanUp()
        {
            _lightDictionary.Clear();
        }

        public void SetToDefault(BinaryWriter writer)
        {
            //

        }

        public void SetToDefault()
        {
            CleanUp();
        }
    }
}
