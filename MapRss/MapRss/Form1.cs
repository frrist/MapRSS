﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MapRss
{
    public partial class Form1 : Form
    {
        TreeView m_treeViewTopics;
        TreeView m_treeViewFeeds;
        DataGridView m_articleDataGridView;
        Rss.Reader rssReader = new Rss.Reader();

        public Form1()
        {
            InitializeComponent();


            m_treeViewTopics = PopulateTopicView();
            m_treeViewFeeds = PopulateFeedView();
            m_articleDataGridView = PopulateArticleDataGridView();

            foreach (TreeNode node in m_treeViewFeeds.Nodes)
            {
                treeViewBase.Nodes.Add((TreeNode)node.Clone());
            }

            treeViewBase.ExpandAll();
        }

        private void Feed_Click(object sender, EventArgs e)
        {
            treeViewBase.Nodes.Clear();

            foreach (TreeNode node in m_treeViewFeeds.Nodes)
            {
                treeViewBase.Nodes.Add((TreeNode)node.Clone());
            }

            treeViewBase.ExpandAll();
        }

        private void Map_Click(object sender, EventArgs e)
        {

        }

        private void Topic_Click(object sender, EventArgs e)
        {
            treeViewBase.Nodes.Clear();

            foreach (TreeNode node in m_treeViewTopics.Nodes)
            {
                treeViewBase.Nodes.Add((TreeNode)node.Clone());
            }

            treeViewBase.ExpandAll();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void addFeed_Click(object sender, EventArgs e)
        {
            //Create new form to be used as popup entry method
            Form formUrl = new Form();
            formUrl.Location = new Point(Size.Width / 2, Size.Height / 2);
            formUrl.Text = "RSS URL";
            formUrl.ClientSize = new System.Drawing.Size(500, 150);

            //Create text and button
            Label textLabel = new Label() { Left = 50, Top = 20, Text = "Please enter URL:" };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button add = new Button() { Text = "Add", Left = 350, Width = 100, Top = 70 };
            
            //Add event
            add.Click += (senderClick, eArg) => { formUrl.Close(); AddFeed(textBox.Text); };
            formUrl.Controls.Add(textBox);
            formUrl.Controls.Add(add);
            formUrl.Controls.Add(textLabel);
            formUrl.AcceptButton = add;
            formUrl.ShowDialog();
        }

        private void removeFeed_Click(object sender, EventArgs e)
        {

        }

     
        private void treeViewBase_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeView test = sender as TreeView;
            
            ArticleDataGridView.Visible = true;

            string name = test.SelectedNode.Text;

            List<Rss.Article> articles = rssReader.GetArticles(name);
            List<Rss.Article> BindList = new List<Rss.Article>();
            foreach (var article in articles)
            {
                BindList.Add(new Rss.Article()
                {
                    Title = article.Title.ToString(),
                    Description = article.Description.ToString(),
                    Date = article.Date,
                    Link = article.Link.ToString(),
                    Location = article.Location.ToString()

                });
            }
            m_articleDataGridView.DataSource = BindList;

        }

        //This will need to load from the XML save file to actually populate the topic area
        private TreeView PopulateTopicView()
        {
            TreeView topics = new TreeView();

            //TreeNode[] topicAreas = new TreeNode[] { new TreeNode("Sports"), new TreeNode("Heath")};

            //TreeNode root = new TreeNode("Topics", topicAreas);

            //topics.Nodes.Add(root);

            return topics;
        }

        //This will need to load from the XML save file to actually populate the topic area
        private TreeView PopulateFeedView()
        {
            TreeView feed = new TreeView();
            feed.BeginUpdate();
            feed.EndUpdate();
            return feed;
        }
        private DataGridView PopulateArticleDataGridView()
        {
            ArticleDataGridView.Visible = false;
            ArticleDataGridView.VirtualMode = true;
            ArticleDataGridView.AutoSize = true;
            ArticleDataGridView.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            ArticleDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            ArticleDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            ArticleDataGridView.AllowUserToAddRows = false;
            ArticleDataGridView.AllowUserToDeleteRows = false;
            DataGridViewCheckBoxColumn CheckBoxColumn = new DataGridViewCheckBoxColumn();
            {
                CheckBoxColumn.Name = "Read";
                CheckBoxColumn.FlatStyle = FlatStyle.Standard;
                CheckBoxColumn.ThreeState = false;
                CheckBoxColumn.CellTemplate = new DataGridViewCheckBoxCell();
                CheckBoxColumn.CellTemplate.Style.BackColor = Color.Beige;
                

            }
            ArticleDataGridView.Columns.Insert(0, CheckBoxColumn);
            return ArticleDataGridView;



        }


        private void AddFeed(string url)
        {
            if(0 < url.Length)
            {
                rssReader.AddFeed(url, url);
                
                m_treeViewFeeds.Nodes.Add(new TreeNode(url));

                //This will just refresh the feed topics
                Feed_Click(null, null);

                //webBrowser1.Url = new Uri(rssReader.GetFeedUrl());
                //webBrowser1.Refresh();
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListView selectedItem = sender as ListView;
            string articleUrl = selectedItem.FocusedItem.SubItems[2].Text;


            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.Url = new Uri(articleUrl);
            webBrowser1.Refresh();
            
     
            
            
        }

        private void webBrowserBack_Button_Click(object sender, EventArgs e)
        {
            webBrowser1.GoBack();
        }
        private void webBrowserFoward_Button_Click(object sender, EventArgs e)
        {
            webBrowser1.GoForward();
        }
        private void webBrowserRefresh_Button_Click(object sender, EventArgs e)
        {
            webBrowser1.Refresh();
        }
        private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
        {
            try
            {
                webBrowserProgressBar.Value = Convert.ToInt32(e.CurrentProgress);
                webBrowserProgressBar.Maximum = Convert.ToInt32(e.MaximumProgress);
            }
            catch (Exception ex)
            {
            }
        }
        private void webBrowserTextOnly_Button_Click(object sender, EventArgs e)
        {
            //TODO: Figure out how to do this...
        }

      

   
    }
}
