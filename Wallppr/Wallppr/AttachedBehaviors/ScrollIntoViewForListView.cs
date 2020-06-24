using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Wallppr.AttachedBehaviors
{
    public class ScrollIntoViewForListViewBehavior : Behavior<ListView>
    {
        /// <summary>
        ///  When Beahvior is attached
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        /// <summary>
        /// On Selection Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListView)
            {
                ListView listBox = sender as ListView;
                if (listBox.SelectedItem != null)
                {
                    listBox.Dispatcher.BeginInvoke((Action)(() =>
                    {
                        listBox.UpdateLayout();
                        if (listBox.SelectedItem != null)
                        {
                            listBox.ScrollIntoView(listBox.SelectedItem);
                        }
                    }));
                }
            }
        }
        /// <summary>
        /// When behavior is detached
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }
    }
}
