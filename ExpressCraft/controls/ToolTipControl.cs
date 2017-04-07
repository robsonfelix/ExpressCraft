﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge.Html5;

namespace ExpressCraft
{
	internal class ToolTipControl : Control
	{
		private bool visible = false;
		private ToolTip _toolTip;
		public ToolTipControl(ToolTip toolTip) : base("tool-tip")
		{
			_toolTip = toolTip;
		}		

		public void Show(MouseEvent ev)
		{
			this.Content.Empty();

			if(_toolTip != null)
			{
				if(_toolTip.Heading.IsEmpty())
				{
					this.Content.AppendChild(new HTMLSpanElement() { InnerHTML = string.Format("<b>{0}</b>", _toolTip.Heading.HtmlEscape()) });
				}
				if(_toolTip.Heading.IsEmpty())
				{
					this.Content.AppendChild(new HTMLSpanElement() { InnerHTML = string.Format("<b>{0}</b>", _toolTip.Heading.HtmlEscape()) });
				}
			}
			var mouse = Helper.GetClientMouseLocation(ev);

			this.Location = new Vector2(mouse.X, mouse.Y.ToInt() + 22);

			if(!visible)
			{
				visible = true;
				ContextMenu.TotalContextHandles++;
				Content.Style.ZIndex = (ContextMenu.TotalContextHandles + Settings.ContextMenuStartingZIndex).ToString();
				Document.Body.AppendChild(this);
			}
		}

		public void Close()
		{
			if(visible)
			{				
				this.Content.Delete();
				visible = false;
				ContextMenu.TotalContextHandles--;				
			}
		}
	}

	public class ToolTip
	{
		public string Description;
		public string Heading;

		public int GetWordCount()
		{
			var fullContent = string.Concat(Heading, " ", Description).Trim();
			var length = fullContent.Length;
			char prevChar = '\0';
			var builder = new StringBuilder();
			char current;
			int WordCount = 1;
			for(int i = 0; i < length; i++)
			{
				current = fullContent[i];
				if(char.IsWhiteSpace(current))
				{
					if(char.IsWhiteSpace(prevChar))
					{
						prevChar = current;
						continue;
					}
					else
					{
						WordCount++;
					}			
				}				
				prevChar = current;
			}
			return WordCount;
		}
	}
}
