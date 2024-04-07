﻿using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Utility.Extensions;

namespace HDT_BGrank
{
    public partial class LeaderBoardPanel : UserControl, IDisposable
    {
        private bool finished = false;
        private bool isDragging = false;
        private Point originalGridPosition;
        private Point originalMousePosition;

        public LeaderBoardPanel()
        {
            InitializeComponent();
            OverlayExtensions.SetIsOverlayHitTestVisible(LeaderText, true);
            OverlayExtensions.SetIsOverlayHitTestVisible(DeleteButton, true);
            OverlayExtensions.SetIsOverlayHitTestVisible(HiddenButton, true);
            Visibility = Visibility.Hidden;
        }

        public void OnUpdate(BGrank rank)
        {
            if (Core.Game.IsInMenu)
            {
                Visibility = Visibility.Hidden;
                finished = false;
            }
            else if (!finished && rank.done)
            {
                int i = 0;
                string allText = "\n";
                if (rank.failToGetData) { allText += "Fail to get data"; }
                else
                {
                    foreach (var opp in rank.oppDict)
                    {
                        allText += opp.Key + " " + opp.Value;
                        i++;
                        if (i < rank.oppDict.Count) { allText += "\n"; }
                    }
                }
                LeaderText.Text = allText;
                finished = true;
                Visibility = Visibility.Visible;
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void HiddenButton_Click(object sender, RoutedEventArgs e)
        {
            if (LeaderText.IsVisible)
            {
                LeaderText.Visibility = Visibility.Hidden;
            }
            else
            {
                LeaderText.Visibility = Visibility.Visible;
            }
        }

        private void LeaderText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = true;
                originalMousePosition = e.GetPosition(this);
                originalGridPosition = new Point(LeaderGrid.Margin.Left, LeaderGrid.Margin.Top);
            }
        }

        private void LeaderText_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                isDragging = false;
            }
        }

        private void LeaderText_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(this);
                double offsetX = currentPosition.X - originalMousePosition.X;
                double offsetY = currentPosition.Y - originalMousePosition.Y;

                double newLeft = originalGridPosition.X + offsetX;
                double newTop = originalGridPosition.Y + offsetY;
                if (newLeft < 0) { newLeft = 0; }
                if (newTop < 0) { newTop = 0; }

                Thickness newMargin = new Thickness(newLeft, newTop, LeaderGrid.Margin.Right, LeaderGrid.Margin.Bottom);
                LeaderGrid.Margin = newMargin;
            }
        }

        public void Dispose()
        {
        }
    }
}
