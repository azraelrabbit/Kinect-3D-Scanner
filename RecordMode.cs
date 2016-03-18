using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.IO;

namespace _3D_Scanner_v2
{
    class RecordMode : Mode
    {
        private MyKinect myKinect;

        private bool isrunning = false;
        
        private bool over = false;

        public Stream SaveStream { get; set; }

        public event ImageChangeEventHandler ImageChanged;


        public RecordMode(MyKinect myKinect)
        {
            this.myKinect = myKinect;
        }

        private void FrameReadyHandlerAdapter(object s, DepthFrameArrivedEventArgs ea)
        {
            using (DepthFrame df = ea.FrameReference.AcquireFrame())
                if (df != null)
                {
                    OnFrameReady(new MyDepthFrameData(df));
                }
        }

        public void Initialize()
        {
            over = false;
            myKinect.depthFrameReader.FrameArrived += FrameReadyHandlerAdapter;
        }


        public void Start()
        {
            isrunning = true;
            SaveStream = File.Open(@"C:\Users\Bullet\Desktop\recording.bin", FileMode.Create);
        }


        public void Stop()
        {
            isrunning = false;

            if (SaveStream != null)
            {
                SaveStream.Close();
            }
        }


        public void Finish()
        {
            myKinect.depthFrameReader.FrameArrived -= FrameReadyHandlerAdapter;
            myKinect.depthBitmap = new WriteableBitmap(myKinect.depthFrameDescription.Width, myKinect.depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);
            OnImageChanged(myKinect.depthBitmap);

            this.Stop();
            over = true;
        }


        public String ButtonRename()
        {
            if (isrunning)
            {
                return "Stop Recording";
            }
            else return "Start Recording";
        }

        public bool isRunning()
        {
            return isrunning;
        }

        private void OnFrameReady(MyDepthFrameData depthFrame)
        {
            if (!over)
            {
                if (isrunning)
                {
                    depthFrame.ToStream(SaveStream);
                }

                ushort maxDepth = ushort.MaxValue;
                ushort minDepth = depthFrame.MinDepth;
                for (int i = 0; i < depthFrame.Data.Length; i++)
                {
                    ushort depth = depthFrame.Data[i];
                    // To convert to a byte, we're mapping the depth value to the byte range.
                    // Values outside the reliable depth range are mapped to 0 (black).
                    myKinect.depthPixels[i] =
                        (byte) (depth >= minDepth && depth <= maxDepth ? (depth/myKinect.MapDepthToByte) : 0);
                }

                myKinect.depthBitmap.WritePixels(
                    new Int32Rect(0, 0, myKinect.depthBitmap.PixelWidth, myKinect.depthBitmap.PixelHeight),
                    myKinect.depthPixels,
                    myKinect.depthBitmap.PixelWidth,
                    0);

                OnImageChanged(myKinect.depthBitmap);
            }
        }
        

        protected virtual void OnImageChanged(WriteableBitmap depthBitmap)
        {

            if (ImageChanged != null)
            {
                ImageChanged(this, depthBitmap);
            }
        }

    }
}
