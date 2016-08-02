using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace BlueprintEditor.Controls
{
    public class ModelVisual3DContainer : ModelVisual3D
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(ObservableCollection<BoxVisual3D>),
            typeof(ModelVisual3DContainer),
            new PropertyMetadata(new ObservableCollection<BoxVisual3D>(), (d, e) => ((ModelVisual3DContainer)d).ItemsSourcePropertyChangedCallback(e)));

        public ObservableCollection<BoxVisual3D> ItemsSource
        {
            get
            {
                return (ObservableCollection<BoxVisual3D>)GetValue(ItemsSourceProperty);
            }

            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        private void ItemsSourcePropertyChangedCallback(DependencyPropertyChangedEventArgs args)
        {
            if (args.OldValue != null)
            {
                var items = (ObservableCollection<BoxVisual3D>)args.OldValue;
                foreach (var item in items)
                {
                    Children.Remove(item);
                }
                items.CollectionChanged -= ItemsSourceCollectionChanged;
            }

            if (args.NewValue != null)
            {
                var items = (ObservableCollection<BoxVisual3D>)args.NewValue;
                foreach (var item in items)
                {
                    Children.Add(item);
                }
                items.CollectionChanged += ItemsSourceCollectionChanged;
            }
        }

        private void ItemsSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
        {
            if (args.NewItems != null)
            {
                foreach (BoxVisual3D newItem in args.NewItems)
                {
                    Children.Add(newItem);
                }
            }

            if (args.OldItems != null)
            {
                foreach (BoxVisual3D oldItem in args.OldItems)
                {
                    Children.Remove(oldItem);
                }
            }
        }
    }
}
