﻿using System.Reflection;
using UnityEngine.Events;

namespace OpenCvSharp.Demo
{
	using UnityEngine;
	using OpenCvSharp;

	public class FaceDetectorScene : WebCamera
	{
		public TextAsset faces;
		public TextAsset eyes;
		public TextAsset shapes;

		public UnityEvent<Vector3> unityEvent;
		
		private FaceProcessorLive<WebCamTexture> processor;
		public CascadeClassifier faceDetector;
		private Vector3 currentVelocity = Vector3.zero;
		/// <summary>
		/// Default initializer for MonoBehavior sub-classes
		/// </summary>
		protected override void Awake()
		{
			base.Awake();
			base.forceFrontalCamera = true; // we work with frontal cams here, let's force it for macOS s MacBook doesn't state frontal cam correctly

			byte[] shapeDat = shapes.bytes;
			if (shapeDat.Length == 0)
			{
				string errorMessage =
					"In order to have Face Landmarks working you must download special pre-trained shape predictor " +
					"available for free via DLib library website and replace a placeholder file located at " +
					"\"OpenCV+Unity/Assets/Resources/shape_predictor_68_face_landmarks.bytes\"\n\n" +
					"Without shape predictor demo will only detect face rects.";

#if UNITY_EDITOR
				// query user to download the proper shape predictor
				if (UnityEditor.EditorUtility.DisplayDialog("Shape predictor data missing", errorMessage, "Download", "OK, process with face rects only"))
					Application.OpenURL("http://dlib.net/files/shape_predictor_68_face_landmarks.dat.bz2");
#else
             UnityEngine.Debug.Log(errorMessage);
#endif
			}

			processor = new FaceProcessorLive<WebCamTexture>();
			processor.Initialize(faces.text, eyes.text, shapes.bytes);

			// data stabilizer - affects face rects, face landmarks etc.
			processor.DataStabilizer.Enabled = true;        // enable stabilizer
			processor.DataStabilizer.Threshold = 2.0;       // threshold value in pixels
			processor.DataStabilizer.SamplesCount = 2;      // how many samples do we need to compute stable data

			// performance data - some tricks to make it work faster
			processor.Performance.Downscale = 256;          // processed image is pre-scaled down to N px by long side
			processor.Performance.SkipRate = 0;             // we actually process only each Nth frame (and every frame for skipRate = 0)
		}

		private void Start()
		{
			faceDetector = processor.GetType().GetField("cascadeFaces", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(processor) as CascadeClassifier;
		}

		private Vector3 curPosition;
		/// <summary>
		/// Per-frame video capture processor
		/// </summary>
		protected override bool ProcessTexture(WebCamTexture input, ref Texture2D output)
		{
			// detect everything we're interested in
			processor.ProcessTexture(input, TextureParameters);

			// mark detected objects
			processor.MarkDetected();

			// processor.Image now holds data we'd like to visualize
			output = Unity.MatToTexture(processor.Image, output);   // if output is valid texture it's buffer will be re-used, otherwise it will be re-created

			foreach (var detectedFace in processor.Faces)
			{
				var location = new Vector2((detectedFace.Region.Center.X - output.width / 2f) / (output.width / 2f),
					(output.height / 2f - detectedFace.Region.Center.Y) / (output.height / 2f));

				location.x = Mathf.Clamp(location.x, -.2f, .2f);
				location.y = Mathf.Clamp(location.y, -.05f, .05f);

				var pos = Vector3.SmoothDamp(curPosition, location, ref currentVelocity, 1f);
				curPosition = pos;
				unityEvent?.Invoke(pos);
			}
			return true;
		}
	}
}