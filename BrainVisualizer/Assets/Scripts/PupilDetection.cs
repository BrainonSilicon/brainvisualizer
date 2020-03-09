namespace OpenCvSharp.PupilDetect
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using OpenCvSharp;
    using System;


    public class PupilDetection : MyWebCamera
    {
        public TextAsset eyes;

        private Mat InputImage;
        private Mat OutputImage;
        protected Mat processingImage = null;


        protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
        {
            InputImage = Unity.TextureToMat(input as UnityEngine.WebCamTexture, TextureParameters);
            ProcessImage();
            output = Unity.MatToTexture(OutputImage, output); ;
            return true;
        }


        private static void RemoveAt<T>(ref T[] arr, int index)
        {
            for (int a = index; a < arr.Length - 1; a++)
            {
                // moving elements downwards, to fill the gap at [index]
                arr[a] = arr[a + 1];
            }
            // finally, let's decrement Array's size by one
            Array.Resize(ref arr, arr.Length - 1);
        }

        private int minList(List<int> list)
        {
            int res = 0;
            foreach (int element in list)
                if (element < res)
                    res = element;

            return res;
        }

        private int maxList(List<int> list)
        {
            int res = 0;
            foreach (int element in list)
                if (element > res)
                    res = element;

            return res;
        }

        private void ProcessImage()
        {
            Mat Frame = new Mat();
            Cv2.CvtColor(InputImage, Frame, ColorConversionCodes.RGB2GRAY);
            Cv2.EqualizeHist(Frame, Frame);
            CascadeClassifier faces = new CascadeClassifier();
            FileStorage storageEyes = new FileStorage(eyes.text, FileStorage.Mode.Read | FileStorage.Mode.Memory);
            if (!faces.Read(storageEyes.GetFirstTopLevelNode()))
                throw new System.Exception("FaceProcessor.Initialize: Failed to load faces cascade classifier");
            var detected = faces.DetectMultiScale(Frame, 1.3, 5);

            var pupilFrame = Frame;
            var pupil0 = Frame;

            for (int i = 0; i < detected.Length; i++)
            {
                var eye = detected[i];
                var r = new OpenCvSharp.Rect(eye.X, eye.Y, eye.Width, eye.Height);
                Cv2.Rectangle((InputOutputArray)Frame, r, Scalar.FromRgb(255, 0, 0), 2);
             
                Cv2.EqualizeHist(Frame[eye.Y + System.Convert.ToInt32(eye.Height *0.25), eye.Y + eye.Height, eye.X, eye.X + eye.Width], pupilFrame);
                pupil0 = pupilFrame;
                Cv2.Threshold(pupilFrame, pupilFrame, 55, 255, ThresholdTypes.Binary);
                Size s = new Size(2, 2);
                Size s1 = new Size(5, 5);
                InputArray windowClose = new InputArray(Mat.Ones(s1, OpenCvSharp.MatType.CV_8U));
                InputArray windowOpen = new InputArray(Mat.Ones(s, MatType.CV_8U));
                InputArray windowErode = new InputArray(Mat.Ones(s, MatType.CV_8U));

                Cv2.MorphologyEx(pupilFrame, pupilFrame, MorphTypes.Close, windowClose);
                Cv2.MorphologyEx(pupilFrame, pupilFrame, MorphTypes.ERODE, windowErode);
                Cv2.MorphologyEx(pupilFrame, pupilFrame, MorphTypes.Open, windowOpen);

                Mat threshold = new Mat();
                Cv2.InRange(pupilFrame, 250, 255, threshold);
                Mat[] contours;// = new Mat[5];
                OutputArray hier = new OutputArray(pupil0);
                Cv2.FindContours(threshold, out contours, hier, RetrievalModes.List, ContourApproximationModes.ApproxSimple);

                List<int> distanceX = new List<int>();

                if (contours.Length >= 2)
                {
                    double maxArea = 0;
                    var MAIndex = 0;
                    for (int j=0; j<contours.Length; j++)
                    {
                        var area = Cv2.ContourArea(contours[j]);
                        var center = Cv2.Moments(contours[j]);
                        int cx = Convert.ToInt32( center.M10 / center.M00);
                        int cy = Convert.ToInt32( center.M01 / center.M00);
                        distanceX.Add(cx);
                        if (area > maxArea)
                        {
                            maxArea = area;
                            MAIndex = j;
                        }
                    }
                    distanceX.RemoveAt(MAIndex);
                    RemoveAt<Mat>(ref contours, MAIndex);
                }
                var eyeRight = "right";

                if (contours.Length >= 2)
                {
                    int edgeOfEye;
                    if (eyeRight == "right")
                    {
                        edgeOfEye = distanceX.IndexOf(minList(distanceX));
                    }
                    else
                    {
                        edgeOfEye = distanceX.IndexOf(maxList(distanceX));
                    }
                    distanceX.RemoveAt(edgeOfEye);
                    RemoveAt<Mat>(ref contours, edgeOfEye);
                }

                Mat largeBlob = new Mat();
                if( contours.Length >= 1)
                {
                    Double maxArea = 0;
                    for (int j=0; j<contours.Length; j++)
                    {
                        var area = Cv2.ContourArea(contours[j]);
                        if (area > maxArea)
                        {
                            maxArea = area;
                            largeBlob = contours[j];
                        }
                    }
                }

                if (!largeBlob.Empty())
                {
                    var center = Cv2.Moments(largeBlob);
                    var cx = Convert.ToInt32(center.M10 / center.M00);
                    var cy = Convert.ToInt32(center.M01 / center.M00);
                    Cv2.Circle(pupil0, cx, cy, 5, Scalar.FromRgb(0, 255, 0), 1);
                }

            }
            OutputImage = pupil0;
        }
    }
}
