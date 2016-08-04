using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Serialization;
using BlueprintEditor.Model;
using GalaSoft.MvvmLight.CommandWpf;
using HelixToolkit.Wpf;

namespace BlueprintEditor
{
    public class MainViewModel : ObservableObject
    {
        private ObservableCollection<BoxVisual3D> _voxels;
        private Point3D _cursorPosition;
        private Point3D _cameraPosition;
        private Color _selectedColor;
        private const double Threshold = .0000000000000000001;

        public ObservableCollection<BoxVisual3D> Voxels
        {
            get { return _voxels; }
            set
            {
                if (value == _voxels) return;
                _voxels = value;
                OnPropertyChanged();
            }
        }

        public Point3D CursorPosition
        {
            get { return _cursorPosition; }
            set
            {
                if (_cursorPosition == value) return;
                _cursorPosition = value;
                OnPropertyChanged();
            }
        }

        public Point3D CameraPosition
        {
            get { return _cameraPosition; }
            set
            {
                if (_cameraPosition == value) return;
                _cameraPosition = value;
                OnPropertyChanged();
            }
        }

        public Color SelectedColor
        {
            get { return _selectedColor; }
            set
            {
                if (_selectedColor == value) return;
                _selectedColor = value;
                OnPropertyChanged();
            }
        }

        public ICommand NewBlueprintCommand { get; private set; }
        public ICommand SaveBlueprintCommand { get; private set; }
        public ICommand OpenBlueprintCommand { get; private set; }

        public MainViewModel()
        {
            RegisterCommands();
            Voxels = new ObservableCollection<BoxVisual3D>();
            SelectedColor = Colors.Red;
        }

        private void RegisterCommands()
        {
            NewBlueprintCommand = new RelayCommand(NewBlueprint);
            SaveBlueprintCommand = new RelayCommand<Tuple<Action<Blueprint, string>, string>>(SaveBlueprint);
            OpenBlueprintCommand = new RelayCommand<Tuple<Func<string, Blueprint>, string>>(OpenBlueprint);
        }

        private void NewBlueprint()
        {
            Voxels = new ObservableCollection<BoxVisual3D> { new BoxVisual3D { Center = new Point3D(0, 0, 0), Fill = new SolidColorBrush(SelectedColor)} };
        }

        private void SaveBlueprint(Tuple<Action<Blueprint, string>, string> tuple)
        {
            Blueprint blueprint = BuildBlueprint();
            tuple.Item1.Invoke(blueprint, tuple.Item2);
        }

        public void OpenBlueprint(Tuple<Func<string, Blueprint>, string> tuple)
        {
            Blueprint blueprint = tuple.Item1.Invoke(tuple.Item2);
            Voxels = new ObservableCollection<BoxVisual3D>();

            foreach (Voxel voxel in blueprint.Voxels)
            {
                Voxels.Add(new BoxVisual3D
                {
                    Center = new Point3D(Convert.ToDouble(voxel.XCoord), Convert.ToDouble(voxel.YCoord), Convert.ToDouble(voxel.ZCoord)),
                    Fill = new SolidColorBrush(voxel.VectorColor)
                });
            }
        }

        private Blueprint BuildBlueprint()
        {
            var blueprint = new Blueprint();
            blueprint.Voxels = new List<Voxel>();

            foreach (var item in Voxels)
            {
                blueprint.Voxels.Add(new Voxel
                {
                    XCoord = Convert.ToDecimal(item.Center.X),
                    YCoord = Convert.ToDecimal(item.Center.Y),
                    ZCoord = Convert.ToDecimal(item.Center.Z),
                    VectorColor = ((SolidColorBrush)item.Fill).Color
                });
            }

            return blueprint;
        }

        private void SaveBlueprint(StreamWriter writer)
        {
            Blueprint blueprint = BuildBlueprint();
            XmlSerializer serializer = new XmlSerializer(typeof(Blueprint));
            serializer.Serialize(writer, blueprint);
        }

        public void OpenBlueprint(StreamReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Blueprint));
            Blueprint blueprint = (Blueprint) serializer.Deserialize(reader);
            Voxels = new ObservableCollection<BoxVisual3D>();

            foreach (Voxel voxel in blueprint.Voxels)
            {
                Voxels.Add(new BoxVisual3D
                {
                    Center = new Point3D(Convert.ToDouble(voxel.XCoord), Convert.ToDouble(voxel.YCoord), Convert.ToDouble(voxel.ZCoord)), 
                    Fill = new SolidColorBrush(voxel.VectorColor)
                });
            }
        }

        public void AddVoxel(Point3D cameraPosition, Point3D cursorPosition)
        {
            double x, y, z, offset;
            Point3D voxelPoint;

            if (Math.Abs(Math.IEEERemainder(cursorPosition.X, .5)) < Threshold) //X Plane
            {
                x = (cameraPosition.X > cursorPosition.X ? -1 : 1) * .5 + cursorPosition.X;
                y = Math.Round(cursorPosition.Y);
                z = Math.Round(cursorPosition.Z);
                offset = cameraPosition.X > 0 ? 1 : -1;
                voxelPoint = new Point3D(x + offset, y, z);
            }
            else if (Math.Abs(Math.IEEERemainder(cursorPosition.Y, .5)) < Threshold) //Y Plane
            {
                x = Math.Round(cursorPosition.X);
                y = (cameraPosition.Y > cursorPosition.Y ? -1 : 1) * .5 + cursorPosition.Y;
                z = Math.Round(cursorPosition.Z);
                offset = cameraPosition.Y > 0 ? 1 : -1;
                voxelPoint = new Point3D(x, y + offset, z);
            }
            else if (Math.Abs(Math.IEEERemainder(cursorPosition.Z, .5)) < Threshold) //Z Plane
            {
                x = Math.Round(cursorPosition.X);
                y = Math.Round(cursorPosition.Y);
                z = (cameraPosition.Z > cursorPosition.Z ? -1 : 1) * .5 + cursorPosition.Z;
                offset = cameraPosition.Z > 0 ? 1 : -1;
                voxelPoint = new Point3D(x, y, z + offset);
            }
            else
            {
                return;
            }

            if(Voxels.All(v => v.Center != voxelPoint))
                Voxels.Add(new BoxVisual3D { Center = voxelPoint, Fill = new SolidColorBrush(SelectedColor)});
        }

        public void RemoveVoxel(Point3D cameraPosition, Point3D cursorPosition)
        {
            double x, y, z;

            if (Math.Abs(Math.IEEERemainder(cursorPosition.X, .5)) < Threshold)
            {
                x = (cameraPosition.X > cursorPosition.X ? -1 : 1) * .5 + cursorPosition.X;
                y = Math.Round(cursorPosition.Y);
                z = Math.Round(cursorPosition.Z);
            }
            else if (Math.Abs(Math.IEEERemainder(cursorPosition.Y, .5)) < Threshold)
            {
                x = Math.Round(cursorPosition.X);
                y = (cameraPosition.Y > cursorPosition.Y ? -1 : 1) * .5 + cursorPosition.Y;
                z = Math.Round(cursorPosition.Z);
            }
            else if (Math.Abs(Math.IEEERemainder(cursorPosition.Z, .5)) < Threshold)
            {
                x = Math.Round(cursorPosition.X);
                y = Math.Round(cursorPosition.Y);
                z = (cameraPosition.Z > cursorPosition.Z ? -1 : 1) * .5 + cursorPosition.Z;
            }
            else
            {
                return;
            }

            //Don't remove center voxel
            if (Math.Abs(x) < Threshold && Math.Abs(y) < Threshold && Math.Abs(z) < Threshold) return;

            var voxel = Voxels.FirstOrDefault(v => v.Center == new Point3D(x, y, z));

            if (voxel != null)
                Voxels.Remove(voxel);
        }

        //private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        //{
        //    //if(propertyChangedEventArgs.PropertyName == "CameraCenter")
        //}
    }
}