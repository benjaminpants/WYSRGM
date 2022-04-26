using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UndertaleModLib;
using UndertaleModLib.Models;
using Newtonsoft.Json;

namespace WYSGMRoomReader
{
    public class RoomData
    {
        public bool isDnd = false;
        public RoomSettings roomSettings = new RoomSettings();
        public LayerData[] layers;


    }

    public class ObjectID
    {
        public string name = "obj_wall";
    }
    
    public class RoomSettings
    {
        public int Width;
        public int Height;
    }

    public class LayerData
    {
        public string name = "UNDEFINED";
        public LayerObject[] instances;
    }

    public class LayerObject
    {
        public ObjectID objectId = new ObjectID();
        public string name = "inst_WHATEVER";
        public float scaleX = 1;
        public float scaleY = 1;
        public float rotation = 0;
        public float x;
        public float y;
    }
}
