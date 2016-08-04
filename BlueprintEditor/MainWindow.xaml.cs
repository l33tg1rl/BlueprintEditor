using System;
using System.Windows;
using System.Windows.Input;
using BlueprintEditor.Model;
using BlueprintEditor.Utilities;
using Microsoft.Win32;

namespace BlueprintEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string BlueprintFileFilters = "xml|*.xml";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainViewport3DOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var removeVoxel = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            var viewModel = MainViewport3D.DataContext as MainViewModel;

            if (viewModel == null || !MainViewport3D.CursorOnElementPosition.HasValue) return;

            if (removeVoxel)
                viewModel.RemoveVoxel(MainViewport3D.Camera.Position, MainViewport3D.CursorOnElementPosition.Value);
            else
                viewModel.AddVoxel(MainViewport3D.Camera.Position, MainViewport3D.CursorOnElementPosition.Value);
        }

        private void SaveButtonOnClick(object sender, RoutedEventArgs e)
        {
            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = BlueprintFileFilters;

            if (saveDialog.ShowDialog() != true) return;

            var viewModel = MainViewport3D.DataContext as MainViewModel;

            if (viewModel == null) return;

            viewModel.SaveBlueprintCommand.Execute(new Tuple<Action<Blueprint, string>, string>(XmlUtilities.SerializeToFile, saveDialog.FileName));
        }

        private void OpenButtonOnClick(object sender, RoutedEventArgs e)
        {
            var openDialog = new OpenFileDialog();
            openDialog.Filter = BlueprintFileFilters;

            if (openDialog.ShowDialog(this) != true) return;

            var viewModel = MainViewport3D.DataContext as MainViewModel;

            if (viewModel == null) return;

            viewModel.OpenBlueprintCommand.Execute(new Tuple<Func<string, Blueprint>, string>(XmlUtilities.DeserializeFromFile<Blueprint>, openDialog.FileName));
        }
    }
}