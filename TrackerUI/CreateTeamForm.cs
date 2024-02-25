using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetAllPersons();

        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        private ITeamRequester callingForm;

        private void WireUp()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }
        private void CreateSampleData()
        {
            availableTeamMembers.Add(new PersonModel { FirstName= "John", LastName = "Wang"});
            availableTeamMembers.Add(new PersonModel { FirstName = "Jessie", LastName = "Jin" });

            selectedTeamMembers.Add(new PersonModel { FirstName = "Gene", LastName = "Wang" });
            selectedTeamMembers.Add(new PersonModel { FirstName = "David", LastName = "Wang" });
        }
        public CreateTeamForm(ITeamRequester caller)
        {
            InitializeComponent();
            // CreateSampleData();
            this.callingForm = caller;
            WireUp();
            
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                // create a PersonModel and fill all the data
                // save PersonModel back to database
                // clean all the fields
                PersonModel p = new PersonModel();
                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.Email = emailValue.Text;
                p.cellphoneNumber = cellphoneValue.Text;

                p = GlobalConfig.Connection.CreatePerson(p);
                selectedTeamMembers.Add(p);

                WireUp();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
                cellphoneValue.Text = "";
            }
            else
            {
                MessageBox.Show("Please fill in all the field.");
            }
        }

        private bool ValidateForm()
        {
            if(firstNameValue.Text.Length == 0)
            {
                return false;
            }
            if(lastNameValue.Text.Length == 0)
            {
                return false;
            }
            if(emailValue.Text.Length == 0)
            {
                return false;
            }
            if(cellphoneValue.Text.Length == 0)
            {
                return false;
            }
            return true;
        }

        private void addMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            // remove the selected member from dropdown, then add it into listbox
            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WireUp(); 
            }
        }

        private void deleteSelectedMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;

            // remove the selected member from list, then add it into dropdown
            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WireUp(); 
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            // TODO - we need to fill up two tables on the database, Teams and TeamMembers. 
            // We need to fill the Team table first, then get the team id. And then, fill the
            // TeamMembers with team id to TeamMembers table.
            TeamModel t = new TeamModel();

            t.TeamName = teamNameValue.Text;

            t.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);

            this.Close();

            // TODO - If we aren't closing this form after creation, reset the form

        }
    }
}
