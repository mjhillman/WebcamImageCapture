using HITS.LIB.WeakEvents;
using OpenCvSharp;
using System.Timers;

namespace WebcamImageCapture
{
    internal sealed class CaptureService : IDisposable
    {
        private System.Timers.Timer _timer;
        private System.Timers.Timer _timer2 = null;
        private VideoCapture _capture;
        private string _capturePath;
        private long _lastDirSize = 0;

        //events
        private EventMgr mgr = new EventMgr();
        internal const long MAX_DIR_SIZE = 5000000000;
        internal const string FRAME_CAPTURED = "FRAME_CAPTURED";
        internal const string MAX__DIR_SIZE_EXCEEDED = "MAX__DIR_SIZE_EXCEEDED";


        public CaptureService()
        {
        }

        public Task StartAsync(CancellationToken cancellationToken, int webcamIndex, int frequency, int width, int height, string path)
        {
            if (_timer2 == null)
            {
                _timer2 = new System.Timers.Timer(1 * 60 * 1000);
                _timer2.Elapsed += CaptureDirSize;
                _timer2.AutoReset = true;   // Keep repeating
                _timer2.Enabled = true;     // Start the timer
            }

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
                        Args = _lastDirSize,
                        Data = filename,
                        Token = FRAME_CAPTURED
                    });
                }
            }
        }

        private void CaptureDirSize(object sender, ElapsedEventArgs e)
        {
            _lastDirSize = GetDirectorySize(_capturePath);
            if (_lastDirSize > MAX_DIR_SIZE)
            {
                mgr.PublishEvent(new EventData
                {
                    Sender = this,
                    Args = null,
                    Data = _lastDirSize,
                    Token = MAX__DIR_SIZE_EXCEEDED
                });
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
            _timer2?.Stop();
            _timer2?.Dispose();
            _timer2 = null;

            _timer?.Stop();
            _capture?.Release();
            _timer?.Elapsed -= CaptureFrame;
            return Task.CompletedTask;
        }

        private long GetDirectorySize(string path)
        {
            long size = 0;
            try
            {
                // Add file sizes.
                foreach (var filePath in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var file = new FileInfo(filePath);
                        size += file.Length;
                    }
                    catch (Exception)
                    {
                        // Ignore files that can't be accessed and continue
                    }
                }

                return size;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public void Dispose()
        {
            _timer?.Dispose();
            _timer2?.Dispose();
            _capture?.Dispose();
        }
    }
}
