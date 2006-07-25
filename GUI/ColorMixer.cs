using System;

namespace System.Drawing
{
	/// <summary>
	/// Creates a random colors.
	/// </summary>
	public class ColorMixer
	{
		static Color[][] mColors;

		/// <summary>
		/// Avoid instatize this class
		/// </summary>
		private ColorMixer(){}
		/// <summary>
		/// Static Constructor;
		/// </summary>
		static ColorMixer()
		{
			mColors = new Color[][]
				{
					new Color[] {Color.Black         ,Color.White},
					new Color[] {Color.Indigo        ,Color.Yellow},
					new Color[] {Color.MidnightBlue	 ,Color.LightCoral},
					new Color[] {Color.Blue          ,Color.WhiteSmoke},
					new Color[] {Color.DarkGreen     ,Color.White},
					new Color[] {Color.WhiteSmoke    ,Color.Black},
					new Color[] {Color.AntiqueWhite  ,Color.DeepPink },
					new Color[] {Color.Wheat         ,Color.Brown},
					new Color[] {Color.SeaShell      ,Color.DarkCyan},
					new Color[] {Color.Aquamarine    ,Color.RosyBrown},
					new Color[] {Color.Bisque        ,Color.Black},
					new Color[] {Color.Yellow		 ,Color.Black},
					new Color[] {Color.LightYellow   ,Color.DarkRed},
					new Color[] {Color.LightGoldenrodYellow,Color.Black},
					new Color[] {Color.LemonChiffon  ,Color.DeepPink},
					new Color[] {Color.CornflowerBlue,Color.DarkGreen},
					new Color[] {Color.LightSlateGray,Color.Yellow},
					new Color[] {Color.LightSkyBlue  ,Color.Brown},
					new Color[] {Color.LightPink     ,Color.DarkMagenta},
					new Color[] {Color.LightSalmon   ,Color.DarkOrchid},
					new Color[] {Color.LightSeaGreen ,Color.DarkBlue},
					new Color[] {Color.LimeGreen     ,Color.Firebrick},
					new Color[] {Color.Cornsilk      ,Color.Crimson },
					new Color[] {Color.PapayaWhip    ,Color.Navy },
					new Color[] {Color.PeachPuff     ,Color.Maroon },
					new Color[] {Color.PowderBlue    ,Color.Black },
					new Color[] {Color.MistyRose     ,Color.Black },
					new Color[] {Color.Thistle       ,Color.DimGray },
					new Color[] {Color.PowderBlue    ,Color.Firebrick },
					new Color[] {Color.Silver        ,Color.PaleGreen },
					new Color[] {Color.SkyBlue       ,Color.SaddleBrown },
					new Color[] {Color.Lavender		 ,Color.Red },
					new Color[] {Color.SpringGreen   ,Color.Navy },
					new Color[] {Color.LightSteelBlue,Color.Black }
				};
		}
		static public void Colors( int seed, out Color foreGround, out Color backGround )
		{
			foreGround = ForeGroundColor( seed );
			backGround = BackGroundColor( seed );

		}
		static public Color ForeGroundColor( int seed )
		{
			Random rnd = new Random( seed );
			return mColors[ rnd.Next( mColors.Length ) ][1];
		}
		static public Color BackGroundColor( int seed )
		{
			Random rnd = new Random( seed );
			return mColors[ rnd.Next( mColors.Length ) ][0];
		}
	}
}
