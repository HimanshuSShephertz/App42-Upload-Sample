using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using com.shephertz.app42.paas.sdk.windows;
using com.shephertz.app42.paas.sdk.windows.user;
using com.shephertz.app42.paas.sdk.windows.upload;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Phone.Controls;
// This is the main page of your app

namespace Sample
{
    public partial class MainPage : PhoneApplicationPage,App42Callback
    {
        static ServiceAPI serviceApi = new ServiceAPI("62f6b446a152a488b92a7cc27421e6ee105247a973246b2d528ca67f746004fc", "6ee0b3f11115e55b92c3eab66a5a2a94197a27cea8e43f4239748a443214a312");
        // Here you have to use your apiKey and secretKey in order to obtain connection from App42 Server
        static String fileName = "Sample"+DateTime.Now.Millisecond+".jpg";
        static String userName;
        static String fileDescription;
        PhotoChooserTask photoChooserTask;
        UploadService uploadservice = serviceApi.BuildUploadService();
        String photoPath;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
        }
        // this is the click event
        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                userName = username.Text;
                fileDescription = description.Text;
                bmp.SetSource(e.ChosenPhoto);
                photoPath = e.OriginalFileName;
                uploadservice.UploadFileForUser(fileName, userName, e.ChosenPhoto, UploadFileType.IMAGE, fileDescription, this);
            }
        }

        private void uploadClick(object sender, RoutedEventArgs e)
        {
            photoChooserTask.Show();
        }
       void App42Callback.OnException(App42Exception exception)
        {
            Dispatcher.BeginInvoke(() => { textB.Text = "Exception Caught"; });
        }

        void App42Callback.OnSuccess(object response)
        {
            // here success is shown
            if (response is Upload)
            {
                Upload upload = (Upload)response;
                Dispatcher.BeginInvoke(() => { textB.Text = "Uploaded File Url.."+ upload.GetFileList()[0].GetTinyUrl(); });
            }
        }
    }
}