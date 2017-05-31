namespace HacmeBank_v2_Website.ascx
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for Loan.
	/// </summary>
	public class Loan : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label lblMessg;
		protected System.Web.UI.WebControls.Button btnSubmit;
		protected System.Web.UI.WebControls.TextBox txtAmount;
		protected System.Web.UI.WebControls.RangeValidator RangeValidator1;
		protected System.Web.UI.WebControls.DropDownList drpdwnCreditAccNo;					
		protected System.Web.UI.WebControls.DropDownList drpdwnLoanPeriodAndInterestRate;
		protected System.Web.UI.WebControls.Label lblHeading;
		protected System.Web.UI.WebControls.Label lblCreditAccNo;
		protected System.Web.UI.WebControls.Label lblLoanAmount;
		protected System.Web.UI.WebControls.Label lblUSD;
		protected System.Web.UI.WebControls.Label lblLoanPeriod;
		protected System.Web.UI.WebControls.Label lblYears;
		protected System.Web.UI.WebControls.Label lblRateofInterest;
		protected System.Web.UI.WebControls.Label lblPercentage;
		protected System.Web.UI.WebControls.Label lblConfirmMessage;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hlblCreditAccNo;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hlblDebitAccNo;
		protected System.Web.UI.HtmlControls.HtmlInputHidden hlblRate_Of_Interest;
		protected System.Web.UI.WebControls.TextBox txtComment;		
		protected System.Web.UI.WebControls.Label lblRate_Of_Interest;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			lblMessg.Text = "";
			if (!IsPostBack)
			{
				Global.objGui.populateDropDownListWithListOfUserAccounts(drpdwnCreditAccNo,(string)Session["userID"].ToString());
				Global.objGui.populateDropDownListWithLoanRates(drpdwnLoanPeriodAndInterestRate);
				// call drpdwnLoanPeriod_SelectedIndexChanged to update the rate value
				drpdwnLoanPeriodAndInterestRate_SelectedIndexChanged(null,null);
			}	
		}

		private void btnSubmit_Click(object sender, System.EventArgs e)
		{
			try
			{
				if (IsPostBack)
				{	
					if (btnSubmit.Text=="Submit")
					{
						if (txtAmount.Text=="") 
							lblMessg.Text="You have to enter a loan amount.<br/>";						
						else
						{
							string destinationAccount = drpdwnCreditAccNo.SelectedValue;
							int loanAmount = Int32.Parse(txtAmount.Text);
							int loanPeriod = Int32.Parse(drpdwnLoanPeriodAndInterestRate.SelectedItem.Text);
							decimal loanInterestRate =  Convert.ToDecimal(drpdwnLoanPeriodAndInterestRate.SelectedValue);
							string loanComment = txtComment.Text;
//							Response.Write (destinationAccount+ " : " +
//								loanAmount.ToString()+ " : " +
//								loanPeriod.ToString()+ " : " +
//								loanInterestRate.ToString() + " : " +
//								loanComment);
							Global.objAccountManagement.WS_RequestALoan("",destinationAccount,loanAmount,loanPeriod,loanInterestRate,loanComment);
							lblMessg.Text="Loan Successfully processed.<br/> The requested money was deposited in your account<br/>";						
						}
					}  
				}	 
			}	
			catch (Exception Ex) {
				lblMessg.Text = Ex.Message;
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
			this.drpdwnLoanPeriodAndInterestRate.SelectedIndexChanged += new System.EventHandler(this.drpdwnLoanPeriodAndInterestRate_SelectedIndexChanged);
			this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void drpdwnLoanPeriodAndInterestRate_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			lblRate_Of_Interest.Text = drpdwnLoanPeriodAndInterestRate.SelectedValue;			
		}
	}
}
