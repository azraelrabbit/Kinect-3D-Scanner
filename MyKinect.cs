using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using Microsoft.Kinect.Fusion;
using System.Windows.Media.Imaging;
using System.IO;
using System.Windows.Media;

namespace _3D_Scanner_v2
{
    class MyKinect
    {
        /// Map depth range to byte range
        public int MapDepthToByte = 8000 / 256;

        /// Active Kinect sensor
        public KinectSensor Sensor = null;

        /// Reader for depth frames
        public DepthFrameReader depthFrameReader = null;

        /// Description of the data contained in the depth frame
        public FrameDescription depthFrameDescription = null;


        /// Bitmap to display
        public WriteableBitmap depthBitmap = null;

        /// Intermediate storage for frame data converted to color
        public byte[] depthPixels = null;


        //delegate for statustext changing
        public delegate void StatusTextEventHandler(object source, string msg);
        public event StatusTextEventHandler StatusTextChanged;


        public event ImageChangeEventHandler ImageChanged;


        public MyKinect()
        {
            // get the kinectSensor object
            Sensor = KinectSensor.GetDefault();

            // open the reader for the depth frames
            depthFrameReader = Sensor.DepthFrameSource.OpenReader();
            
            // get FrameDescription from DepthFrameSource
            depthFrameDescription = Sensor.DepthFrameSource.FrameDescription;

            // allocate space to put the pixels being received and converted
            depthPixels = new byte[depthFrameDescription.Width * depthFrameDescription.Height];

            // create the bitmap to display
            depthBitmap = new WriteableBitmap(this.depthFrameDescription.Width, this.depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);

            // set IsAvailableChanged event notifier
            Sensor.IsAvailableChanged += this.Sensor_IsAvailableChanged;
        }

        public bool StartSensor()
        {
            bool ret = false;
            

            if (CheckRequirements() == true)
            {
                Sensor.Open();
                ret = true;
            }

            return ret;
        }

        //TO DO
        //StopSensor//////////////

        private bool CheckRequirements()
        {
            bool ret = true;
            return ret;
        }



        protected virtual void OnStatusTextChanged(string message)
        {
            if (StatusTextChanged != null)
            {
                StatusTextChanged(this, message);
            }
        }


        /// Handles the event which the sensor becomes unavailable (E.g. paused, closed, unplugged).
        private void Sensor_IsAvailableChanged(object sender, IsAvailableChangedEventArgs e)
        {
            if (Sensor.IsAvailable)
            {
                OnStatusTextChanged("Running");
            }
            else
            {
                OnStatusTextChanged("No Sensor Available");
            }
        }
    }
}
