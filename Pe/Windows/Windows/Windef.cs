﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 13:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.InteropServices;

namespace SC.Windows
{
	/// <summary>
	/// http://www.pinvoke.net/default.aspx/Structures/rect.html
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;
		
		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}
		
		public RECT(System.Drawing.Rectangle r) : this(r.Left, r.Top, r.Right, r.Bottom) { }
		
		public int X
		{
			get { return Left; }
			set { Right -= (Left - value); Left = value; }
		}
		
		public int Y
		{
			get { return Top; }
			set { Bottom -= (Top - value); Top = value; }
		}
		
		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = value + Top; }
		}
		
		public int Width
		{
			get { return Right - Left; }
			set { Right = value + Left; }
		}
		
		public System.Drawing.Point Location
		{
			get { return new System.Drawing.Point(Left, Top); }
			set { X = value.X; Y = value.Y; }
		}
		
		public System.Drawing.Size Size
		{
			get { return new System.Drawing.Size(Width, Height); }
			set { Width = value.Width; Height = value.Height; }
		}
		
		public static implicit operator System.Drawing.Rectangle(RECT r)
		{
			return new System.Drawing.Rectangle(r.Left, r.Top, r.Width, r.Height);
		}
		
		public static implicit operator RECT(System.Drawing.Rectangle r)
		{
			return new RECT(r);
		}
		
		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}
		
		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}
		
		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}
		
		public override bool Equals(object obj)
		{
			if (obj is RECT)
				return Equals((RECT)obj);
			else if (obj is System.Drawing.Rectangle)
				return Equals(new RECT((System.Drawing.Rectangle)obj));
			return false;
		}
		
		public override int GetHashCode()
		{
			return ((System.Drawing.Rectangle)this).GetHashCode();
		}
		
		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}
	}
	
	/// <summary>
	/// http://pinvoke.net/default.aspx/Structures.POINT
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct POINT
	{
		public int X;
		public int Y;

		public POINT(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		public POINT(System.Drawing.Point pt) : this(pt.X, pt.Y) { }

		public static implicit operator System.Drawing.Point(POINT p)
		{
			return new System.Drawing.Point(p.X, p.Y);
		}

		public static implicit operator POINT(System.Drawing.Point p)
		{
			return new POINT(p.X, p.Y);
		}
	}
}
