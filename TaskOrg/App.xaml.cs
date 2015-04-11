using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;


// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace TaskOrg
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public sealed partial class App : Application
    {
        private TransitionCollection transitions;
        StorageFolder localFolder = null;
        int localCounter = 0;
        StorageFolder localCacheFolder = null;
        int localCacheCounter = 0;
        StorageFolder roamingFolder = null;
        int roamingCounter = 0;
        StorageFolder temporaryFolder = null;
        int temporaryCounter = 0;

        const string filename = "data.txt";

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += this.OnSuspending;
            localFolder = ApplicationData.Current.LocalFolder;
            localCacheFolder = ApplicationData.Current.LocalCacheFolder;
            roamingFolder = ApplicationData.Current.RoamingFolder;
            temporaryFolder = ApplicationData.Current.TemporaryFolder;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used when the application is launched to open a specific file, to display
        /// search results, and so forth.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                this.DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                // TODO: change this value to a cache size that is appropriate for your application
                rootFrame.CacheSize = 1;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null)
                {
                    this.transitions = new TransitionCollection();
                    foreach (var c in rootFrame.ContentTransitions)
                    {
                        this.transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += this.RootFrame_FirstNavigated;

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof(MainPage), e.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = this.transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= this.RootFrame_FirstNavigated;
           
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

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        async void Increment_Local_Click(Object sender, RoutedEventArgs e)
        {
            localCounter++;

            StorageFile file = await localFolder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, localCounter.ToString());



            Read_Local_Counter();
        }

        async void Read_Local_Counter()
        {
            try
            {
                List<TaskList> temp = new List<TaskList>();
                StorageFile file = await localFolder.GetFileAsync(filename);
                IList<string> text = await FileIO.ReadLinesAsync(file);
                for(int j = 0; j<text.Count;j++)
                {
                    string input = text[j];
                    String[] tokens = input.Split('|');
                    for (int i = 0; i < tokens.Count(); i++)
                    {
                        TaskList tempList = new TaskList();
                        List<task> tasks = new List<task>();
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
                }
                listArray = temp;
            }
            catch (Exception)
            {
                
            }
        }

        public static void loadOld()
        {
            StorageFolder local = ApplicationData.Current.LocalFolder;

            string name = "data.txt";
            StorageFile manifestFile = (StorageFile)(local.GetFileAsync(name));
            

            

            // Specify the file path and options.
            using (var isoFileStream =
                    new System.IO.IsolatedStorage.IsolatedStorageFileStream
                        ("DataFolder\\data.txt", System.IO.FileMode.Open, local))
            {
                // Read the data.
                using (var isoFileReader = new System.IO.StreamReader(isoFileStream))
                {
                    while (!isoFileReader.EndOfStream)
                    {
                        String input = isoFileReader.ReadLine();
                        String[] tokens = input.Split('|');
                        for(int i = 0; i < tokens.Count(); i++)
                        {
                            TaskList tempList = new TaskList();
                            List<task> tasks= new List<task>();
                            if(i == 0)
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
                    }
                }
            }

            listArray = temp;
        }
        /// <summary>
        /// Writes the content of the open flashcards to a file called name.txt
        /// </summary>
        /// <param name="name">The file name to use, will add a .txt to end of string</param>
        public static void saveCurrent()
        {
            System.IO.IsolatedStorage.IsolatedStorageFile local =
                System.IO.IsolatedStorage.IsolatedStorageFile.GetUserStoreForApplication();

            if (!local.DirectoryExists("DataFolder"))
                local.CreateDirectory("DataFolder");

            using (var isoFileStream =
            new System.IO.IsolatedStorage.IsolatedStorageFileStream(
                "DataFolder\\data.txt",
                System.IO.FileMode.Create,
                    local))
            {
                // Write the data from the textbox.
                using (var isoFileWriter = new System.IO.StreamWriter(isoFileStream))
                {
                    int size = listArray.Count();
                    for (int i = 0; i < size; i++)
                    {
                        TaskList currList = listArray[i];
                        isoFileWriter.WriteLine(currList.Title);
                        for(int k = 0; i < currList.Tasks.Count(); k++)
                        {
                            task currTask = currList.Tasks[k];
                            isoFileWriter.Write("|" + currTask.Title + "," + currTask.Description);
                        }
                    }
                    
                }
            }

            
        }
    }
}