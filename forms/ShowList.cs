﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Project.Forms.Layouts;
using Project.Helpers;
using Project.Models;
using Project.Services;

namespace Project.Forms {

    public class ShowList : BaseLayout {

        // Frontend
        private ListView container;
        private Label title;

        private Button newButton;

        public ShowList() {
            InitializeComponent();
        }

        public override string GetHandle() {
            return "showList";
        }

        public override void OnShow() {
            Program app = Program.GetInstance();
            ShowService showService = app.GetService<ShowService>("shows");
            List<Show> shows = showService.GetShows();

            base.OnShow();

            container.Items.Clear();
            
            for (int i = 0; i < shows.Count; i++) {
                Show show = shows[i];
                Movie movie = show.GetMovie();
                Room room = show.GetRoom();
                ListViewItem item = new ListViewItem(movie.name + " - Nummer " + room.number + " - " + show.startTime.ToString(Program.DATETIME_FORMAT), i);

                item.Tag = show.id;
                container.Items.Add(item);
            }
        }

        private void InitializeComponent() {
            this.container = new System.Windows.Forms.ListView();
            this.newButton = new System.Windows.Forms.Button();
            this.title = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // container
            // 
            this.container.HideSelection = false;
            this.container.Location = new System.Drawing.Point(40, 174);
            this.container.Name = "container";
            this.container.Size = new System.Drawing.Size(670, 430);
            this.container.TabIndex = 2;
            this.container.UseCompatibleStateImageBehavior = false;
            this.container.Click += new System.EventHandler(this.ListItem_Click);
            // 
            // newButton
            // 
            this.newButton.Location = new System.Drawing.Point(40, 638);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(140, 23);
            this.newButton.TabIndex = 3;
            this.newButton.Text = "Nieuw";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.ButtonNew_Click);
            // 
            // title
            // 
            this.title.AutoEllipsis = true;
            this.title.AutoSize = true;
            this.title.BackColor = System.Drawing.SystemColors.Control;
            this.title.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F);
            this.title.Location = new System.Drawing.Point(32, 112);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(300, 46);
            this.title.TabIndex = 5;
            this.title.Text = "Voorstelling lijst";
            // 
            // ShowList
            // 
            this.ClientSize = new System.Drawing.Size(1262, 673);
            this.Controls.Add(this.title);
            this.Controls.Add(this.newButton);
            this.Controls.Add(this.container);
            this.Name = "ShowList";
            this.Load += new System.EventHandler(this.MovieList_Load);
            this.Controls.SetChildIndex(this.container, 0);
            this.Controls.SetChildIndex(this.newButton, 0);
            this.Controls.SetChildIndex(this.title, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MovieList_Load(object sender, System.EventArgs e) {
            container.View = View.Details;
            container.Columns.Add("Voorstellingen (film - zaal - starttijd)", 600);
        }

        private void ListItem_Click(object sender, EventArgs e) {
            Program app = Program.GetInstance();
            ShowService showService = app.GetService<ShowService>("shows");

            // Get the clicked item
            ListViewItem item = container.SelectedItems[0];

            if(item == null) {
                GuiHelper.ShowError("Geen item geselecteerd");
                return;
            }

            // Find the show
            int id = (int) item.Tag;
            Show show = showService.GetShowById(id);

            if(show == null) {
                GuiHelper.ShowError("Kon geen voorstelling vinden voor dit item");
                return;
            }

            // Redirect to screen
            ShowDetail showDetail = app.GetScreen<ShowDetail>("showDetail");
            showDetail.SetShow(show);
            app.ShowScreen(showDetail);
        }

        private void ButtonNew_Click(object sender, EventArgs e) {
            Program app = Program.GetInstance();
            ShowCreate newScreen = app.GetScreen<ShowCreate>("showCreate");
            app.ShowScreen(newScreen);
        }

    }

}
