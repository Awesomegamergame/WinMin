using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace sethc.exe
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			string thisprocessname = Process.GetCurrentProcess().ProcessName;
			if (Process.GetProcesses().Count(p => p.ProcessName == thisprocessname) > 1)
				return;

			Application.Run(new Main());
		}
	}
}
