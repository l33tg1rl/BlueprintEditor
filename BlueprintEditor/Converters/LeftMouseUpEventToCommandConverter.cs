using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using HelixToolkit.Wpf;

namespace BlueprintEditor.Converters
{
    public class LeftMouseUpEventToCommandConverter : IEventArgsConverter
    {
        public object Convert(object value, object parameter)
        {
            return  parameter as HelixViewport3D;
        }
    }
}