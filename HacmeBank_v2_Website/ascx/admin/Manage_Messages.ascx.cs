namespace HacmeBank_v2_Website.ascx
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for PostMessageForm.
	/// </summary>
	public class AdminManageMessages : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataGrid dgPostedMessages;
		protected System.Web.UI.WebControls.Label lblMessage;
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.TextBox txtSubject;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.TextBox txtText;
		protected System.Web.UI.WebControls.Label lblError;
		protected System.Web.UI.WebControls.Button btnPostMessage;
		protected System.Web.UI.WebControls.Label lblPostedMessages;
		protected System.Web.UI.WebControls.Button btnNewMessage;
		protected System.Web.UI.WebControls.Label lblErrorMessage;

		private void Page_Load(object sender, System.EventArgs e)
		{			
			// Put user code to initialize the page here
			if (!IsPostBack)
			{
				LoadPostedMessages();
			}
		}

		private void LoadPostedMessages()
		{
//			lblPostedMessages.Text = "";
			dataClasses.postedMessage[] allPostedMessages = Global.objUsersCommunity.WS_GetPostedMessages("");

			DataTable dataTableWithPostedMessages = new DataTable();
			dataTableWithPostedMessages.Columns.Add("messageID");
			dataTableWithPostedMessages.Columns.Add("userID");
			dataTableWithPostedMessages.Columns.Add("messageDate");
			dataTableWithPostedMessages.Columns.Add("messageSubject");
			dataTableWithPostedMessages.Columns.Add("messageText");
			foreach (dataClasses.postedMessage objPostedMessage in allPostedMessages)
			{					
				dataTableWithPostedMessages.Rows.Add(new object[5] {	objPostedMessage.messageID,
																		objPostedMessage.userID,
																		objPostedMessage.messageDate,
																		objPostedMessage.messageSubject,
																		Server.HtmlEncode(objPostedMessage.messageText)});	
			}						
			dgPostedMessages.DataSource = dataTableWithPostedMessages;
			dgPostedMessages.DataBind();					
		}
	
		public void DeleteMessage(object oss,DataGridCommandEventArgs e)
		{			
			try
			{
				string messageToDelete = e.Item.Cells[0].Text;	
				if ( Global.objUsersCommunity.WS_DeleteMessage("",messageToDelete) > 0)
				{
					LoadPostedMessages();
					lblErrorMessage.Text= "Message #" + messageToDelete + " deleted";
				}
				else   
				{
					lblErrorMessage.Text= "Error deleting message #" +  messageToDelete;
				}
			}
			catch(Exception Ex)
			{
				lblErrorMessage.Text=Ex.Message;
			}
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{			
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
