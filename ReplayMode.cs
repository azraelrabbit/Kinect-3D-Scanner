using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace _3D_Scanner_v2
{
    class ReplayMode : Mode
    {

        private MyKinect myKinect;

        private bool isrunning = false;
        
        private bool over = false;

        public event ImageChangeEventHandler ImageChanged;

        private FileFrameSource frameSource;

        private TimeSpan? oldFrameTime;

        public ReplayMode(MyKinect myKinect)
        {
            this.myKinect = myKinect;
            
        }

        private void FrameReadyHandlerAdapter(object s, FrameArrivedEventArgs ea)
        {
            if (oldFrameTime == null || oldFrameTime.Value > ea.DepthFrame.RelativeTime)
                oldFrameTime = ea.DepthFrame.RelativeTime;
            Thread.Sleep(ea.DepthFrame.RelativeTime - oldFrameTime.Value);
            OnFrameReady(ea.DepthFrame);
            oldFrameTime = ea.DepthFrame.RelativeTime;
        }

        private void OnFrameReady(MyDepthFrameData depthFrame)
        {
            if (!over)
            {
                ushort maxDepth = ushort.MaxValue;
                ushort minDepth = depthFrame.MinDepth;
                for (int i = 0; i < depthFrame.Data.Length; ++i)
                {
                    ushort depth = depthFrame.Data[i];
                    // To convert to a byte, we're mapping the depth value to the byte range.
                    // Values outside the reliable depth range are mapped to 0 (black).
                    myKinect.depthPixels[i] =
                        (byte) (depth >= minDepth && depth <= maxDepth ? (depth/myKinect.MapDepthToByte) : 0);
                }
                myKinect.depthBitmap.Dispatcher.Invoke(() =>
                {
                    myKinect.depthBitmap.WritePixels(
                        new Int32Rect(0, 0, myKinect.depthBitmap.PixelWidth, myKinect.depthBitmap.PixelHeight),
                        myKinect.depthPixels,
                        myKinect.depthBitmap.PixelWidth,
                        0);

                    OnImageChanged(myKinect.depthBitmap);
                });
            }
        }

        public void Start()
        {
            over = false;
            isrunning = true;
            frameSource = new FileFrameSource(File.Open(@"C:\Users\Bullet\Desktop\recording.bin", FileMode.Open, FileAccess.Read, FileShare.Read));
            frameSource.FrameArrived += FrameReadyHandlerAdapter;
            frameSource.Start();
        }

        public void Stop()
        {
            isrunning = false;
            frameSource.Stop();
        }


        public void Finish()
        {
            if (isrunning)
            {
                frameSource.FrameArrived -= FrameReadyHandlerAdapter;
                myKinect.depthBitmap = new WriteableBitmap(myKinect.depthFrameDescription.Width,
                    myKinect.depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Gray8, null);
                OnImageChanged(myKinect.depthBitmap);

                this.Stop();
                over = true;
            }

        }


        public String ButtonRename()
        {
            if (isrunning)
            {
                return "Stop Replay";
            }
            else return "Start Replay";
        }


        public bool isRunning()
        {
            return isrunning;
        }


        public void Initialize()
        {

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
