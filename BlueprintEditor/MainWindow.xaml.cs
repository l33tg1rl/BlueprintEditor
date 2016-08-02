using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace BlueprintEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainViewport3DOnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var removeVoxel = Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);
            var viewModel = MainViewport3D.DataContext as MainViewModel;

            if (viewModel != null && MainViewport3D.CursorOnElementPosition.HasValue)
            {
                if (removeVoxel)
                    viewModel.RemoveVoxel(MainViewport3D.Camera.Position, MainViewport3D.CursorOnElementPosition.Value);
                else
                    viewModel.AddVoxel(MainViewport3D.Camera.Position, MainViewport3D.CursorOnElementPosition.Value);
            }
        }
    }
}