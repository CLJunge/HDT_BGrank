﻿using System;
using Hearthstone_Deck_Tracker.API;
using Hearthstone_Deck_Tracker.Plugins;
using System.Windows.Controls;

namespace HDT_BGrank
{
    public class BGrankPlugin : IPlugin
    {
        internal BGrank rank;
        public MenuItem MenuItem { get; private set; }
        private LeaderBoardPanel leaderBoardPanel;

        public string Author
        {
            get { return "IBM5100"; }
        }

        public string ButtonText
        {
            get { return "No Settings"; }
        }

        public string Description
        {
            get { return "A battleground plugin searchs opponents' MMR for you."; }
        }

        public string Name
        {
            get { return "HDT_BGrank"; }
        }

        public void OnButtonPress()
        {
        }

        public void OnLoad()
        {
            CreateMenuItem();
            MenuItem.IsChecked = true;
            rank = new BGrank();
            GameEvents.OnGameStart.Add(rank.OnGameStart);
            GameEvents.OnTurnStart.Add(rank.OnTurnStart);
        }

        public void OnUnload()
        {
            MenuItem.IsChecked = false;
        }

        public void OnUpdate()
        {
            rank.OnUpdate();
            leaderBoardPanel.OnUpdate(rank);
        }

        private void CreateMenuItem()
        {
            MenuItem = new MenuItem()
            {
                Header = "BGrank"
            };

            MenuItem.IsCheckable = true;

            MenuItem.Checked += async (sender, args) =>
            {
                if (leaderBoardPanel == null)
                {
                    leaderBoardPanel = new LeaderBoardPanel();
                    Core.OverlayCanvas.Children.Add(leaderBoardPanel);
                }
            };

            MenuItem.Unchecked += (sender, args) =>
            {
                using (leaderBoardPanel)
                {
                    Core.OverlayCanvas.Children.Remove(leaderBoardPanel);
                    leaderBoardPanel = null;
                }
            };
        }

        public Version Version
        {
            get { return new Version(1, 0, 2); }
        }

    }
}