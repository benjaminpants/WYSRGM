using GmmlPatcher;
using UndertaleModLib;
using UndertaleModLib.Models;
using System;
using WysApi.Api;
using System.IO;
using TSIMPH;
using UndertaleModLib.Decompiler;
using System.Reflection;
using GmmlHooker;
using Newtonsoft.Json;


namespace WYSGMRoomReader
{
    public class GameMakerMod : IGameMakerMod
    {
        public void Load(int audioGroup, UndertaleData data, ModData currentMod)
        {

            if (audioGroup != 0) return;
            
        }
    }

    public static class RoomReader
    {
        public static UndertaleRoom CreateRoomFromFile(UndertaleData data, string room_folder, string room_name, bool instantapply, out Dictionary<UndertaleRoom.GameObject,string> CreationCodesToApply)
        {
            UndertaleRoom rm = Conviences.CreateBlankLevelRoom(room_name, data);

            CreationCodesToApply = new Dictionary<UndertaleRoom.GameObject, string>();

            RoomData rmdata = JsonConvert.DeserializeObject<RoomData>(File.ReadAllText(Path.Combine(Path.Combine(room_folder, room_name), room_name + ".yy")));

            Patcher.AddFileToCache(0, Path.Combine(Path.Combine(room_folder, room_name), room_name + ".yy"));

            foreach (LayerData ld in rmdata.layers)
            {
                if (ld.instances != null)
                {
                    foreach (LayerObject item in ld.instances)
                    {
                        UndertaleRoom.GameObject ugo = rm.AddObjectToLayer(data, item.objectId.name, ld.name);
                        ugo.ScaleX = item.scaleX;
                        ugo.ScaleY = item.scaleY;
                        ugo.X = (int)item.x;
                        ugo.Y = (int)item.y;
                        ugo.Rotation = item.rotation;
                        string creation_code_local = Path.Combine(room_folder, room_name, "InstanceCreationCode_" + item.name + ".gml");
                        if (File.Exists(creation_code_local))
                        {
                            Patcher.AddFileToCache(0, creation_code_local);
                            if (instantapply)
                            {
                                ugo.CreationCode = data.CreateCode(item.name + "_CreationCode", File.ReadAllText(creation_code_local), 0);
                            }
                            else
                            {
                                CreationCodesToApply.Add(ugo, File.ReadAllText(creation_code_local));
                            }
                            Logger.Log(item.name + " has creation code");
                        }
                    }
                }
            }

            rm.Width = (uint)rmdata.roomSettings.Width;
            rm.Height = (uint)rmdata.roomSettings.Height;

            return rm;
        }
    
        public static UndertaleRoom CreateRoomFromFile(UndertaleData data, string room_folder, string room_name)
        {
            return CreateRoomFromFile(data,room_folder,room_name,true, out Dictionary<UndertaleRoom.GameObject,string> ccta);
        }
    
    }
    
}