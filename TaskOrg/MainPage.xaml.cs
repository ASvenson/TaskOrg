using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TaskOrg
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        List<TaskList> listArray;
        int NewButtonID;
        
        public MainPage()
        {
            this.InitializeComponent();
            NewButtonID = 0;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            listArray = new List<TaskList>();

            for (int i = 0; i < 3; i++)
            {

                addList();

            }
            
        }
        public void addList()
        {
            int ButtonID = NewButtonID;
            NewButtonID++;
            TaskList temp = new TaskList();
            Button newBox = new Button() { Height = 100, Width = 200, Content = temp.Title+ButtonID, Tag=temp};
            newBox.Click += Box_Click;
            newBox.Background = new Windows.UI.Xaml.Media.SolidColorBrush()
            {
                Color = Windows.UI.Color.FromArgb(255, 255, 0, 0)
            };
            listArray.Add(temp);
            TaskStack.Children.Add(newBox);
            Grid.Height = TaskStack.Height;
            //Tnum++;
        }
        private async void Box_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(Tlist), ((Button)sender).Tag));
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // listArray= new List<TaskList>();

            
            

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            addList();
        }

        
    }
}
