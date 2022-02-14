using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5B
{
    public partial class Form1 : Form
    {
        SqlConnection Connection;
        // Connecting to the data
        string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=COMP10204_LAB5;Integrated Security=True";
        // three list for staring data of doctor, companion and episode for linq operations
        List<Doctor> doctorList = new List<Doctor>();
        List<Companion> companionList = new List<Companion>();
        List<Episode> episodeList = new List<Episode>();

        public Form1()
        {
            InitializeComponent();
            SqlSetUp();                                 // method connecting to the database
            StoringValueInList();                       // getting data and stroing in Lists
            DoctorIdSetUp();                            // storing Ids into combo box
        }
        /// <summary>
        /// Connecting to the database and update status with red and green text color
        /// </summary>
        public void SqlSetUp()
        {
            try
            {
                Connection = new SqlConnection();
                Connection.ConnectionString = connectionString;
                Connection.Open(); DBStatusLabel.ForeColor = Color.Green;
                DBStatusLabel.Text = ("Connected to Database Successfully");
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = ($"Error connecting to DB {ex.Message}");
                return;
            }
        }
        /// <summary>
        /// Storing the data from database to partocular list 
        /// </summary>
        public void StoringValueInList()
        {
            //seperate method for every differnet List/table
            DoctorData();
            CompanionData();
            EpisodeData();
        }
        /// <summary>
        /// Whatever index(id) changed in ComboBox, using it to fill all other fields 
        /// -- SQL  -> Every field is filled in seperate method
        /// -- LINQ -> Used two method - One for left groupBox Doctor data    &      One for right groupBox Companion data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoctorIDComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            CompanionListBox.Items.Clear();                                     //Clearing List Box text
            int id = DoctorIDComboBox.SelectedIndex + 1;                        //getting id

            if (SQLradioButton.Checked)                                         // if user wants process with SQL
            {
                DoctorName(id);
                DoctorYear(id);
                DoctorSeries(id);
                DoctorAge(id);
                DoctorFirstEpisode(id);
                DoctorImage(id);
                CompanionsDataForOutput(id);
            }
            else if (LINQRadioButton.Checked)                                     // if user wants process with LINQ
            {
                LINQDoctorData(id);
                LINQCampanionData(id);
            }
        }
        /// <summary>
        /// Doctor ids into comBOx
        /// </summary>
        public void DoctorIdSetUp()
        {
            SqlDataReader reader = null;             // If exception rises in last method then current reader can start with new data as filling null reader
            try
            {
                SqlCommand command = new SqlCommand("Select DOCTORID FROM DOCTOR", Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    DoctorIDComboBox.Items.Add($"{reader[0].ToString(),50}");           //Adding data(ids) into comboBOx
                }

            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "DoctorID: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's name text box
        /// </summary>
        /// <param name="selectedDocId">Doctorid of selected actor</param>
        public void DoctorName(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                string query = "Select ACTOR FROM DOCTOR WHERE DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    PlayedByTextBox.Text = reader[0].ToString();
                }

            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor name: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's Debut Year text box
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void DoctorYear(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                // using join to coonect two table with storyId & debutID and got year
                string query = "Select SEASONYEAR from EPISODE e Join DOCTOR d ON e.STORYID=d.DEBUT Where DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    YearTextBox.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor Debut-Year: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's Series text box
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void DoctorSeries(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                string query = "Select Series FROM DOCTOR WHERE DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SeriesTextBox.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor series: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's Age text box
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void DoctorAge(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                string query = "Select AGE FROM DOCTOR WHERE DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    AgeTextBox.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor series: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's First Episode Title text box
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void DoctorFirstEpisode(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                // using joins for get title of first Doctor Id
                string query = "Select Title from EPISODE e Join DOCTOR d ON e.STORYID=d.DEBUT Where DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FirstEpisodeTextBox.Text = reader[0].ToString();
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor Debut-Year: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Actor's Image with using provided code from MyCanvas
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void DoctorImage(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                string query = "Select PICTURE FROM DOCTOR WHERE DOCTORID=" + selectedDocId;
                SqlCommand command = new SqlCommand(query, Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    byte[] photo = (byte[])reader["Picture"];
                    MemoryStream stream = new MemoryStream(photo);
                    Image image = Image.FromStream(stream);
                    DoctorPictureBox.Image = Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting actor series: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Filling Comanion List Box
        ///  --> joining companion and episode table and get required details in 'order' of Season year and title
        ///  --> using list for in nice formatting 
        /// </summary>
        /// <param name="selectedDocId">DoctorId of selected actor</param>
        public void CompanionsDataForOutput(int selectedDocId)
        {
            SqlDataReader reader = null;
            try
            {
                string queryName = "Select * from COMPANION c JOIN EPISODE e ON e.STORYID=c.STORYID Where c.DOCTORID=" + selectedDocId + " order by SEASONYEAR, TITLE";

                SqlCommand command = new SqlCommand(queryName, Connection);
                reader = command.ExecuteReader();
                List<string> companionDetails = new List<string>();                     // for doctor name and actor
                List<string> episodeDetails = new List<string>();                       // for season year and title
                List<string> storyIds = new List<string>();                             // for priting the data from tis length which id common in both table
                while (reader.Read())
                {
                    companionDetails.Add($"{reader["NAME"].ToString()}  ({reader["ACTOR"].ToString()})");
                    episodeDetails.Add($"{reader["TITLE"].ToString()} ({ reader["SEASONYEAR"].ToString()})");
                    storyIds.Add(reader["STORYID"].ToString());
                }
                // Adding to List box
                for (int i = 0; i < storyIds.Count; i++)
                {
                    CompanionListBox.Items.Add($"\n {companionDetails[i]} ");
                    CompanionListBox.Items.Add($" \"{episodeDetails[i]}\"  ");
                    CompanionListBox.Items.Add($"     ");
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.ForeColor = Color.Red;
                DBStatusLabel.Text = "Selecting Campanion Name: Database operation failed: " + ex.Message;
            }
            reader.Close();
        }
        /// <summary>
        /// Data for doctor details for left group box
        /// --> Using LINQ
        /// --> Selecting required field using selected id and priting it out in textBoxes
        /// </summary>
        /// <param name="selectedDocID">DoctorId of selected actor</param>
        public void LINQDoctorData(int selectedDocID)
        {
            var doctorName =                                                                 // for name
                from d in doctorList
                where d.DoctorID == selectedDocID
                select d.Actor;
            PlayedByTextBox.Text = "" + doctorName.First().ToString();

            var doctorYear =                                                                 // for year
                from e in episodeList                                                               // Joining table
                join dr in doctorList on e.StoryId equals dr.Debut
                where dr.DoctorID == selectedDocID
                select e.SeasonYear;
            YearTextBox.Text = "" + doctorYear.First().ToString();

            var doctorSeries =                                                               // for series
                 from d in doctorList
                 where d.DoctorID == selectedDocID
                 select d.Series;
            SeriesTextBox.Text = "" + doctorSeries.First().ToString();

            var doctorAge =                                                                  // for age
                 from d in doctorList
                 where d.DoctorID == selectedDocID
                 select d.Age;
            AgeTextBox.Text = "" + doctorAge.First().ToString();

            var doctorFirstEpisode =                                                         // for first episode title
                 from e in episodeList                                                                   // Joining table
                 join dr in doctorList
                 on e.StoryId equals dr.Debut
                 where dr.DoctorID == selectedDocID
                 select e.Title;
            FirstEpisodeTextBox.Text = "" + doctorFirstEpisode.First().ToString();

            var doctorImage =                                                                // for Image
                  from d in doctorList
                  where d.DoctorID == selectedDocID
                  select d.Picture;
            DoctorPictureBox.Image = doctorImage.First();
        }
        /// <summary>
        /// Data for Companion details for right side group box
        /// --> Using LINQ
        /// --> Selecting required field using selected id and priting it out in textBoxes
        /// --> Using Join for connect companion and episode
        /// </summary>
        /// <param name="selectedDocID">DoctorId of selected actor</param>
        public void LINQCampanionData(int selectedDocID)
        {
            var companionDetails =                                                           // for name
                from c in companionList
                join e in episodeList
                on c.StoryID equals e.StoryId
                where c.DoctorID == selectedDocID
                orderby e.SeasonYear, e.Title
                select new { c.Name, c.Actor, e.Title, e.SeasonYear };                      // selecting multiple items 

            //Adding result in list Box
            foreach (var v in companionDetails)
            {
                CompanionListBox.Items.Add($"{v.Name} ({v.Actor})");
                CompanionListBox.Items.Add($" \"{v.Title}\" ({v.SeasonYear})");
                CompanionListBox.Items.Add(" ");
            }
        }
        /// <summary>
        /// Selecting all data from database and storing it into list of Doctors
        /// </summary>
        public void DoctorData()
        {
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand("Select * FROM DOCTOR", Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    //code for image (from myCanvas)
                    byte[] photo = (byte[])(reader["PICTURE"]);
                    MemoryStream stream = new MemoryStream(photo);
                    Image image = Image.FromStream(stream);

                    doctorList.Add(new Doctor(int.Parse(reader[0].ToString()), reader[1].ToString(), int.Parse(reader[2].ToString()), int.Parse(reader[3].ToString()), reader[4].ToString(), image));
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.Text = "Doctor data in List: Database operation failed: " + ex.Message;
            }
            reader.Close();

        }
        /// <summary>
        /// Selecting all data from database and storing it into list of Companions
        /// </summary>
        public void CompanionData()
        {
            SqlDataReader reader = null;
            try
            {
                SqlCommand command = new SqlCommand("Select * FROM COMPANION", Connection);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    companionList.Add(new Companion(reader[0].ToString(), reader[1].ToString(), int.Parse(reader[2].ToString()), reader[3].ToString()));
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.Text = "Companion data in List for OUTPUT: Database operation failed: " + ex.Message;
            }
            reader.Close();

        }

        /// <summary>
        /// Selecting all data from database and storing it into list of Episodes
        /// </summary>
        public void EpisodeData()
        {
            SqlDataReader reader2 = null;
            try
            {
                SqlCommand command2 = new SqlCommand("Select * FROM EPISODE", Connection);
                reader2 = command2.ExecuteReader();
                while (reader2.Read())
                {
                    episodeList.Add(new Episode(reader2[0].ToString(), reader2[1].ToString(), int.Parse(reader2[2].ToString()), reader2[3].ToString()));
                }
            }
            catch (Exception ex)
            {
                DBStatusLabel.Text = "Episode data in List: Database operation failed: " + ex.Message;
            }
            reader2.Close();

        }
        /// <summary>
        /// Closing the application with Menu Strip - 'Fill'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
