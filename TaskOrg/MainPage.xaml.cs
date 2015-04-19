using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
        static List<TaskList> listArray;
        int NewButtonID;


        
        StorageFolder localFolder = null;
        int localCounter = 0;
        StorageFolder localCacheFolder = null;
        int localCacheCounter = 0;
        StorageFolder roamingFolder = null;
        int roamingCounter = 0;
        StorageFolder temporaryFolder = null;
        int temporaryCounter = 0;

        const string filename = "data.txt";

        public MainPage()
        {

            listArray = new List<TaskList>();
           // createFile();
            

            this.InitializeComponent();
         
            NewButtonID = 0;
            this.NavigationCacheMode = NavigationCacheMode.Required;

            

            
           


            localFolder = ApplicationData.Current.LocalFolder;
            localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            roamingFolder = ApplicationData.Current.RoamingFolder;
            temporaryFolder = ApplicationData.Current.TemporaryFolder;


        }
        public async void createFile()
        {
            StorageFile file = await localFolder.CreateFileAsync(filename);
            IList<string> text= new List<string>();
            text.Add("hello");
            await FileIO.WriteLinesAsync(file, text);

        }
        public void addList()
        {
            // int ButtonID = NewButtonID;
            // NewButtonID++;
            TaskList temp = new TaskList();
            Button newBox = new Button() { Height = 100, Width = 200, Content = temp.Title, Tag = temp };
            newBox.Click += Box_Click;
            newBox.IsHoldingEnabled = true;
            newBox.Holding += newBox_Holding;
            newBox.Background = new Windows.UI.Xaml.Media.SolidColorBrush()
            {
                Color = Windows.UI.Color.FromArgb(255, 255, 0, 255)
            };
            listArray.Add(temp);
            TaskStack.Children.Add(newBox);
            Grid.Height = TaskStack.Height;
        }

        private void newBox_Holding(object sender, HoldingRoutedEventArgs e)
        {
            listArray.Remove(((TaskList)((Button)sender).Tag));
            refresh();
        }

       

        public void addList(TaskList temp)
        {
            Button newBox = new Button() { Height = 100, Width = 200, Content = temp.Title, Tag = temp };
            newBox.Click += Box_Click;
            newBox.IsHoldingEnabled = true;
            newBox.Holding += newBox_Holding;
            newBox.Background = new Windows.UI.Xaml.Media.SolidColorBrush()
            {
                Color = Windows.UI.Color.FromArgb(255, 255, 0, 255)
            };
            TaskStack.Children.Add(newBox);
            Grid.Height = TaskStack.Height;
        }
        private async void Box_Click(object sender, RoutedEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () => Frame.Navigate(typeof(Tlist), ((Button)sender).Tag));
        }

        private void refresh()
        {
            TaskStack.Children.Clear();
            //loadOld();
            int count = listArray.Count;
            for (int i = 0; i < count; i++)
            {
                addList(listArray[i]);
            }
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

            refresh();




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

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            saveCurrent();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }


        public async void loadOld()
        {
            //StorageFolder local = ApplicationData.Current.LocalFolder;
            StorageFile file;
            try
            {
                file = await localFolder.GetFileAsync(filename);

                List<TaskList> temp = new List<TaskList>();
                IList<string> text = await FileIO.ReadLinesAsync(file);
                for (int j = 0; j < text.Count; j++)
                {
                    string input = text[j];
                    String[] tokens = input.Split('|');

                    TaskList tempList = new TaskList();
                    List<task> tasks = new List<task>();
                    for (int i = 0; i < tokens.Count(); i++)
                    {
                        if (i == 0)
                        {
                            tempList.Title = tokens[i];
                        }
                        else
                        {
                            String[] taskTokens = tokens[i].Split(',');
                            task newTask = new task();
                            newTask.Title = taskTokens[0];
                            newTask.Description = taskTokens[1];
                            tasks.Add(newTask);
                        }
                    }
                    tempList.Tasks = tasks;
                    temp.Add(tempList);
                }
                listArray = temp;
            }

            catch (FileNotFoundException)
            {
               createFile();
               loadOld();
            }
        }

            
        /// <summary>
        /// Writes the content of the open flashcards to a file called name.txt
        /// </summary>
        /// <param name="name">The file name to use, will add a .txt to end of string</param>
        public async void saveCurrent()
        {
            //StorageFolder local = ApplicationData.Current.LocalFolder;
            try
            {
                List<TaskList> temp = new List<TaskList>();
                StorageFile file = await localFolder.GetFileAsync(filename);
                
                IList<string> lines = new List<string>();

                int size = listArray.Count;
                for (int j = 0; j < size; j++)
                {
                    TaskList tempList = listArray[j];
                    string line = tempList.Title;
                    List<task> tList = tempList.Tasks;
                    
                    for (int i = 0; i < tList.Count(); i++)
                    {
                        task tempTask = tList[i];
                        line = line + "|" + tempTask.Title + "," + tempTask.Description;
                    }
                    lines.Add(line);
                }
                await FileIO.WriteLinesAsync(file, lines);

            }
            catch
            {

            }

        }

    }
}
