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

namespace FantasyFootball.cs
{
    public class Ticker
    {
        const double gap = 50; // pixel gap between each TextBlock
        const int timer_interval = 2; // number of ms between timer ticks. 16 is near 1/60th second, for smoother updates on LCD displays
        const double move_amount = 1.5; // number of pixels to move each timer tick. 1 - 1.5 is ideal, any higher will introduce judders

        Canvas ticker;

        private LinkedList<TextBlock> textBlocks = new LinkedList<TextBlock>();
        private Timer timer = new Timer();

        public Ticker(Canvas tick)
        {
            ticker = tick;
        }

        public void UpdateTicker()
        {
            textBlocks.Clear();
            string ticker = string.Empty;

            if (Brain.lastFivePlayersDrafted.Count > 0)
            {
                ticker += "LAST FIVE:  ";

                foreach (Player p in Brain.lastFivePlayersDrafted)
                {
                    ticker += (p.First.ElementAt(0) + ". " + p.Last + ", " + p.Position + " ($" + p.Dollar + ")   ");
                }
            }

            ticker += "    BEST QB:   " + addToTicker("QB");

            ticker += "    BEST RB:   " + addToTicker("RB");

            ticker += "    BEST WR:   " + addToTicker("WR");

            AddTextBlock(ticker);
        }

        private string addToTicker(string pos)
        {
            string tick = string.Empty;

            var best =
              from p in Brain.MyDraft.Players
              where p.Position.Equals(pos)
              orderby p.Points descending
              select p;

            if (best.Any())
            {
                for (int i = 0; i < 3; i++)
                {
                    tick += best.ElementAt(i).First.ElementAt(0) + ". " + best.ElementAt(i).Last + " ($" + best.ElementAt(i).Dollar + ")   ";
                }
            }
            else
            {
                tick += "None";
            }

            return tick;
        }

        public void startTicker()
        {
            AddTextBlock("Welcome");

            var node = textBlocks.First;

            while (node != null)
            {
                if (node.Previous != null)
                {
                    Canvas.SetLeft(node.Value, Canvas.GetLeft(node.Previous.Value) + node.Previous.Value.ActualWidth + gap);
                }
                else
                {
                    Canvas.SetLeft(node.Value, 750);
                }

                node = node.Next;
            }

            timer.Interval = timer_interval;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
        }

        public void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            ticker.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
            {
                var node = textBlocks.First;
                var lastNode = textBlocks.Last;

                while (node != null)
                {
                    double newLeft = Canvas.GetLeft(node.Value) - move_amount;

                    if (newLeft < (0 - (node.Value.ActualWidth + gap)))
                    {
                        textBlocks.Remove(node);

                        var lastNodeLeftPos = Canvas.GetLeft(lastNode.Value);

                        if (!node.Value.Text.Equals("Welcome"))
                        {
                            textBlocks.AddLast(node);
                        }

                        if ((lastNodeLeftPos + lastNode.Value.ActualWidth + gap) > ticker.Width) // Last element is offscreen
                        {
                            UpdateTicker();
                        }
                        else
                        {
                            UpdateTicker();
                        }
                    }

                    Canvas.SetLeft(node.Value, newLeft);

                    node = node == lastNode ? null : node.Next;
                }

                return null;

            }), null);
        }

        void AddTextBlock(string Text)
        {
            ticker.Children.Clear();

            TextBlock tb = new TextBlock();
            tb.Text = Text;
            tb.FontSize = 24;
            tb.FontWeight = FontWeights.Normal;
            tb.Foreground = Brushes.White;
            tb.FontFamily = new FontFamily("Moire Light");
            tb.VerticalAlignment = VerticalAlignment.Center;

            ticker.Children.Add(tb);

            var node = textBlocks.Last;

            if (node != null)
            {
                double starter = Canvas.GetLeft(node.Value);

                Canvas.SetLeft(tb, 738);
            }
            else
            {
                Canvas.SetTop(tb, 0);
                Canvas.SetLeft(tb, 738);
            }

            textBlocks.AddLast(tb);
        }

        Timer fadeIn = new Timer();
    }
}
