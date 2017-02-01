﻿using System;
using Bridge;
using Bridge.Html5;
using ExpressCraft;

namespace ExpressCraftDesign
{
    public class App
    {
        public static void Main()
        {
			Form.Setup();
			AceCodeEditor.Setup();

			var studio = new StudioForm();
			studio.Show();

		}
    }
}