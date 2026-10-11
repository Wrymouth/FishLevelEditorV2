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
                WriteLevelPaletteRefs(level, outputFile);
                WriteLevelEntryRefs(level, outputFile);
                WriteLevelInitStruct(level, outputFile);
            }
        }

        private void ExportAttributes(Level level)
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

        private void ExportLevelTiles(Level level)
        {
            switch (ExportSettings.Format)
            {
                case ExportSettings.ExportFormat.Screens:
                    ExportAttrByScreens(level);
                    break;
                case ExportSettings.ExportFormat.Columns:
                    ExportAttrByColumns(level);
                    break;
                case ExportSettings.ExportFormat.Rows:
                    ExportAttrByRows(level);
                    break;
                case ExportSettings.ExportFormat.Map:
                    ExportAttrAsMap(level);
                    break;
                default:
                    break;
            }
        }

        private void ExportAttrAsMap(Level level)
        {
            throw new NotImplementedException();
        }

        private void ExportAttrByRows(Level level)
        {
            for (int i = 0; i < level.ScreenMetatiles[0].Count; i += 2)
            {
                ExportAttrRow(level.ScreenMetatiles, i, $"{FolderPath}{level.FileName}_attr_row_{i / 2}.bin");
            }
        }

        private void ExportAttrByColumns(Level level)
        {
            for (int i = 0; i < level.ScreenMetatiles.Count; i += 2)
            {
                ExportAttrColumn(level.ScreenMetatiles, i, $"{FolderPath}{level.FileName}_attr_col_{i / 2}.bin");
            }
        }

        private void ExportAttrByScreens(Level level)
        {
            throw new NotImplementedException();
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

        private void ExportAttrRow(List<List<ScreenMetatile>> screenMetatiles, int metatileRow, string fileName)
        {
            byte[] attrBytes = new byte[screenMetatiles.Count / 2];
            for (int i = 0; i < screenMetatiles.Count; i += 2)
            {
                uint topLeft = screenMetatiles[i][metatileRow].pi;
                uint topRight = screenMetatiles[i+1][metatileRow].pi;
                uint bottomLeft = screenMetatiles[i][metatileRow+1].pi;
                uint bottomRight = screenMetatiles[i+1][metatileRow+1].pi;

                uint attr = (bottomRight << 6) | (bottomLeft << 4) | (topRight << 2) | (topLeft << 0);
                attrBytes[i / 2] = (byte)attr;
            }
            File.WriteAllBytes(fileName, attrBytes);
        }

        private void ExportAttrColumn(List<List<ScreenMetatile>> screenMetatiles, int metatileColumn, string fileName)
        {
            byte[] attrBytes = new byte[screenMetatiles[metatileColumn].Count / 2];
            for (int i = 0; i < screenMetatiles[metatileColumn].Count; i += 2)
            {
                uint topLeft, topRight, bottomLeft, bottomRight = uint.MaxValue;

                topLeft = screenMetatiles[metatileColumn][i].pi;
                
                if (metatileColumn + 1 >= screenMetatiles.Count)
                {
                    topRight = 0;
                    bottomRight = 0;
                }
                else
                {
                    topRight = screenMetatiles[metatileColumn + 1][i].pi;
                }
                
                if (i + 1 >= screenMetatiles[metatileColumn].Count)
                {
                    bottomLeft = 0;
                    bottomRight = 0;
                }
                else
                {
                    bottomLeft = screenMetatiles[metatileColumn][i + 1].pi;
                }

                if (bottomRight == uint.MaxValue)
                {
                    bottomRight = screenMetatiles[metatileColumn + 1][i + 1].pi;
                }

                uint attr = (bottomRight << 6) | (bottomLeft << 4) | (topRight << 2) | (topLeft << 0);
                attrBytes[i / 2] = (byte)attr;
            }
            File.WriteAllBytes(fileName, attrBytes);
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
            byte[] metatileBytes = new byte[screenMetatiles[column].Count];
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
            string level_attr_columns = level.FileName + "_attr_columns";

            outputFile.Write($".define {level_attr_columns}");
            for (int i = 0; i < level.ScreenMetatiles.Count; i += 2)
            {
                outputFile.Write($" {level.FileName}_attr_col_{i/2}");
                if (i < level.ScreenMetatiles.Count - 2)
                {
                    outputFile.Write($",");
                }
            }
            outputFile.WriteLine();
            outputFile.WriteLine();

            outputFile.WriteLine($"{level_attr_columns}_l:");
            outputFile.WriteLine($".lobytes {level_attr_columns}");
            outputFile.WriteLine($"{level_attr_columns}_h:");
            outputFile.WriteLine($".hibytes {level_attr_columns}");

            for (int i = 0; i < level.ScreenMetatiles.Count; i += 2)
            {
                outputFile.WriteLine($"{level.FileName}_attr_col_{i/2}:");
                outputFile.WriteLine($".incbin \"{level.FileName}_attr_col_{i/2}.bin\"");
            }
            outputFile.WriteLine();
        }

        private void WriteLevelObjectRefs(Level level, StreamWriter outputFile)
        {
            List<LevelObject> exportObjects = level.Objects.OrderBy(obj => obj.PosX).ToList();

            outputFile.WriteLine($".byte $00, $00, $00, $00, $00");
            outputFile.WriteLine($"{level.FileName}_objects:");
            foreach (var obj in exportObjects)
            {
                if (obj.Definition.Type == LevelObjectDefinition.ObjectTypes.Small)
                {
                    outputFile.Write("SMALL_");
                }
                outputFile.Write("LEVEL_OBJ ");

                int varValue;
                if (!int.TryParse(obj.VarValue, out varValue))
                {
                    varValue = 0;
                }

                outputFile.Write($"${obj.PosX:X2}, ${obj.PosY:X2}, init_{obj.Definition.FileName}, ${varValue:X2}");
                outputFile.WriteLine();
            }
            outputFile.WriteLine($".byte $00");
            outputFile.WriteLine();
        }

        private void WriteLevelPaletteRefs(Level level, StreamWriter outputFile)
        {
            outputFile.WriteLine($"{level.FileName}_palettes:");
            foreach (var palette in level.BackgroundPalettes)
            {
                outputFile.WriteLine($".byte ${palette.Colors[0]:X2}, ${palette.Colors[1]:X2}, ${palette.Colors[2]:X2}, ${palette.Colors[3]:X2}");
            }
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
                outputFile.Write($"{level.FileName}_attr_columns_l, {level.FileName}_attr_columns_h, ");
            }
            outputFile.Write($"{level.MetatileSet.FileName}_0, {level.MetatileSet.FileName}_1, {level.MetatileSet.FileName}_2, {level.MetatileSet.FileName}_3, {level.MetatileSet.FileName}_collision, {level.FileName}_objects, {level.FileName}_entries_x, {level.FileName}_entries_y, {level.FileName}_exits_dest_entry, {level.FileName}_exits_dest_level, {level.FileName}_exits_transition_types, {level.FileName}_palettes");
            outputFile.WriteLine();
            outputFile.WriteLine();
        }
    }
}
