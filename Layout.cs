using DevExpress.LookAndFeel;
using DevExpress.Utils.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BFS_VoIP
{
	class MyUltraFlatLookAndFeelPainters : UltraFlatLookAndFeelPainters
	{
		public MyUltraFlatLookAndFeelPainters(UserLookAndFeel owner) : base(owner) { }
		protected override ObjectPainter CreateButtonPainter()
		{
			return new MyUltraFlatButtonObjectPainter();
		}
	}

	class MyUltraFlatButtonObjectPainter : UltraFlatButtonObjectPainter
	{
		protected override Brush GetHotBackBrush(ObjectInfoArgs e, bool pressed)
		{
			return new SolidBrush(ControlPaint.Dark((GetNormalBackBrush(e) as SolidBrush).Color, 0.2f));
		}

		protected override Color GetHotBorderColor(ObjectInfoArgs e, bool pressed)
		{
			return Color.Transparent;
		}
	}

	public class MyRenderer : ToolStripProfessionalRenderer
	{
		public MyRenderer() : base(new MyColors()) { }
	}

	class MyColors : ProfessionalColorTable
	{
		public override Color MenuItemBorder
		{
			get { return Color.Transparent; }
		}
		public override Color MenuItemSelected
		{
			get { return Color.DarkGray; }
		}
		public override Color MenuItemSelectedGradientBegin
		{
			get { return Color.Gray; }
		}
		public override Color MenuItemSelectedGradientEnd
		{
			get { return Color.DarkGray; }
		}
	}
}
