using HITS.LIB.WeakEvents;
using OpenCvSharp;
using System.Timers;

namespace WebcamImageCapture
{
    internal sealed class CaptureService : IDisposable
    {
        private System.Timers.Timer _timer;
        private VideoCapture _capture;
        private EventMgr mgr = new EventMgr();
        private string _capturePath;

        public CaptureService()
        {
            
        }

        public Task StartAsync(CancellationToken cancellationToken, int webcamIndex, int frequency, int width, int height, string path)
        {
            _capture?.Dispose();
            _timer?.Dispose();
            _capturePath = path;
            _capture = new VideoCapture(webcamIndex); // 0=default webcam 1=external webcam
            _capture.Set(VideoCaptureProperties.FrameWidth, width);
            _capture.Set(VideoCaptureProperties.FrameHeight, height);
            _timer = new System.Timers.Timer(frequency * 1000);
            _timer.Elapsed += CaptureFrame;
            _timer.Start();
            return Task.CompletedTask;
        }

        private void CaptureFrame(object sender, ElapsedEventArgs e)
        {
            using (var frame = new Mat())
            {
                _capture.Read(frame);
                if (!frame.Empty())
                {
                    string d = DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss");
                    var filename = $"{_capturePath}\\{d}.jpg";
                    Cv2.ImWrite(filename, frame);
                    mgr.PublishEvent(new EventData
                    {
                        Sender = this,
                        Args = null,
                        Data = filename,
                        Token = "CaptureService"
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="webcamIndex"></param>
        /// <returns>var maxRes = WebcamHelper.GetMaxResolution(0); // 0 = default webcam
        ///Console.WriteLine($"Max supported resolution: {maxRes.width}x{maxRes.height}");
        ///</returns>
        public static (int width, int height) GetMaxResolution(int webcamIndex = 0)
        {
            using var capture = new VideoCapture(webcamIndex);

            // Try to set an absurdly high resolution
            capture.Set(VideoCaptureProperties.FrameWidth, 9999);
            capture.Set(VideoCaptureProperties.FrameHeight, 9999);

            // Query back what the camera actually negotiated
            int actualW = (int)capture.Get(VideoCaptureProperties.FrameWidth);
            int actualH = (int)capture.Get(VideoCaptureProperties.FrameHeight);
            capture.Dispose();
            return (actualW, actualH);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            _capture?.Release();
            _timer?.Elapsed -= CaptureFrame;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            _capture?.Dispose();
        }
    }
}
