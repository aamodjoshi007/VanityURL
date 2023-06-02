using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class cms_addSchedule : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(BasicFunction.GetConnectionstring());

    string str = string.Empty;
    static string sThumbImage = "";
    int Id;
    string strQuery = "";
    DataTable dTable;
    static string sImage = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Session["admin"] as string))
        {
            Response.Redirect("Login.aspx");
        }

           }

       protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("ViewSchedule.aspx");
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (isValidate())
        {
            con.Open();
            SqlCommand cmd_Update = new SqlCommand("sp_CMS_Schedule_Insert", con);
            cmd_Update.CommandType = CommandType.StoredProcedure;
            try
            {

                cmd_Update.Parameters.AddWithValue("@EventName", txtEventName.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@Date", txtEventDate.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@StartTime", txtEventStartTime.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@EndTime", txtEventEndTime.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@Location", txtEventLocation.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@Description", txtEventDescription.Text.Trim());
                cmd_Update.Parameters.AddWithValue("@UrlLink", txtEventUrlLink.Text.Trim());
               
                cmd_Update.ExecuteNonQuery();

                Response.Redirect("ViewSchedule.aspx");
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
            finally
            {
                cmd_Update.Dispose();
                con.Close();
                con.Dispose();
            }
        }
    }

    public bool isValidate()
    {
        lblEventName.Text = "";

        if (string.IsNullOrEmpty(txtEventName.Text.Trim()))
        {
            lblEventName.Text = "Please enter Event Name.";
            return false;
        }

        if (BasicFunction.ValidateEventName(txtEventName.Text.Trim()) == false)
        {
            lblEventName.Text = "Please enter Valid Event Name.";
            return false;
        }

        string format = "yyyy-MM-dd";
        DateTime parsedDate;

        if (DateTime.TryParseExact(txtEventDate.Text, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
        {
            // Date is valid and parsed successfully
        }
        else
        {
            lblEventDate.Text = "Please enter Valid Event Date.";
            return false;
        }

        if (BasicFunction.ValidateEventDate(txtEventDate.Text.Trim()) == false)
        {
            lblEventDate.Text = "Please enter Valid Event Date.";
            return false;
        }

        string pattern = @"^(0[0-9]|1[0-9]|2[0-3]):[0-5][0-9]$";
        bool isValid = System.Text.RegularExpressions.Regex.IsMatch(txtEventStartTime.Text, pattern);

        if (isValid)
        {
            // Time is valid
        }
        else
        {
            lblEventStartTime.Text = "Please enter Valid Event Start Time.";
            return false;
        }

        bool isValidEnd = System.Text.RegularExpressions.Regex.IsMatch(txtEventEndTime.Text, pattern);

        if (isValidEnd)
        {
            // Time is valid
        }
        else
        {
            lblEventEndTime.Text = "Please enter Valid Event End Time.";
            return false;
        }

        if (string.IsNullOrEmpty(txtEventLocation.Text.Trim()))
        {
            lblEventLocation.Text = "Please enter Event Location.";
            return false;
        }

        if (BasicFunction.ValidateLocationName(txtEventLocation.Text.Trim()) == false)
        {
            lblEventLocation.Text = "Please enter Valid Event Location.";
            return false;
        }

        if (BasicFunction.ValidateDescription(txtEventDescription.Text.Trim()))
        {
            lblEventDescription.Text = "Please Enter Valid Event Description.";
            return false;
        }

        return true;
    }
}