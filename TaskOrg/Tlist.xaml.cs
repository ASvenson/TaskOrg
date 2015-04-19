using TaskOrg.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace TaskOrg
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Tlist : Page
    {
        private TaskList currTaskList;
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        public Tlist()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            addTask();
        }

        public void addTask()
        {
            task newTask = new task();
            Button newTBox = new Button() { Height = 100, Width = 200, Content = newTask.Title, Tag = newTask};
            newTBox.Click += Task_Click;
            newTBox.IsHoldingEnabled = true;
            newTBox.Holding+=newTBox_Holding;
            newTBox.Background = new Windows.UI.Xaml.Media.SolidColorBrush()
            {
                Color = Windows.UI.Color.FromArgb(255, 255, 0, 0)
            };
            currTaskList.Tasks.Add(newTask);
            TaskStack.Children.Add(newTBox);
            Grid.Height = TaskStack.Height;
        }
        public void addTask(task currTask)
        {
            Button newTBox = new Button() { Height = 100, Width = 200, Content = currTask.Title, Tag = currTask};
            newTBox.Click += Task_Click;
            newTBox.IsHoldingEnabled = true;
            newTBox.Holding += newTBox_Holding;
            newTBox.Background = new Windows.UI.Xaml.Media.SolidColorBrush()
            {
                Color = Windows.UI.Color.FromArgb(255, 255, 0, 0)
            };
            TaskStack.Children.Add(newTBox);
            Grid.Height = TaskStack.Height;
        }

        private void newTBox_Holding(object sender, HoldingRoutedEventArgs e)
        {
            currTaskList.Tasks.Remove(((task)((Button)sender).Tag));
            ((Button)sender).Visibility = Visibility.Collapsed;
            
            
        }

        private void refresh(LoadStateEventArgs e)
        {
            currTaskList = ((TaskList)e.NavigationParameter);
            TaskStack.Children.Clear();
            int num = currTaskList.Tasks.Count;
            TitleBox.Text = currTaskList.Title;
            for (int i = 0; i < num; i++)
            {
                addTask(currTaskList.Tasks[i]);
            }
        }

        private void refresh()
        {
            TaskStack.Children.Clear();
            int num = currTaskList.Tasks.Count;
            TitleBox.Text = currTaskList.Title;
            for (int i = 0; i < num; i++)
            {
                addTask(currTaskList.Tasks[i]);
            }

        }

        public void SetTitle(object sender, TextChangedEventArgs e)
        {
            currTaskList.Title = TitleBox.Text;
        }

        

        private async void Task_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(TPage), ((Button)sender).Tag));
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            refresh(e);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Scroller_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {

        }
    }
}
