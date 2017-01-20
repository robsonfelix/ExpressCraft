﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.Html5;
using Bridge.jQuery2;
using Bridge.Text.RegularExpressions;

namespace ExpressCraft
{
	public static class Helper
	{
		public class DataTableJson
		{
			public string[] fieldNames = null;
			public object[][] rows = null;
			public DataType[] dataTypes = null;

			public static DataTableJson FromExternal(object o)
			{
				// #TODO - Get Namespace auto..
				var sw = Stopwatch.StartNew();				
				var obj = Script.Write<DataTableJson>("Bridge.merge(Bridge.createInstance(ExpressCraft.Helper.DataTableJson), o);");
				
				sw.Stop();

				Console.WriteLine("FromExternal: " + sw.ElapsedMilliseconds);

				return obj;
			}

			public static DataTable Parse(dynamic o)
			{
				DataTable dt = new DataTable();
				var length = o.fieldNames.length;
				for(int i = 0; i < length; i++)
				{
					dt.AddColumn(o.fieldNames[i], o.dataTypes[i]);
				}
				if(o.rows != null)
				{
					length = o.rows.length;
					dt.BeginNewRow(length);

					for(int i = 0; i < length; i++)
					{
						var dr = dt.NewRow();
						dr.batchData = o.rows[i];
					}
					dt.AcceptNewRows();
				}
				return dt;
			}            

			public DataTable ToTable()
			{
				var sw = Stopwatch.StartNew();

				var dt = new DataTable();

				for(int i = 0; i < fieldNames.Length; i++)
				{
					dt.AddColumn(fieldNames[i], dataTypes[i]);
				}

				if(rows != null)
				{
					dt.BeginNewRow(rows.Length);

					for(int i = 0; i < rows.Length; i++)
					{
						var dr = dt.NewRow();
						dr.batchData = rows[i];
					}
					dt.AcceptNewRows();
				}

				sw.Stop();
				Console.WriteLine("ToTable: " + sw.ElapsedMilliseconds);

				return dt;
			}
		}

        public static int IsTrue(this string value)
        {
            return ((value = value.ToLower()) == "true" || value == "1" || value == "on") ? 1 : 0;
        }

        public static bool IsNumber(this object value)
		{
			return value is sbyte
					|| value is byte
					|| value is short
					|| value is ushort
					|| value is int
					|| value is uint
					|| value is long
					|| value is ulong
					|| value is float
					|| value is double
					|| value is decimal;
		}

		public static void Empty(this HTMLElement element)
		{
			/*@
			var len = element.childNodes.length;
			while(len--)
			{
				element.removeChild(element.lastChild);
			};
			*/
		}

		public static Point GetClientMouseLocation(object e)
		{
			var x = 0;
			var y = 0;
			/*@			  
			  if (!e) var e = window.event;

			  if (e.pageX || e.pageY) {
				x = e.pageX;
				y = e.pageY;
			  } else if (e.clientX || e.clientY) {
				x = e.clientX + document.body.scrollLeft + 
								   document.documentElement.scrollLeft;
				y = e.clientY + document.body.scrollTop + 
								   document.documentElement.scrollTop;
			  }			  
			*/
			return new Point(x, y);
		}

		public static void SetChecked(this Control input, object value)
		{
			input.Content.SetChecked(value);			
		}

		public static void SetChecked(this HTMLElement input, object value)
		{
			bool check = false;
			if(value != null)
			{
				if(value is bool || value.IsNumber())
				{
					check = (bool)value;					
				}
				else if(value is string)
				{
					string strValue = ((string)value);
					check = (strValue == "1" || string.Compare(strValue.ToLower(), "true") == 0);					
				}
			}
			if(!check)
			{
				input.RemoveAttribute(GridViewCellDisplayCheckBox.resource_checked);
			}
			else
			{
				input.SetAttribute(GridViewCellDisplayCheckBox.resource_checked, null);
			}			
		}
		
		/// <summary>
		/// IE does not support .remove on Element use delete
		/// </summary>
		/// <param name="c"></param>
		public static void Delete(this Element c)
		{
			jQuery.Select(c).Remove();
		}		

		public static string ToPx(this float i)
		{
			return Script.Write<string>("i + 'px'");			
		}

		public static string ToPx(this int i)
		{
			return Script.Write<string>("i + 'px'");
		}

		public static string ToPx(this decimal i)
		{
			return Script.Write<string>("i + 'px'");
		}

		public static void Log(object jso)
		{
			Script.Call("console.log", jso);
		}
		
		public static void AppendChildren(this Node c, params Node[] Nodes)
        {
            if(Nodes != null && Nodes.Length > 0)
            {
                for (int i = 0; i < Nodes.Length; i++)
                {
                    c.AppendChild(Nodes[i]);
                }
            }
        }

		public static void AppendChildrenTabIndex(this Node c, params Control[] Nodes)
		{
			if(Nodes != null && Nodes.Length > 0)
			{
				for(int i = 0; i < Nodes.Length; i++)
				{
					Nodes[i].Content.TabIndex = i;
					c.AppendChild(Nodes[i]);
				}
			}
		}

		public static void AppendChildren(this Node c, params Control[] Nodes)
		{
			if(Nodes != null && Nodes.Length > 0)
			{
				for(int i = 0; i < Nodes.Length; i++)
				{
					c.AppendChild(Nodes[i]);
				}
			}
		}

		public static void SetBounds(this HTMLElement c, int left, int top, int width, int height)
		{
			c.SetBounds(left.ToPx(), top.ToPx(), width.ToPx(), height.ToPx());
		}

		public static void SetBounds(this HTMLElement c, decimal left, decimal top, decimal width, decimal height)
		{
			c.SetBounds(left.ToPx(), top.ToPx(), width.ToPx(), height.ToPx());
		}

		public static void SetBounds(this HTMLElement c, float left, float top, float width, float height)
		{
			c.SetBounds(left.ToPx(), top.ToPx(), width.ToPx(), height.ToPx());
		}

		public static void SetSize(this HTMLElement c, int width, int height)
		{
			c.SetSize(width.ToPx(), height.ToPx());
		}

		public static void SetBounds(this Control c, int left, int top, int width, int height)
		{
			c.Content.SetBounds(left.ToPx(), top.ToPx(), width.ToPx(), height.ToPx());
		}

		public static void SetSize(this Control c, int width, int height)
		{
			c.Content.SetSize(width.ToPx(), height.ToPx());
		}

		public static void SetBounds(this Control c, string left, string top, string width, string height)
		{
			c.Content.SetBounds(left, top, width, height);			
		}

		public static void SetBoundsFull(this Control c)
		{
			c.Content.SetBoundsFull();
		}

		public static void SetBoundsFull(this HTMLElement c)
		{
			c.SetBounds("0", "0", "100%", "100%");
		}

		public static void SetSize(this Control c, string width, string height)
		{
			c.Content.SetSize(width, height);
		}    

        public static void SetBounds(this HTMLElement c, string left, string top, string width, string height)
		{
			c.Style.Left = left;
			c.Style.Top = top;
			c.Style.Width = width;
			c.Style.Height = height;
		}

        public static void SetImage(this Control c, string str, bool useURL = true)
        {
            if(!str.StartsWith("url("))
            {
                str = useURL ? Control.GetImageStringURI(str) : Control.GetImageString(str);
            }
            SetImage(c.Content, str, useURL);
        }

        public static void SetImage(this HTMLElement c, string str, bool useURL = true)
        {
            if(string.IsNullOrWhiteSpace(str))
            {
                c.Style.Background = "";
                c.Style.BackgroundSize = "";
                return;
            }
            else if (!str.StartsWith("url("))
            {
                str = useURL ? Control.GetImageStringURI(str) : Control.GetImageString(str);
            }
            c.Style.Background = str;
            c.Style.BackgroundSize = "100% 100%";
        }

        public static void SetSize(this HTMLElement c, string width, string height)
		{
			c.Style.Width = width;
			c.Style.Height = height;
		}

        public static void SetLocation(this Control c, int left, int top)
        {
            c.Content.SetLocation(left.ToPx(), top.ToPx());
        }

        public static void SetLocation(this Control c, string left, string top)
        {
            c.Content.SetLocation(left, top);
        }

		public static void SetLocation(this HTMLElement c, decimal left, decimal top)
		{
			c.SetLocation(left.ToPx(), top.ToPx());
		}

		public static void SetLocation(this HTMLElement c, float left, float top)
		{
			c.SetLocation(left.ToPx(), top.ToPx());
		}

		public static void SetLocation(this HTMLElement c, int left, int top)
        {
            c.SetLocation(left.ToPx(), top.ToPx());
        }

        public static void SetLocation(this HTMLElement c, string left, string top)
        {
            c.Style.Left = left;
            c.Style.Top = top;
        }

		/// <summary>
		/// HtmlEscape XSS
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string HtmlEscape(this object obj)
		{
			return (obj as string).HtmlEscape();			
		}

		/// <summary>
		/// HtmlUrlUnescape XSS
		/// </summary>
		/// <returns></returns>
		public static string HtmlUrlUnescape(this string input)
		{
			return !string.IsNullOrEmpty(input)
				? input
					.Replace("&amp", "&")
					.Replace("&lt", "<")
					.Replace("&gt", ">")
					.Replace("&#x27", "'")					
				: "";
		}

		/// <summary>
		/// HtmlUrlEscape XSS
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string HtmlUrlEscape(this string input)
		{
			return !string.IsNullOrEmpty(input)
				? input
					.Replace("&", "&amp")
					.Replace("<", "&lt")
					.Replace(">", "&gt")
					.Replace("'", "&#x27")					
				: string.Empty;
		}

		/// <summary>
		/// HtmlEscape XSS
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string HtmlEscape(this string input) {
			return !string.IsNullOrEmpty(input) ? 
				HtmlUrlEscape(input).Replace(@"\/", "&#x2F").Replace("\"", "&quot") : 
				string.Empty;			
        }

		/// <summary>
		/// HtmlUnescape XSS
		/// </summary>
		/// <returns></returns>
		public static string HtmlUnescape(this string input) {
			return !string.IsNullOrEmpty(input) ?
				HtmlUrlUnescape(input).Replace("&#x2F", @"\/").Replace("&quot", "\"") :
				string.Empty;
        }
    }
}