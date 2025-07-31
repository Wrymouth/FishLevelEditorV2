using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.DataAccess
{
    public class LevelRepository : ILevelRepository
    {
        public string FolderPath { get; set; }
        public ExportSettings ExportSettings { get; set; }

        public LevelRepository(ExportSettings exportSettings)
        {
            ExportSettings = exportSettings;
        }

        public void Export(Level level, string folderPath)
        {
            FolderPath = folderPath;
            ExportMetatileSet(level.MetatileSet);
            ExportLevelTiles(level);
            if (ExportSettings.SaveAttributes)
            {
                ExportAttributes(level);
            }
            ExportLevelData(level);
        }

        private void ExportMetatileSet(MetatileSet metatileSet)
        {
            for (int i = 0; i < Enum.GetValues(typeof(Metatile.Entries)).Length; i++)
            {
                string fileName = $"{FolderPath}{metatileSet.FileName}_";
                if (i == (int)Metatile.Entries.Collision)
                {
                    fileName += "collision.bin";
                }
                else
                {
                    fileName += $"{i}.bin";
                }
                byte[] metatiles = GetMetatileBytes(metatileSet, i);
                File.WriteAllBytes(fileName, metatiles);
            }
        }

        private byte[] GetMetatileBytes(MetatileSet metatileSet, int index)
        {
            byte[] metatiles = new byte[256];

            for (int i = 0; i < metatileSet.Metatiles.Count; i++)
            {
                metatiles[i] = GetMetatileIndex(metatileSet, index, i);
            }
            return metatiles;
        }

        private static byte GetMetatileIndex(MetatileSet metatileSet, int entry, int arrayIndex)
        {
            if (entry == (int)Metatile.Entries.Collision)
            {
                return (byte)metatileSet.Metatiles[arrayIndex].Type;
            }
            else
            {
                return (byte)metatileSet.Metatiles[arrayIndex].Tiles[entry];
            }
        }

        private void ExportLevelData(Level level)
        {
            string fileName = $"{FolderPath}{level.FileName}_gen.inc.s";

            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                switch (ExportSettings.Format)
                {
                    case ExportSettings.ExportFormat.Screens:
                        break;
                    case ExportSettings.ExportFormat.Columns:
                        WriteLevelColumnRefs(level, outputFile);
                        break;
                    case ExportSettings.ExportFormat.Rows:
                        WriteLevelRowRefs(level, outputFile);
                        break;
                    case ExportSettings.ExportFormat.Map:
                        break;
                    default:
                        break;
                }
                if (ExportSettings.SaveAttributes)
                {
                    WriteLevelAttrRefs(level, outputFile);
                }
                WriteLevelObjectRefs(level, outputFile);
                WriteLevelEntryRefs(level, outputFile);
                WriteLevelInitStruct(level, outputFile);
            }
        }

        private void ExportAttributes(Level level)
        {
            throw new NotImplementedException();
        }

        private void ExportLevelTiles(Level level)
        {
            switch (ExportSettings.Format)
            {
                case ExportSettings.ExportFormat.Screens:
                    ExportByScreens(level);
                    break;
                case ExportSettings.ExportFormat.Columns:
                    ExportByColumns(level);
                    break;
                case ExportSettings.ExportFormat.Rows:
                    ExportByRows(level);
                    break;
                case ExportSettings.ExportFormat.Map:
                    ExportAsMap(level);
                    break;
                default:
                    break;
            }
        }

        private void ExportAsMap(Level level)
        {
            throw new NotImplementedException();
        }

        private void ExportByRows(Level level)
        {
            for (int i = 0; i < level.ScreenMetatiles[0].Count; i++)
            {
                ExportMetatileRow(level.ScreenMetatiles, i, $"{FolderPath}{level.FileName}_row_{i}.bin");
            }
        }

        private void ExportByColumns(Level level)
        {
            for (int i = 0; i < level.ScreenMetatiles.Count; i++)
            {
                ExportMetatileColumn(level.ScreenMetatiles, i, $"{FolderPath}{level.FileName}_col_{i}.bin");
            }
        }

        private void ExportMetatileRow(List<List<ScreenMetatile>> screenMetatiles, int row, string fileName)
        {
            byte[] metatileBytes = new byte[screenMetatiles.Count];
            for (int i = 0; i < screenMetatiles.Count; i++)
            {
                metatileBytes[i] = (byte)screenMetatiles[i][row].mi;
            }
            File.WriteAllBytes(fileName, metatileBytes);
        }

        private void ExportMetatileColumn(List<List<ScreenMetatile>> screenMetatiles, int column, string fileName)
        {
            byte[] metatileBytes = new byte[screenMetatiles.Count];
            for (int i = 0; i < screenMetatiles[column].Count; i++)
            {
                metatileBytes[i] = (byte)screenMetatiles[column][i].mi;
            }
            File.WriteAllBytes(fileName, metatileBytes);
        }

        private void ExportByScreens(Level level)
        {
            throw new NotImplementedException();
        }

        private void WriteLevelColumnRefs(Level level, StreamWriter outputFile)
        {
            string level_columns = level.FileName + "_columns";

            outputFile.Write($".define {level_columns}");
            for (int i = 0; i < level.ScreenMetatiles.Count; i++)
            {
                outputFile.Write($" {level.FileName}_col_{i}");
                if (i < level.ScreenMetatiles.Count - 1)
                {
                    outputFile.Write($",");
                }
            }
            outputFile.WriteLine();
            outputFile.WriteLine();

            outputFile.WriteLine($"{level_columns}_l:");
            outputFile.WriteLine($".lobytes {level_columns}");
            outputFile.WriteLine($"{level_columns}_h:");
            outputFile.WriteLine($".hibytes {level_columns}");

            for (int i = 0; i < level.ScreenMetatiles.Count; i++)
            {
                outputFile.WriteLine($"{level.FileName}_col_{i}:");
                outputFile.WriteLine($".incbin \"{level.FileName}_col_{i}.bin\"");
            }
            outputFile.WriteLine();

        }

        private void WriteLevelRowRefs(Level level, StreamWriter outputFile)
        {
            throw new NotImplementedException();
        }

        private void WriteLevelAttrRefs(Level level, StreamWriter outputFile)
        {
            throw new NotImplementedException();
        }

        private void WriteLevelObjectRefs(Level level, StreamWriter outputFile)
        {
            List<LevelObject> exportObjects = level.Objects.OrderBy(obj => obj.Column).ToList();

            outputFile.WriteLine($".byte $00, $00, $00, $00, $00");
            outputFile.WriteLine($"{level.FileName}_objects:");
            foreach (var obj in exportObjects)
            {
                if (obj.Type == LevelObject.ObjectTypes.Small)
                {
                    outputFile.Write("SMALL_");
                }
                outputFile.Write("LEVEL_OBJ ");

                outputFile.Write($"${obj.Column:X2}, ${obj.Row:X2}, init_{obj.Name.ToLower()}, ${obj.Var:X2}");
                outputFile.WriteLine();
            }
            outputFile.WriteLine($".byte $00");
            outputFile.WriteLine();
        }

        private void WriteLevelEntryRefs(Level level, StreamWriter outputFile)
        {
            outputFile.WriteLine($"{level.FileName}_entries_x:");
            foreach (var entry in level.Entries)
            {
                outputFile.WriteLine($"\t.byte ${entry.PosX:X2}");
            }
            outputFile.WriteLine();

            outputFile.WriteLine($"{level.FileName}_entries_y:");
            foreach (var entry in level.Entries)
            {
                outputFile.WriteLine($"\t.byte ${entry.PosY:X2}");
            }
            outputFile.WriteLine();

            outputFile.WriteLine($"{level.FileName}_exits_dest_level:");
            foreach (var exit in level.Exits)
            {
                outputFile.WriteLine($"\t.byte ${exit.DestLevel:X2}");
            }
            outputFile.WriteLine();

            outputFile.WriteLine($"{level.FileName}_exits_dest_entry:");
            foreach (var exit in level.Exits)
            {
                outputFile.WriteLine($"\t.byte ${exit.DestEntry:X2}");
            }
            outputFile.WriteLine();

            outputFile.WriteLine($"{level.FileName}_exits_transition_types:");
            foreach (var exit in level.Exits)
            {
                outputFile.WriteLine($"\t.byte ${(int)exit.TransitionType:X2}");
            }
            outputFile.WriteLine();
        }

        private void WriteLevelInitStruct(Level level, StreamWriter outputFile)
        {
            outputFile.WriteLine($".export {level.FileName}_init_struct");
            outputFile.WriteLine($"{level.FileName}_init_struct:");
            outputFile.WriteLine($".byte {level.ScreenMetatiles.Count}");
            outputFile.Write($".addr {level.FileName}_columns_l, {level.FileName}_columns_h, ");
            if (ExportSettings.SaveAttributes)
            {
                outputFile.Write($"{level.FileName}_attribute_screens_l, {level.FileName}_attribute_screens_h, ");
            }
            outputFile.Write($"{level.MetatileSet.FileName}_0, {level.MetatileSet.FileName}_1, {level.MetatileSet.FileName}_2, {level.MetatileSet.FileName}_3, {level.MetatileSet.FileName}_collision, {level.FileName}_objects, {level.FileName}_entries_x, {level.FileName}_entries_y, {level.FileName}_exits_dest_level, {level.FileName}_exits_dest_entry, {level.FileName}_exits_transition_types");
            outputFile.WriteLine();
            outputFile.WriteLine();
        }
    }
}
