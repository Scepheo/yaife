﻿using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace Yaife
{
	public partial class SubtitleForm : Form
	{
		public Subtitle[] Subtitles
		{
			get { return subtitlesBindingSource.Cast<Subtitle>().ToArray(); }
			set { subtitlesBindingSource.DataSource = value; }
		}

		public SubtitleForm()
		{
			InitializeComponent();
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}
	}

	public class SubtitleEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			var form = new SubtitleForm();
			form.Subtitles = value as Subtitle[];

			if (form.ShowDialog() == DialogResult.OK)
				return form.Subtitles;
			else
				return value;
		}
	}

	public class SubtitleConverter : ExpandableObjectConverter
	{
		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			if (destinationType == typeof(Subtitle[]))
				return true;

			return base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			if (value is Subtitle[] && destinationType == typeof(string))
			{
				var array = value as Subtitle[];

				if (array.Length > 0)
					return array[0].Text;
				else
					return "";
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}