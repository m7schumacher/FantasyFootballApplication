using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.VisualBasic;
using System.Timers;
using System.Reflection;
using System.IO;
using System.Windows.Media.Animation;


namespace FantasyFootball.cs
{
    public static class GUI_Animations
    {
        static DoubleAnimation fadeOut;
        static DoubleAnimation fadeIn;

        public static bool menuGenerated = false;

        static MainWindow window;

        public static void Initialize(MainWindow win)
        {
            fadeOut = new DoubleAnimation(0, TimeSpan.FromSeconds(3));
            fadeOut.Completed += Completed_FadeOut;

            fadeIn = new DoubleAnimation(1, TimeSpan.FromSeconds(3));
            window = win;
        }

        static void Completed_FadeOut(object sender, EventArgs e)
        {
            ViewEditor.actionEnumerator.MoveNext();

            if (ViewEditor.actionEnumerator.Current != null)
            {
                ViewEditor.actionEnumerator.Current();
            }
        }

        public static void Fade_All_Controls()
        {
            foreach (UIElement c in Brain.Controls)
            {
                c.Opacity = 0;
            }
        }

        public static void Fade_Out_Control(Control c, int seconds)
        {
            Duration dur = new Duration(new TimeSpan(0, 0, 0, 0, seconds));
            fadeOut.Duration = dur;
            c.BeginAnimation(Control.OpacityProperty, fadeOut);
        }

        public static void Fade_In_Control(UIElement c, int seconds)
        {
            Duration dur = new Duration(new TimeSpan(0, 0, 0, 0, seconds));
            fadeIn.Duration = dur;
            c.BeginAnimation(Control.OpacityProperty, fadeIn);
        } 
    }
}
