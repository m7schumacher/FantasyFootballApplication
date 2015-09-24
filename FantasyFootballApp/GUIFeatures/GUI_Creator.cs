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
using System.Threading;

namespace FantasyFootball.cs
{
    public static class GUI_Creator
    {
        public static Label atlas_Create(double height, double width)
        {
            Label can = new Label();
            can.Background = Brushes.Transparent;

            can.Height = height;
            can.Width = width;

            can.BorderBrush = Brushes.Gray;
            can.BorderThickness = new Thickness(1, 0, 1, 1);

            Canvas vas = new Canvas();

            vas.Height = can.Height;
            vas.Width = can.Width;

            vas.Background = Brushes.Transparent;

            Ellipse outer = new Ellipse();
            outer.Width = 74;
            outer.Height = 74;
            outer.Stroke = Brushes.LimeGreen;
            outer.StrokeThickness = 3;
            outer.Fill = Brushes.DarkGreen;

            Ellipse inner = new Ellipse();
            inner.Width = 54;
            inner.Height = 54;
            inner.Stroke = Brushes.Green;
            inner.StrokeThickness = 1;
            inner.Fill = Brushes.Gray;

            TextBox box = new TextBox();
            box.Width = vas.Width - outer.Width - 35;
            box.Background = Brushes.Black;
            box.Foreground = Brushes.White;
            box.BorderBrush = Brushes.DarkGreen;
            box.BorderThickness = new Thickness(2);
            box.Height = 20;
            box.HorizontalContentAlignment = HorizontalAlignment.Center;
            box.KeyDown += box_KeyDown;

            Label lab = new Label();
            lab.Foreground = Brushes.WhiteSmoke;
            lab.Content = "ATLAS";
            lab.FontSize = 26;
            lab.Background = Brushes.Transparent;
            lab.FontFamily = new FontFamily("Segoe UI");
            lab.FontWeight = FontWeights.Thin;
            
            Canvas.SetTop(lab, 0);
            Canvas.SetLeft(lab, 180);

            Canvas.SetTop(outer, 0);
            Canvas.SetLeft(outer, 0);

            Canvas.SetTop(inner, 10);
            Canvas.SetLeft(inner, 10);

            Canvas.SetLeft(box, 90);
            Canvas.SetTop(box, 50);

            vas.Children.Add(outer);
            vas.Children.Add(inner);
            vas.Children.Add(box);
            vas.Children.Add(lab);

            can.Content = vas;

            return can;
        }

        static void box_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                string input = box.Text;
                Task doc = Task.Factory.StartNew(() => Brain.SendMessageTCP(input));

                box.Text = string.Empty;
            } 
        }

        public static TextBox textBox_Create()
        {
            TextBox next = new TextBox();

            next.Foreground = Brain.foregroundTheme;
            next.Background = Brushes.Transparent;
            next.FontSize = 20;
            next.Width = 80;
            next.FontFamily = new FontFamily("Meiryo");
            next.HorizontalContentAlignment = HorizontalAlignment.Center;
            next.VerticalContentAlignment = VerticalAlignment.Center;
            next.VerticalAlignment = VerticalAlignment.Center;
            next.Opacity = 0;
            next.BorderThickness = new Thickness(2);
            next.BorderBrush = Brain.foregroundTheme;
            next.Margin = new Thickness(8);

            return next;
        }

        public static ListBox listBox_Create()
        {
            ListBox next = new ListBox();

            next.Foreground = Brain.foregroundTheme;
            next.Background = Brushes.Transparent;
            next.FontSize = 20;
            next.Width = 400;
            next.FontFamily = new FontFamily("Meiryo");
            next.HorizontalContentAlignment = HorizontalAlignment.Left;
            next.VerticalContentAlignment = VerticalAlignment.Center;
            next.VerticalAlignment = VerticalAlignment.Center;
            next.Opacity = 0;
            next.BorderThickness = new Thickness(2);
            next.BorderBrush = Brain.foregroundTheme;
            next.Margin = new Thickness(8);

            return next;
        }

        public static Button Button_Create(string content)
        {
            Button next = new Button();

            next.Content = content;
            next.Foreground = Brushes.White;
            next.Background = Brushes.Transparent;
            next.FontSize = 35;
            next.FontFamily = new FontFamily("Yu Gothic Light");
            next.HorizontalContentAlignment = HorizontalAlignment.Center;
            next.VerticalContentAlignment = VerticalAlignment.Center;
            next.VerticalAlignment = VerticalAlignment.Center;
            next.Opacity = 0;
            next.BorderThickness = new Thickness(2);
            next.BorderBrush = Brain.foregroundTheme;
            next.Margin = new Thickness(10);
            next.Width = 500;

            Color first = Color.FromRgb(0, 51, 0);
            Color second = Color.FromRgb(0, 0, 0);
            LinearGradientBrush back = new LinearGradientBrush(first, second, 50);
            next.Background = back;

            return next;
        }

        public static Label label_Create(string content)
        {
            Label next = new Label();

            next.Content = content;
            next.Foreground = Brain.foregroundTheme;
            next.Background = Brushes.Transparent;
            next.FontSize = 35;
            next.FontFamily = new FontFamily("Meiryo");
            next.HorizontalContentAlignment = HorizontalAlignment.Center;
            next.VerticalContentAlignment = VerticalAlignment.Center;
            next.VerticalAlignment = VerticalAlignment.Center;
            next.Opacity = 0;
            next.BorderThickness = new Thickness(2);
            next.BorderBrush = Brain.foregroundTheme;
            next.Margin = new Thickness(8);

            return next;
        }

        public static Label label_Create(string content, string name, double width, double height, double fontsize, Brush foreground)
        {
            Label next = new Label();

            next.Content = content;
            next.Foreground = foreground;
            next.Background = Brushes.Transparent;
            next.FontSize = fontsize;
            next.FontFamily = new FontFamily("Meiryo");
            next.Name = name;
            next.Width = width;
            next.Height = height;

            next.HorizontalContentAlignment = HorizontalAlignment.Center;
            next.VerticalContentAlignment = VerticalAlignment.Center;
            next.VerticalAlignment = VerticalAlignment.Center;

            next.Opacity = 0;

            next.BorderThickness = new Thickness(2);
            next.BorderBrush = Brain.foregroundTheme;
            next.Margin = new Thickness(8);

            return next;
        }
    }
}
