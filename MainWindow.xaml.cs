using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace _3D_Scanner_v2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        RadioButton[] radios;
        Mode[] modes;

        public MainWindow()
        {
            InitializeComponent();
        }


        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            radios = new RadioButton[3];
            radios[0] = FindName("radiobuttonRecord") as RadioButton;
            radios[1] = FindName("radiobuttonReplay") as RadioButton;
            radios[2] = FindName("radiobuttonReconstruct") as RadioButton;

            MyKinect mykinect = new MyKinect();
            mykinect.StatusTextChanged += OnStatusTextChanged;
            if (mykinect.StartSensor())
            {
                modes = new Mode[3];
                modes[0] = new RecordMode(mykinect);
                modes[1] = new ReplayMode(mykinect);
                modes[2] = new ReconstructionMode();

                modes[0].ImageChanged += OnImageChanged;
                modes[1].ImageChanged += OnImageChanged;
                //modes[2].ImageChanged += OnImageChanged;

            }

            WriteableBitmap depthBitmap;
            depthBitmap = new WriteableBitmap(mykinect.depthFrameDescription.Width, mykinect.depthFrameDescription.Height, 96.0, 96.0, PixelFormats.Bgr32, null);
            this.Image.Source = depthBitmap;

        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            
        }


        private void radioButtonChecked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < radios.Length; i++)
            {
                if (radios[i].IsChecked == true)
                {
                    UniversalButton.Content = modes[i].ButtonRename();
                    modes[i].Initialize();
                }
                else
                {
                    modes[i].Finish();
                }
            }
        }

        private void ButtonClicked(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < radios.Length; i++)
            {
                if (radios[i].IsChecked == true)
                {
                    if (modes[i].isRunning())
                    {
                        modes[i].Stop();
                        UniversalButton.Content = modes[i].ButtonRename();
                    }
                    else
                    {
                        modes[i].Start();
                        UniversalButton.Content = modes[i].ButtonRename();
                    }
                }
            }
        }


        public void OnStatusTextChanged(object source, string msg)
        {
            statusBarText.Text = msg;
        }
        

        public void OnImageChanged(object source, WriteableBitmap colorBitmap)
        {
            Image.Source = colorBitmap;
        }
    }
}
