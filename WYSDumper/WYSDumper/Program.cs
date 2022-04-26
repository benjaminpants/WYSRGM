// See https://aka.ms/new-console-template for more information
using UndertaleModLib;
using UndertaleModLib.Models;
using UndertaleModLib.Util;
using WYSDumper;


string fileName = "";

string export_folder = "";

string project_folder = "";

UndertaleData data = null;


Console.WriteLine("Please type out the file name of the data.win file you want to use.");
fileName = Console.ReadLine();
Console.WriteLine("Please type temp data folder output(YOU WILL NEED TO ACCESS THIS FOLDER)");
export_folder = Console.ReadLine();
Console.WriteLine("Please type the GMS2 Project folder location");
project_folder = Console.ReadLine();

Console.WriteLine("Please note that this process isn't fully automated, so you'll have to do some manual work, you'll be prompted to do so, keep your attention on this prompt.");
Console.WriteLine("Press any key to continue.");
Console.ReadKey();

try
{

    FileStream strm = File.OpenRead(fileName);
    data = UndertaleIO.Read(strm);
    strm.Close();

}
catch (Exception E)
{
    Console.WriteLine("Encountered an error while reading the file: " + E.Message);
    
    return 1;
}

TextureWorker worker = new TextureWorker();


string sprites_dir = Path.Combine(export_folder, "Sprites");

string objects_dir = Path.Combine(project_folder, "objects");

Directory.CreateDirectory(sprites_dir);

Console.WriteLine("Dumping Sprites that are used by objects.");


foreach (UndertaleGameObject item in data.GameObjects)
{
    if (item.Sprite != null)
    {
        if (item.Sprite.Textures.Count != 0)
        {
            if (!File.Exists(Path.Combine(sprites_dir, item.Sprite.Name.Content + ".png")))
            {
                worker.ExportAsPNG(item.Sprite.Textures[0].Texture, Path.Combine(sprites_dir, item.Sprite.Name.Content + ".png"),null,true);
            }
        }
    }
}

Console.WriteLine("Select everything in the sprites folder and drag it onto the sprites tab in Game Maker Studio 2. Once you do and you see the sprites appear in the editor, close Game Maker Studio 2 and press any key to allow the program to correct offsets.");

Console.ReadKey();

string project_addon = "";

foreach (UndertaleGameObject item in data.GameObjects)
{
    if (item.Sprite != null)
    {
        if (item.Sprite.Textures.Count != 0)
        {
            if (Directory.Exists(Path.Combine(project_folder, "sprites", item.Sprite.Name.Content)))
            {
                string yy = Path.Combine(project_folder, "sprites", item.Sprite.Name.Content, item.Sprite.Name.Content + ".yy");
                if (File.Exists(yy))
                {
                    string yy_data = File.ReadAllText(yy);
                    yy_data = yy_data.Replace("\"type\": 0,", "\"type\": 9,");
                    
                    yy_data = yy_data.Replace("\"xorigin\": 0,", $"\"xorigin\": {item.Sprite.OriginX},");

                    yy_data = yy_data.Replace("\"yorigin\": 0,", $"\"yorigin\": {item.Sprite.OriginY},");

                    File.WriteAllText(yy,yy_data);

                    Console.WriteLine("Corrected offsets for:" + item.Sprite.Name.Content);
                }
            }
        }
    }
    string obj_path = Path.Combine(objects_dir, item.Name.Content);
    Directory.CreateDirectory(obj_path);
    string yyc_write = item.Sprite == null ? Constants.ObjectYYTemplateSpriteless : Constants.ObjectYYTemplate;
    File.WriteAllText(Path.Combine(obj_path, item.Name.Content + ".yy"), yyc_write.Replace("#sprite#", item.Sprite == null ? "amogus" : item.Sprite.Name.Content).Replace("#object#",item.Name.Content));
    project_addon = project_addon + Constants.ProjectStringEntry.Replace("#object#",item.Name.Content) + "\n";
}

string yp = Directory.GetFiles(project_folder, "*.yyp")[0];

string yp_dat = File.ReadAllText(yp);

yp_dat = yp_dat.Replace("\"resources\": [", "\"resources\": [\n" + project_addon);

File.WriteAllText(yp, yp_dat);



return 0;
