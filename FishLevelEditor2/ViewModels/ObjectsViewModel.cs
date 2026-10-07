using Avalonia.Controls;
using Avalonia.Media.Imaging;
using FishLevelEditor2.Logic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class ObjectsViewModel : ViewModelBase
    {
        public ObservableCollection<LevelObjectDefinition> ObjectDefinitions { get; set; }

        public ObjectsViewModel(List<LevelObjectDefinition> objects)
        {
            ObjectDefinitions = new ObservableCollection<LevelObjectDefinition>(objects);
        }
    }
}
