﻿using System.IO;
using System.Windows.Forms;

namespace Yaife.Formats.PCSX
{
	public class Movie : IMovie, IFrameCount
	{
		public InputLog InputLog { get; set; }

		public Header RealHeader;
		public object Header
		{
			get { return RealHeader; }
		}

		public bool HasMenu { get { return false; } }
		public ToolStripMenuItem GetMovieMenu(MovieTab tab) { return null; }

		public string Path { get; set; }

		public string Description
		{
			get { return "PCSX-rr"; }
		}

		public string[] Extensions
		{
			get { return new string[] { ".pxm" }; }
		}

		public void ReadFile(string path)
		{
			this.Path = path;

			var file = new FileStream(path, FileMode.Open);
			this.RealHeader = new Header();
			RealHeader.Read(file);
			InputLog = InputLogIO.Read(file, RealHeader.ControllerTypePort1, RealHeader.ControllerTypePort2);

			file.Close();
		}

		public void WriteFile(string path)
		{
			var file = new FileStream(path, FileMode.Create);

			RealHeader.Write(file);
			InputLogIO.Write(file, InputLog);

			file.Close();
		}

		public bool IsEmptyFrame(string[] strings)
		{
			var frame = new Frame();
			frame.Parse(strings);
			return frame.IsEmpty();
		}

		public void SetFrameCount(uint count)
		{
			RealHeader.FrameCount = count;
		}
	}
}