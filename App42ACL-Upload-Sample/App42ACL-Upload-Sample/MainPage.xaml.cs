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
using Microsoft.Phone.Controls;
using com.shephertz.app42.paas.sdk.windows;
using Microsoft.Phone.Tasks;
using com.shephertz.app42.paas.sdk.windows.upload;
using System.Windows.Media.Imaging;
using System.IO;
using com.shephertz.app42.paas.sdk.windows.user;

namespace App42ACL_Upload_Sample
{
    public partial class MainPage : PhoneApplicationPage, App42Callback
    {
        static ServiceAPI serviceApi = new ServiceAPI("5a1297a10422f957781ccebbee0aa6a2988d2dd9e0a1c2eea9b949b065fd1d87", "5e94d4fbaa6c6f032478ff92cedc7059ada2c62da46a392a5c2ed9826be6a250");
        // Here you have to use your apiKey and secretKey in order to obtain connection from App42 Server
        static String uploadFileName ;
        static String userName;
        static String fileDescription;
        Stream filepathStream;
        PhotoChooserTask photoChooserTask;
        UploadService uploadservice;
        UserService userService;
        String photoPath;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            photoChooserTask = new PhotoChooserTask();
            photoChooserTask.Completed += new EventHandler<PhotoResult>(photoChooserTask_Completed);
            uploadservice = serviceApi.BuildUploadService();
            userService = serviceApi.BuildUserService();
}
        // this is the click event
        void photoChooserTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bmp = new BitmapImage();
                //userName = username.Text;
                //uploadFileName = fileName.Text;
                //fileDescription = description.Text;
                bmp.SetSource(e.ChosenPhoto);
                photoPath = e.OriginalFileName;
                filepathStream = e.ChosenPhoto;
                //uploadservice.SetAdminKey("64c9a0342f603107d44f9e7408cb0da8ee4b8b2fb57e322c6d0e39eecd03a961");
                uploadservice.UploadFileForUser(uploadFileName,userName, filepathStream, UploadFileType.IMAGE, fileDescription, this);
             }
        }

        private void uploadClick(object sender, RoutedEventArgs e)
        {
           //photoChooserTask.Show();
            userName = username.Text; 
            uploadFileName = fileName.Text + ".jpg";
            fileDescription = description.Text;
            String email = userName + "@shephertz.co.in";
            String password = "123456";
            userService.CreateUser(userName, password, email, this);
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
                Dispatcher.BeginInvoke(() => { textB.Text = "Uploaded File Url.." + upload.GetFileList()[0].GetTinyUrl(); });
            }
            if (response is User)
            {
                photoChooserTask.Show();
            }
        }
    }
}